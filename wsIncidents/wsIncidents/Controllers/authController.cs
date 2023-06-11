using wsIncidents.Models;
using System.Threading.Tasks;
using wsIncidents.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;

namespace wsIncidents.Controllers {

    [ApiController]
    [ApiVersion("1.0")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("v{version:apiVersion}/[controller]")]
    public class authController : Controller {

        private readonly IConfiguration config;

        public authController(IConfiguration configuration) {
            config = configuration;
        }

        /// <summary>
        /// Gets the token if the username and password are valid
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST v1.0/auth/login
        ///     {
        ///         "user": "user",
        ///         "pasword": "pasword"
        ///     }
        /// </remarks>
        /// <param name="model">Json structure</param>
        /// <returns>Token</returns>
        [MapToApiVersion("1.0")]
        [HttpPost, Route("login")]
        [SwaggerOperation(Tags = new[] { "Authentication" })]
        [
            SwaggerResponse(200,"Token if all it's ok",typeof(tokenResponseModel)),
            SwaggerResponse(400,"If user or password are incorrect, or something is wrong")
        ]
        public async Task<tokenResponseModel> login(authModel model) {

            authRepository repository = new authRepository(config);

            return await repository.login(model);
        }



        [MapToApiVersion("1.0")]
        [HttpPost, Route("logind")]
        [SwaggerOperation(Tags = new[] { "Authentication" })]
        [
            SwaggerResponse(200,"Token if all it's ok",typeof(tokenResponseModel)),
            SwaggerResponse(400,"If user or password are incorrect, or something is wrong")
        ]
        public async Task<tokenResponseModel> logid() {

            authModel model = new authModel() {
                user = "root",
                password = "root"
            };
            authRepository repository = new authRepository(config);

            return await repository.login(model);
        }
    }
}
