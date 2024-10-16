using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICompanyService> _companyService;
        private readonly Lazy<IEmployeeService> _employeeService;
        private readonly Lazy<IAuthenticationService> _authenticationService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper, 
            IDataShaper<EmployeeDto> dataShaper, UserManager<User> userManager, IConfiguration configuration)
        {
            _companyService = new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, loggerManager, mapper));
            _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager, loggerManager, mapper, dataShaper));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, loggerManager, mapper, configuration));
        }

        public ICompanyService CompanyService
        {
            get
            {
                return _companyService.Value;
            }
        }

        public IEmployeeService EmployeeService
        {
            get
            {
                return _employeeService.Value;
            }
        }

        public IAuthenticationService AuthenticationService
        {
            get
            {
                return _authenticationService.Value;
            }
        }
    }
}
