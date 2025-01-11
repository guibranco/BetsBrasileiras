using System.Text.RegularExpressions;

namespace BetsBrasileiras.Helpers;

internal static class Patterns
{
    public static readonly Regex SpaPattern = new Regex(
        @"(?<applicationNumber>\d{4})\s+(?<applicationYear>\d{4})\s+(?<name>.+)\s+(?<document>\d{3}\.\d{3}\.\d{3}-\d{2})\s+(?<brand>.+)\s+(?<domain>.+)"
    );
}
