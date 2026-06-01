namespace AutoQ.Agent.Core.Utilities;

using System.Security.Cryptography;
using System.Text;
using AutoQ.Agent.Core.Domain;

public static class IdempotencyKey
{
    public static string For(SpecialistContext ctx, string suffix)
    {
        var raw = $"{ctx.JobId}:{ctx.CustomSolutionId}:{suffix}";
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
