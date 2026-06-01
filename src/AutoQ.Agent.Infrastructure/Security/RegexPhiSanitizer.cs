namespace AutoQ.Agent.Infrastructure.Security;

using System.Text.RegularExpressions;
using AutoQ.Agent.Core.Security;

public sealed class RegexPhiSanitizer : IPhiSanitizer
{
    private static readonly Regex PhiRegex = new(
        @"(\b\d{3}-\d{2}-\d{4}\b)|(\b\d{9}\b)|(\bMRN\s*\d+\b)|(\bDOB\s*\d{2}/\d{2}/\d{4}\b)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public bool ContainsPotentialPhi(string text) => !string.IsNullOrEmpty(text) && PhiRegex.IsMatch(text);

    public string Redact(string text) => string.IsNullOrEmpty(text) ? text : PhiRegex.Replace(text, "[REDACTED]");
}
