using AppLogic;
using DTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [EnableCors("DemoPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class RHConnectorController : ControllerBase
    {
        private readonly IRHConnector _rhConnector;

        public RHConnectorController(IRHConnector rhConnector)
        {
            _rhConnector = rhConnector;
        }

        [HttpGet("GetAllEmployees")]
        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _rhConnector.RetrieveAllEmployees();
        }

        [HttpGet("GetAllSpecialties")]
        public async Task<List<string>> GetAllSpecialties()
        {
            return await _rhConnector.RetrieveAllSpecialties();
        }
    }
}
