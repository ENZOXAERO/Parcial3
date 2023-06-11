using System;
using System.Net;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace wsIncidents.Models {

    public class tokenResponseModel {

        [SwaggerSchema("Status code http")]
        public HttpStatusCode status { get; set; }

        [DataType(DataType.Text)]
        [SwaggerSchema("Message resposne")]
        public string message { get; set; }

        [DataType(DataType.Text)]
        [SwaggerSchema("Result reponse (Token)")]
        public string result { get; set; }

        [DataType(DataType.DateTime)]
        [SwaggerSchema("Time to expire token",Format = "date")]
        public DateTime expire { get; set; }

        [SwaggerSchema("User information")]
        public userModel user { get; set; }

    }

}
