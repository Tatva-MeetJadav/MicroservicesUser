using Newtonsoft.Json;

namespace MicroservicesUser.Models.DTO
{
    public class APIResponse<T>
    {
        [JsonProperty("apiResponse")]
        public APIResponseMetaData? apiResponseMetaData { get; set; }

        [JsonProperty("result")]
        public T? Result { get; set; }
    }
}
