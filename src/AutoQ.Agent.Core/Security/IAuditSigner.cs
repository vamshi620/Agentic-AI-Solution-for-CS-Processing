namespace AutoQ.Agent.Core.Security;

public interface IAuditSigner
{
    string Sign(string payload);
}
