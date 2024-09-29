using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public abstract record CompanyForManipulationDto
    {
        [Required(ErrorMessage = "Company name is a required field")]
        public string? Name { get; init; }
        [Required(ErrorMessage = "Address name is a required field")]
        public string? Address { get; init; }
        [Required(ErrorMessage = "Country name is a required field")]
        public string? Country { get; init; }
        public IEnumerable<EmployeeForCreationDto>? Employees { get; init; }
    }
}
