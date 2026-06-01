namespace AutoQ.Agent.Core.Security;

public interface IPhiSanitizer
{
    bool ContainsPotentialPhi(string text);
    string Redact(string text);
}
