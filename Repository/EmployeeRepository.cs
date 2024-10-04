using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            Delete(employee);
        }

        public Employee GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
        {
            return FindByCondition(e => e.CompanyId == companyId && e.Id == employeeId, trackChanges).SingleOrDefault();
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
        {
            return await FindByCondition(e => e.CompanyId == companyId && e.Id == employeeId, trackChanges).SingleOrDefaultAsync();
        }

        public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges)
        {
            return FindByCondition(x => x.CompanyId == companyId, trackChanges).OrderBy(x => x.Name).ToList();
        }

        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId,
            EmployeeParameters employeeParameters, bool trackChanges)
        {
            var employees = await FindByCondition(x=>x.CompanyId == companyId,trackChanges)
                .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
                .Search(employeeParameters.SearchTerm)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return PagedList<Employee>.ToPagedList(employees, employeeParameters.PageNumber, employeeParameters.PageSize);
        }
    }
}
