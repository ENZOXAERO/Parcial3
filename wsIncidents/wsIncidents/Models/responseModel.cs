using System.Net;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace wsIncidents.Models {

    public class responseModel {

        [JsonProperty("status")]
        [SwaggerSchema("Status code http")]
        public HttpStatusCode status { get; set; }

        [JsonProperty("code")]
        [SwaggerSchema("Inserted code from database")]
        public int code { get; set; }

        [JsonProperty("message")]
        [SwaggerSchema("Response menssage")]
        public string message { get; set; }

        public object reponse { get; set; }
      
    }
}
