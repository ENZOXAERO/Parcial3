using wsIncidents.Models;
using System.Threading.Tasks;
using wsIncidents.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace wsIncidents.Controllers {

    [ApiController]
    [ApiVersion("1.0")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("v{version:apiVersion}/[controller]")]

    public class incidentController : Controller {

        /// <summary>
        /// Method to create a new incident
        /// </summary>
        /// <param name="model">Json structure</param>
        /// <returns>responseModel</returns>
        [MapToApiVersion("1.0")]
        [HttpPost("create"), Authorize]
        [
            SwaggerResponse(200,"If data has saved ",typeof(responseModel)),
            SwaggerResponse(400,"If not saved or connection is lost, or something is wrong")
        ]
        public async Task<responseModel> create(incidentModel model) {

            incidentRepository repository = new incidentRepository();

            return await repository.create(model);
        }

        /// <summary>
        /// Method to create a new incident detail
        /// </summary>
        /// <param name="model">Json structure</param>
        /// <returns>responseModel</returns>
        [MapToApiVersion("1.0")]
        [HttpPost("createDetail"), Authorize]
        [
            SwaggerResponse(200,"If data has saved ",typeof(responseModel)),
            SwaggerResponse(400,"If not saved or connection is lost, or something is wrong")
        ]
        public async Task<responseModel> createDetail(incidentDetailsModel model) {

            incidentRepository repository = new incidentRepository();

            return await repository.createDetail(model);
        }

        /// <summary>
        /// Method to create a new incident detail
        /// </summary>
        /// <param name="model">Json structure</param>
        /// <returns>responseModel</returns>
        [MapToApiVersion("1.0")]
        [HttpPost("updateDetail"), Authorize]
        [
            SwaggerResponse(200,"If data has update ",typeof(responseModel)),
            SwaggerResponse(400,"If not updated or connection is lost, or something is wrong")
        ]
        public async Task<responseModel> updateDetail(incidentDetailsModel model) {

            incidentRepository repository = new incidentRepository();

            return await repository.updateDetail(model);
        }

        

        /// <summary>
        /// Method to update an incident
        /// </summary>
        /// <param name="model">Json structure</param>
        /// <returns>responseModel</returns>
        [MapToApiVersion("1.0")]
        [HttpPost("update"), Authorize]
        [
            SwaggerResponse(200,"If data has updated ",typeof(responseModel)),
            SwaggerResponse(400,"If not updated or connection is lost, or something is wrong")
        ]
        public async Task<responseModel> update(incidentModel model) {

            incidentRepository repository = new incidentRepository();

            return await repository.update(model);
        }

        /// <summary>
        /// Method to delete an incident
        /// </summary>
        /// <param name="model">Json structure</param>
        /// <returns>responseModel</returns>
        [MapToApiVersion("1.0")]
        [HttpPost("delete"), Authorize]
        [
            SwaggerResponse(200,"If data has deleted ",typeof(responseModel)),
            SwaggerResponse(400,"If not deleted or connection is lost, or something is wrong")
        ]
        public async Task<responseModel> delete(incidentModel model) {

            incidentRepository repository = new incidentRepository();

            return await repository.delete(model);
        }

        /// <summary>
        /// Method to asign group
        /// </summary>
        /// <param name="model">Json structure</param>
        /// <returns>responseModel</returns>
        [MapToApiVersion("1.0")]
        [HttpPost("createGroup"), Authorize]
        [
            SwaggerResponse(200,"If data has saved ",typeof(responseModel)),
            SwaggerResponse(400,"If not saved or connection is lost, or something is wrong")
        ]
        public async Task<responseModel> createGroup(groupIncidentModel model) {

            incidentRepository repository = new incidentRepository();

            return await repository.createGroup(model);
        }

        /// <summary>
        /// Method to get incident data
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>responseModel</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("get/{code}"), Authorize]
        [
            SwaggerResponse(200,"If data has deleted ",typeof(responseModel)),
            SwaggerResponse(400,"If not deleted or connection is lost, or something is wrong"),
            SwaggerResponse(404,"No data found")
        ]
        public async Task<responseModel> get(int? code) {

            incidentRepository repository = new incidentRepository();

            return await repository.get(code);
        }

        /// <summary>
        /// Method to get incident data detail
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>responseModel</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("getDetail/{code}"), Authorize]
        [
            SwaggerResponse(200,"If data has deleted ",typeof(responseModel)),
            SwaggerResponse(400,"If not deleted or connection is lost, or something is wrong"),
            SwaggerResponse(404,"No data found")
        ]
        public async Task<responseModel> getDetail(int? code) {

            incidentRepository repository = new incidentRepository();

            return await repository.getDetail(code);
        }

        /// <summary>
        /// Method to get all incidents
        /// </summary>
        /// <param name="state"></param>
        /// <param name="search"></param>
        /// <returns>responseModel</returns>
        [Route("getAll")]
        [MapToApiVersion("1.0")]
        [HttpGet("getAll/{codusr}/{state}/{search}"), Authorize]
        [
            SwaggerResponse(200,"If data has deleted ",typeof(responseModel)),
            SwaggerResponse(400,"If not deleted or connection is lost, or something is wrong"),
            SwaggerResponse(404,"No data found")
        ]
        public async Task<responseModel> getAll(int codusr = 1, byte state = 1, string search = "") {

            incidentRepository repository = new incidentRepository();

            return await repository.getAll(state, "", codusr);
        }

        /// <summary>
        /// Method to get all types
        /// </summary>
        /// <returns>responseModel</returns>
        [Route("getTypes")]
        [MapToApiVersion("1.0")]
        [HttpGet("getTypes"), Authorize]
        public async Task<responseModel> getType() {

            incidentRepository repository = new incidentRepository();

            return await repository.getTypes();
        }

        /// <summary>
        /// Method to get all types
        /// </summary>
        /// <returns>responseModel</returns>
        [Route("getStates")]
        [MapToApiVersion("1.0")]
        [HttpGet("getStates"), Authorize]
        public async Task<responseModel> getStates() {

            incidentRepository repository = new incidentRepository();

            return await repository.getStates();
        }

    }
}
