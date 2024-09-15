using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public EmployeeDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }
            var employeeDb = _repository.Employee.GetEmployee(companyId, employeeId, trackChanges);
            if (employeeDb == null)
            {
                throw new EmployeeNotFoundException(employeeId);
            }
            var employee = _mapper.Map<EmployeeDto>(employeeDb);
            return employee;
        }

        public IEnumerable<EmployeeDto> GetEmployees(Guid Id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(Id, trackChanges);
            if (company == null)
            {
                throw new CompanyNotFoundException(Id);
            }
            var employeeesFromDb = _repository.Employee.GetEmployees(Id, trackChanges);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeeesFromDb);
            return employeesDto;
        }

    }
}
