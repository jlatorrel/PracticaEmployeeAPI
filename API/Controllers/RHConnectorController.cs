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

        [HttpGet("GetEmployeeManager/{id}")]
        public async Task<IActionResult> GetEmployeeManager(int id)
        {
            var result = await _rhConnector.GetEmployeeManager(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("GetOldestEmployee")]
        public async Task<IActionResult> GetOldestEmployee()
        {
            var result = await _rhConnector.GetOldestEmployee();
            return Ok(result);
        }

        [HttpGet("GetNewestEmployee")]
        public async Task<IActionResult> GetNewestEmployee()
        {
            var result = await _rhConnector.GetNewestEmployee();
            return Ok(result);
        }

        [HttpGet("GetEmployeeBy{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var result = await _rhConnector.GetEmployeeById(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("GetEmployeesWithMoreThan/{years}")]
        public async Task<IActionResult> GetEmployeesWithMoreThan(int years)
        {
            var result = await _rhConnector.GetEmployeesWithMoreThan(years);
            return Ok(result);
        }

        [HttpGet("GetEmployeesWithLessThan/{years}")]
        public async Task<IActionResult> GetEmployeesWithLessThan(int years)
        {
            var result = await _rhConnector.GetEmployeesWithLessThan(years);
            return Ok(result);
        }
    }
}
