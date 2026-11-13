using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Fx.ControlKit.Reports;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace FlexCore.Showcase.Services;

public sealed class AtlasRunContext
{
    public bool Enabled { get; set; }
    public bool LayoutOnly { get; set; }
}

/// <summary>Read-only access to the public, offline-seeded demonstration database.</summary>
public sealed class AtlasDatabase(IWebHostEnvironment environment)
{
    public string DatabasePath => Path.Combine(environment.WebRootPath, "reports", "ai-atlas", "data", "atlas.db");
    public static readonly string[] Tables = ["Sources", "Countries", "Organizations", "Milestones", "Models", "Benchmarks", "ModelScores", "Industries", "AdoptionMetrics", "Scenarios", "EconomicImpacts", "ComputeSites"];
    public static readonly string[] Views = ["AtlasHistory", "AtlasPlayers", "AtlasModels", "AtlasAdoption", "AtlasEconomics", "AtlasCompute"];
    private static readonly HashSet<string> Readable = new(Tables.Concat(Views), StringComparer.OrdinalIgnoreCase);
    private static readonly HashSet<string> Functions = new(["abs", "avg", "coalesce", "count", "date", "datetime", "ifnull", "iif", "instr", "julianday", "length", "like", "lower", "ltrim", "max", "min", "nullif", "printf", "replace", "round", "rtrim", "strftime", "substr", "substring", "sum", "total", "trim", "upper", "row_number", "rank", "dense_rank"], StringComparer.OrdinalIgnoreCase);

    public SqliteConnection Open()
    {
        var connection = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = DatabasePath, Mode = SqliteOpenMode.ReadOnly, Pooling = false }.ToString());
        connection.Open();
        return connection;
    }

    public IReadOnlyList<ReportDesignerDataTable> Schema()
    {
        using var connection = Open();
        return Tables.Concat(Views).Select(name =>
        {
            var table = new ReportDesignerDataTable { Name = name, SourceName = name };
            using var command = connection.CreateCommand(); command.CommandText = $"PRAGMA table_info([{name}])";
            using var reader = command.ExecuteReader();
            while (reader.Read()) table.Fields.Add(new() { Table = name, Name = reader.GetString(1), Type = reader.GetString(2) is "REAL" or "INTEGER" ? "NumberField" : "StringField" });
            return table;
        }).ToArray();
    }

    public DataTable Browse(string table)
    {
        if (!Readable.Contains(table)) throw new InvalidDataException("Select an AI Atlas table or view.");
        return Execute($"SELECT * FROM [{table}] LIMIT 100", null);
    }

    public DataTable Execute(string sql, IDictionary<string, object>? parameters)
    {
        if (sql.Length > 100_000) throw new InvalidDataException("Report query exceeds 100 KB.");
        // The shared designer emits a leading TOP for its bounded data preview.
        // This is syntax adaptation only; SQLite's authorizer enforces security.
        var top = Regex.Match(sql, @"\A\s*SELECT\s+TOP\s+(\d+)\s+", RegexOptions.IgnoreCase);
        if (top.Success) sql = "SELECT " + sql[top.Length..].TrimEnd().TrimEnd(';') + " LIMIT " + Math.Clamp(int.Parse(top.Groups[1].Value), 1, 1000);
        using var connection = Open();
        using (var setup = connection.CreateCommand()) { setup.CommandText = "PRAGMA query_only=ON; PRAGMA trusted_schema=OFF;"; setup.ExecuteNonQuery(); }
        var handle = connection.Handle!;
        raw.sqlite3_limit(handle, raw.SQLITE_LIMIT_LENGTH, 1_000_000);
        raw.sqlite3_limit(handle, raw.SQLITE_LIMIT_SQL_LENGTH, 100_000);
        raw.sqlite3_limit(handle, raw.SQLITE_LIMIT_COLUMN, 256);
        raw.sqlite3_limit(handle, raw.SQLITE_LIMIT_EXPR_DEPTH, 100);
        raw.sqlite3_limit(handle, raw.SQLITE_LIMIT_COMPOUND_SELECT, 32);
        raw.sqlite3_set_authorizer(handle, (_, action, p0, p1, database, _) =>
        {
            if (action is raw.SQLITE_SELECT or raw.SQLITE_RECURSIVE) return raw.SQLITE_OK;
            if (action == raw.SQLITE_READ && database.utf8_to_string() == "main" && Readable.Contains(p0.utf8_to_string())) return raw.SQLITE_OK;
            if (action == raw.SQLITE_FUNCTION && Functions.Contains(p1.utf8_to_string())) return raw.SQLITE_OK;
            return raw.SQLITE_DENY;
        }, null);
        var clock = Stopwatch.StartNew(); var instructions = 0;
        raw.sqlite3_progress_handler(handle, 1000, _ => ++instructions > 10_000 || clock.Elapsed > TimeSpan.FromSeconds(3) ? 1 : 0, null);
        using var command = connection.CreateCommand(); command.CommandText = sql; command.CommandTimeout = 5;
        foreach (var pair in parameters ?? new Dictionary<string, object>())
            command.Parameters.AddWithValue("@" + pair.Key.TrimStart('@'), pair.Value ?? DBNull.Value);
        try
        {
            using var reader = command.ExecuteReader();
            var result = new DataTable();
            for (var i = 0; i < reader.FieldCount; i++) result.Columns.Add(reader.GetName(i), typeof(object));
            while (reader.Read())
            {
                if (result.Rows.Count >= 10_000) throw new InvalidDataException("Report query exceeds 10,000 rows. Narrow its filters.");
                var values = new object[reader.FieldCount]; reader.GetValues(values); result.Rows.Add(values);
            }
            if (reader.NextResult()) throw new InvalidDataException("Only one report result set is permitted.");
            return result;
        }
        catch (SqliteException error)
        {
            throw new InvalidDataException("AI Atlas accepts bounded, read-only queries against its sample tables. External report schemas must be mapped in the designer, or viewed in Layout only mode. " + error.Message, error);
        }
    }
}
