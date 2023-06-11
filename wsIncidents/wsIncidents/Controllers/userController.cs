using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using wsIncidents.Models;
using wsIncidents.Repositories;

namespace wsIncidents.Controllers
{

    [ApiController]
    [ApiVersion("1.0")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("v{version:apiVersion}/[controller]")]
    public class userController : Controller
    {

        private readonly IConfiguration config;

        public userController(IConfiguration configuration)
        {
            config = configuration;
        }

        /// <summary>
        /// Method to create a new user
        /// </summary>
        /// <param name="model">Json structure</param>
        /// <returns>responseModel</returns>
        [MapToApiVersion("1.0")]
        [HttpPost("create"), Authorize]
        [
            SwaggerResponse(200, "If data has saved ", typeof(responseModel)),
            SwaggerResponse(400, "If not saved or connection is lost, or something is wrong")
        ]
        public async Task<responseModel> create(userModel model)
        {

            userRepository repository = new userRepository();

            return await repository.create(model);
        }

    }
}


