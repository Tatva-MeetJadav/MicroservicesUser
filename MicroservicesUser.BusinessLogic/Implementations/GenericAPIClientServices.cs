using MicroservicesUser.BusinessLogic.Interfaces;
using System.Text.Json;
using System.Text;

namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class GenericAPIClientServices : IGenericAPIClientServices
    {
        private readonly HttpClient _httpClient;
        public GenericAPIClientServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request, string baseUrl)
        {
            try
            {
                string json = JsonSerializer.Serialize(request);
                using StringContent? content = new(json, Encoding.UTF8, "application/json");

                // Send POST request
                using HttpResponseMessage? response = await _httpClient.PostAsync(baseUrl, content);
                response.EnsureSuccessStatusCode();

                // Read and deserialize the response content
                await using Stream? responseStream = await response.Content.ReadAsStreamAsync();
                TResponse? result = await JsonSerializer.DeserializeAsync<TResponse>(responseStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new InvalidOperationException("Deserialized response was null.");
                return result;
            }
            catch (HttpRequestException httpEx)
            {
                // Handle request-level errors (connection issues, 4xx, 5xx)
                throw new InvalidOperationException($"HTTP request failed: {httpEx.Message}", httpEx);
            }
            catch (JsonException jsonEx)
            {
                // Handle deserialization issues
                throw new InvalidOperationException($"Failed to deserialize response: {jsonEx.Message}", jsonEx);
            }
            catch (Exception ex)
            {
                // Catch-all for other issues
                throw new InvalidOperationException("Unexpected error during API call.", ex);
            }
        }

    }
}
