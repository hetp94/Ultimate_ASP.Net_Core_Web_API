using Contracts;
using Entities.Models;
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

        public Employee GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
        {
            return FindByCondition(e => e.CompanyId == companyId && e.Id == employeeId, trackChanges).SingleOrDefault();
        }

        public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges)
        {
            return FindByCondition(x => x.CompanyId == companyId, trackChanges).OrderBy(x => x.Name).ToList();
        }
    }
}
