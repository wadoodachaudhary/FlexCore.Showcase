using Fx.ControlKit.Reports;

namespace FlexCore.Showcase.Services;

/// <summary>
/// Showcase stand-in for <see cref="IReportSessionContext"/>.
/// Returns hard-coded demo values for any parameter the report's SQL references
/// but the caller didn't supply (e.g. DivisionID). Real apps return values from
/// their session-state service / claims principal.
/// </summary>
public class ShowcaseReportSessionContext : IReportSessionContext
{
    public object? Get(string parameterName) => parameterName.ToLowerInvariant() switch
    {
        "divisionid" => 1,
        "userid"     => "showcase",
        _            => null,
    };
}
