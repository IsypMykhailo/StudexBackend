namespace Studex.Services;

public interface IAIContentGenerator
{
    Task<string> GenerateContentAsync(string prompt);
}