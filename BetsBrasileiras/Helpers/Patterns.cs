using System.Text.RegularExpressions;

namespace BetsBrasileiras.Helpers;

/// <summary>
/// Class Patterns.
/// </summary>
internal static class Patterns
{
    /// <summary>
    /// The flags
    /// </summary>
    private static RegexOptions _flags =
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

    /// <summary>
    /// The spa full line pattern
    /// </summary>
    public static readonly Regex SpaFullLinePattern = new(
        @"(?<applicationNumber>\d{4})\/(?<applicationYear>\d{4})\s+(?<name>.+)\s+(?<document>\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2})\s+(?<brand>.+?)\s(?<domain>.+)$",
        _flags
    );

    /// <summary>
    /// The spa brand domain pattern
    /// </summary>
    public static readonly Regex SpaBrandDomainPattern = new(
        @"(?<brand>.+?)\s(?<domain>.+)$",
        _flags
    );
}
