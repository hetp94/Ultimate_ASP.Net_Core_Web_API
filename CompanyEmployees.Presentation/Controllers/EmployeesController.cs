using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public EmployeesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var employees = _serviceManager.EmployeeService.GetEmployees(companyId, trackChanges: false);
            return Ok(employees);
        }

        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid employeeId)
        {
            var employee = _serviceManager.EmployeeService.GetEmployee(companyId, employeeId, trackChanges: false);
            return Ok(employee);
        }

        [HttpPost]
        public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            if (employee == null)
            {
                return BadRequest("EmployeeForCreationDto object is null");
            }

            var employeeReturn = _serviceManager.EmployeeService.CreateEmployeeForCompany(companyId, employee, trackChanges: false);
            return CreatedAtRoute(nameof(GetEmployeeForCompany), new { companyId, id = employeeReturn.Id }, employeeReturn);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            _serviceManager.EmployeeService.DeleteEmployeeForCompany(companyId, id, trackChanges: false);

            return NoContent();
        }
    }
}
