using System.Text;
using System.Text.Json;

public class OllamaApiService
{
    private readonly HttpClient _httpClient;

    public OllamaApiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> PullModelAsync(string ollamaUrl, string modelName)
    {
        var requestUri = $"{ollamaUrl}/api/pull";
        var jsonContent = $"{{ \"name\": \"{modelName}\" }}";
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(requestUri, content);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        // split the responseString into an array of string with a new element for each appearance of the string '{"status":'
        var responseArray = responseString.Split("{\"status\":");

        // remote the char '}' from the each element of the array
        var responseHtml = "";
        for (int i = 0; i < responseArray.Length; i++)
        {
            responseArray[i] = responseArray[i].Replace("}", "");
            responseHtml += $"<p>{responseArray[i]}</p>";
        }


        return responseHtml;
    }
}
