using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

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

        public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            var employeeEntity = _mapper.Map<Employee>(employeeForCreationDto);

            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            _repository.Save();

            return _mapper.Map<EmployeeDto>(employeeEntity);
        }

        public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            var employeeEntity = _mapper.Map<Employee>(employeeForCreationDto);

            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repository.SaveAsync();

            return _mapper.Map<EmployeeDto>(employeeEntity);
        }

        public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            var employeeForCompany = _repository.Employee.GetEmployee(companyId, id, trackChanges);

            if (employeeForCompany == null)
            {
                throw new EmployeeNotFoundException(id);
            }
            _repository.Employee.DeleteEmployee(employeeForCompany);
            _repository.Save();
        }

        public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            var employeeForCompany = _repository.Employee.GetEmployee(companyId, id, trackChanges);

            if (employeeForCompany == null)
            {
                throw new EmployeeNotFoundException(id);
            }
            _repository.Employee.DeleteEmployee(employeeForCompany);
            await _repository.SaveAsync();
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

        public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }
            var employeeDb = await _repository.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges);
            if (employeeDb == null)
            {
                throw new EmployeeNotFoundException(employeeId);
            }
            var employee = _mapper.Map<EmployeeDto>(employeeDb);
            return employee;
        }

        public (EmployeeForUpdateDto employeePatch, Employee employeeEntrity) GetEmployeeForPatch
            (Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            Company company = _repository.Company.GetCompany(companyId, compTrackChanges);
            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            Employee employeeEntity = _repository.Employee.GetEmployee(companyId, id, empTrackChanges);
            if (employeeEntity == null)
            {
                throw new EmployeeNotFoundException(id);
            }

            EmployeeForUpdateDto employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
            return (employeeToPatch, employeeEntity);
        }

        public async Task<(EmployeeForUpdateDto employeePatch, Employee employeeEntrity)> GetEmployeeForPatchAsync
            (Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            Company company = await _repository.Company.GetCompanyAsync(companyId, compTrackChanges);
            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            Employee employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges);
            if (employeeEntity == null)
            {
                throw new EmployeeNotFoundException(id);
            }

            EmployeeForUpdateDto employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
            return (employeeToPatch, employeeEntity);
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

        public async Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync
            (Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);
            var employeesWithMetaData = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
            return (employees: employeesDto, metaData: employeesWithMetaData.MetaData);
        }

        public void SaveChangedForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            _repository.Save();
        }

        public async Task SaveChangedForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            await _repository.SaveAsync();
        }

        public void UpdateEmployeeForCompany
            (Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdateDto, bool compTrackChanges, bool empTrackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, compTrackChanges);
            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            var employeeEntity = _repository.Employee.GetEmployee(companyId, id, empTrackChanges);

            if (employeeEntity == null)
            {
                throw new EmployeeNotFoundException(id);
            }

            _mapper.Map(employeeForUpdateDto, employeeEntity);

            _repository.Save();
        }

        public async Task UpdateEmployeeForCompanyAsync
            (Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdateDto, bool compTrackChanges, bool empTrackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, compTrackChanges);
            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges);

            if (employeeEntity == null)
            {
                throw new EmployeeNotFoundException(id);
            }

            _mapper.Map(employeeForUpdateDto, employeeEntity);

            await _repository.SaveAsync();
        }

        private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
        }
    }
}
