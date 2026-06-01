namespace AutoQ.Agent.Infrastructure.Audit;

using System.Security.Cryptography;
using System.Text;
using AutoQ.Agent.Core.Security;
using AutoQ.Agent.Infrastructure.Options;
using Microsoft.Extensions.Options;

public sealed class HmacAuditSigner : IAuditSigner
{
    private readonly byte[] _key;

    public HmacAuditSigner(IOptions<AuditSigningOptions> options)
    {
        var raw = options.Value.SigningKey;
        _key = string.IsNullOrWhiteSpace(raw)
            ? RandomNumberGenerator.GetBytes(32)
            : Encoding.UTF8.GetBytes(raw);
    }

    public string Sign(string payload)
    {
        using var hmac = new HMACSHA256(_key);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
