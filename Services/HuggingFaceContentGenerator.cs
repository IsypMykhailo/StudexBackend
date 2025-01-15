namespace Studex.Services;

public class HuggingFaceContentGenerator : IAIContentGenerator
{
    private readonly HttpClient _httpClient;
    private readonly string _model;
    
    public HuggingFaceContentGenerator(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {configuration["HuggingFace:ApiKey"]}");
        _model = configuration["HuggingFace:Model"] ?? "gpt-neo-2.7B";
    }
    public async Task<string> GenerateContentAsync(string prompt)
    {
        var request = new
        {
            inputs = prompt,
            parameters = new
            {
                max_length = 1500,
                temperature = 0.7,
                top_k = 50,
                top_p = 0.9
            }
        };

        var response = await _httpClient.PostAsJsonAsync($"https://api-inference.huggingface.co/models/{_model}", request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Hugging Face API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
        }

        var result = await response.Content.ReadFromJsonAsync<HuggingFaceResponse>();
        return result?.GeneratedText ?? string.Empty;
    }
}

public class HuggingFaceResponse
{
    public string? GeneratedText { get; set; }
}