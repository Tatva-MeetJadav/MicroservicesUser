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
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send POST request
                using HttpResponseMessage response = await _httpClient.PostAsync(baseUrl, content);
                response.EnsureSuccessStatusCode();

                // Read and deserialize the response content
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                TResponse? result = await JsonSerializer.DeserializeAsync<TResponse>(responseStream);
                return result ?? Activator.CreateInstance<TResponse>();
            }
            catch(Exception ex)
            {
                throw new Exception("Something went wrong!",ex.InnerException);
            }
            
        }
    }
}
