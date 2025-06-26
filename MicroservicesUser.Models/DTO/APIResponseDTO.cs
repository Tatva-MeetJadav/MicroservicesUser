using Newtonsoft.Json;

namespace MicroservicesUser.Models.DTO
{
    public class APIResponseDTO<T>
    {
        [JsonProperty("apiResponse")]
        public APIResponseMetaDataDTO? apiResponseMetaData { get; set; }

        [JsonProperty("result")]
        public T? Result { get; set; }
    }
}
