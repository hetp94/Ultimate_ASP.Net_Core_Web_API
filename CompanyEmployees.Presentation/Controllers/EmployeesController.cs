using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

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
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            var pagedList = await _serviceManager.EmployeeService.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(pagedList.metaData));

            return Ok(pagedList.employees);
        }

        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid employeeId)
        {
            var employee = await _serviceManager.EmployeeService.GetEmployeeAsync(companyId, employeeId, trackChanges: false);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            if (employee == null)
            {
                return BadRequest("EmployeeForCreationDto object is null");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var employeeReturn = await _serviceManager.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, trackChanges: false);
            return CreatedAtRoute(nameof(GetEmployeeForCompany), new { companyId, id = employeeReturn.Id }, employeeReturn);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            await _serviceManager.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, id, trackChanges: false);

            return NoContent();
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            if (employee == null)
            {
                return BadRequest("EmployeeForUpdateDto object is null");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            await _serviceManager.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, id,
                  employee, compTrackChanges: false, empTrackChanges: true);

            return NoContent();
        }
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
            [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("patchDoc object sent from client is null");
            }

            var result = await _serviceManager.EmployeeService.GetEmployeeForPatchAsync(companyId, id,
                compTrackChanges: false, empTrackChanges: true);

            patchDoc.ApplyTo(result.employeePatch, ModelState);

            TryValidateModel(result.employeePatch);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            await _serviceManager.EmployeeService.SaveChangedForPatchAsync(result.employeePatch, result.employeeEntrity);

            return NoContent();
        }
    }
}
