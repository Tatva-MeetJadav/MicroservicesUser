namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface IGenericAPIClientServices
    {
        Task<TResponse> PostAsync<TRequest,TResponse>(TRequest request, string baseUrl);
    }
}
