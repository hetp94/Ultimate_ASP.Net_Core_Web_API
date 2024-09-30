using Entities.Models;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetEmployees(Guid Id, bool trackChanges);

        EmployeeDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
        EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges);
        void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges);
        void UpdateEmployeeForCompany(Guid companyId, Guid id,
            EmployeeForUpdateDto employeeForUpdateDto,
            bool compTrackChanges, bool empTrackChanges);

        (EmployeeForUpdateDto employeePatch, Employee employeeEntrity) GetEmployeeForPatch
            (Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges);

        void SaveChangedForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);

        Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid Id, bool trackChanges);

        Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
        Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges);
        Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges);
        Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id,
            EmployeeForUpdateDto employeeForUpdateDto,
            bool compTrackChanges, bool empTrackChanges);

        Task<(EmployeeForUpdateDto employeePatch, Employee employeeEntrity)> GetEmployeeForPatchAsync
            (Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges);

        Task SaveChangedForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);
    }
}
