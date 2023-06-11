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
    public class employeeController : Controller {

        /// <summary>
        /// Method to create a new employee
        /// </summary>
        /// <param name="model">Json structure</param>
        /// <returns>responseModel</returns>
        [MapToApiVersion("1.0")]
        [HttpPost("create"), Authorize]
        [
            SwaggerResponse(200,"If data has saved ",typeof(responseModel)),
            SwaggerResponse(400,"If not saved or connection is lost, or something is wrong")
        ]
        public async Task<responseModel> create(employeeModel model) {

            employeeRepository repository = new employeeRepository();

            return await repository.create(model);
        }

        /// <summary>
        /// Method to update an employee
        /// </summary>
        /// <param name="model">Json structure</param>
        /// <returns>responseModel</returns>
        [MapToApiVersion("1.0")]
        [HttpPost("update"), Authorize]
        [
            SwaggerResponse(200,"If data has updated ",typeof(responseModel)),
            SwaggerResponse(400,"If not updated or connection is lost, or something is wrong")
        ]
        public async Task<responseModel> update(employeeModel model) {

            employeeRepository repository = new employeeRepository();

            return await repository.update(model);
        }

        /// <summary>
        /// Method to delete an employee
        /// </summary>
        /// <param name="model">Json structure</param>
        /// <returns>responseModel</returns>
        [MapToApiVersion("1.0")]
        [HttpPost("delete"), Authorize]
        [
            SwaggerResponse(200,"If data has deleted ",typeof(responseModel)),
            SwaggerResponse(400,"If not deleted or connection is lost, or something is wrong")
        ]
        public async Task<responseModel> delete(employeeModel model) {

            employeeRepository repository = new employeeRepository();

            return await repository.delete(model);
        }

        /// <summary>
        /// Method to get employee data
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

            employeeRepository repository = new employeeRepository();

            return await repository.get(code);
        }

        /// <summary>
        /// Method to get all employees
        /// </summary>
        /// <param name="search">String</param>
        /// <returns>responseModel</returns>
        [Route("getAll")]
        [MapToApiVersion("1.0")]
        [HttpGet("getAll/{search}"), Authorize]
        [
            SwaggerResponse(200,"If data has deleted ",typeof(responseModel)),
            SwaggerResponse(400,"If not deleted or connection is lost, or something is wrong"),
            SwaggerResponse(404,"No data found")
        ]
        public async Task<responseModel> getAll(string search) {

            employeeRepository repository = new employeeRepository();

            return await repository.getAll(search);
        }
    }
}
