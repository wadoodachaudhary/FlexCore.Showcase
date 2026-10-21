// 2026-10-13: sign-up / question form backing store. Submissions are appended as JSON lines to a file:
//   - on Azure App Service (WEBSITE_SITE_NAME set): $HOME/data/flexcore-signups.jsonl, which survives deploys and restarts
//   - elsewhere: App_Data/signups.jsonl under the content root (git-ignored)
// Override with "Signups:Path" in configuration. Email delivery of the Community edition link is not wired yet; see tracker SC-274.
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace FlexCore.Showcase.Services.Signups;

/// <summary>What the form binds to. Labels come from <see cref="DisplayAttribute"/> through FormItemControl.</summary>
public sealed class SignupRequest
{
    [Required(ErrorMessage = "Please tell us your name."), StringLength(80)]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "We need an email address to send the link to."), EmailAddress(ErrorMessage = "That does not look like an email address."), StringLength(160)]
    [Display(Name = "Email")]
    public string Email { get; set; } = "";

    [StringLength(120)]
    [Display(Name = "Company (optional)")]
    public string? Company { get; set; }

    [StringLength(2000)]
    [Display(Name = "Question, or what you are building (optional)")]
    public string? Message { get; set; }

    [Display(Name = "Email me the Community edition download when it is ready")]
    public bool WantsDownload { get; set; } = true;
}

public sealed record SignupRecord(Guid Id, DateTimeOffset ReceivedUtc, string Name, string Email, string? Company, string? Message, bool WantsDownload, string Source);

public sealed class SignupOptions
{
    /// <summary>Full path of the JSON-lines file. Optional; see the file header for the defaults.</summary>
    public string? Path { get; set; }
}

public interface ISignupStore
{
    Task<SignupRecord> SaveAsync(SignupRequest request, string source, CancellationToken cancellationToken = default);
}

public sealed class FileSignupStore : ISignupStore
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);
    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly string _path;
    private readonly ILogger<FileSignupStore> _log;

    public FileSignupStore(IOptions<SignupOptions> options, IHostEnvironment env, ILogger<FileSignupStore> log)
    {
        _log = log;
        _path = options.Value.Path ?? DefaultPath(env);
    }

    public string Path => _path;

    private static string DefaultPath(IHostEnvironment env)
    {
        var home = Environment.GetEnvironmentVariable("HOME");
        var onAppService = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
        return onAppService && !string.IsNullOrEmpty(home)
            ? System.IO.Path.Combine(home, "data", "flexcore-signups.jsonl")
            : System.IO.Path.Combine(env.ContentRootPath, "App_Data", "signups.jsonl");
    }

    public async Task<SignupRecord> SaveAsync(SignupRequest request, string source, CancellationToken cancellationToken = default)
    {
        var record = new SignupRecord(Guid.NewGuid(), DateTimeOffset.UtcNow, request.Name.Trim(), request.Email.Trim(),
            Blank(request.Company), Blank(request.Message), request.WantsDownload, source);
        var line = JsonSerializer.Serialize(record, Json) + Environment.NewLine;
        await _gate.WaitAsync(cancellationToken);
        try
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_path)!);
            await File.AppendAllTextAsync(_path, line, cancellationToken);
        }
        finally { _gate.Release(); }
        // No personal data in the log: the id is enough to find the line in the file.
        _log.LogInformation("Sign-up {Id} stored (download requested: {WantsDownload})", record.Id, record.WantsDownload);
        return record;
    }

    private static string? Blank(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
