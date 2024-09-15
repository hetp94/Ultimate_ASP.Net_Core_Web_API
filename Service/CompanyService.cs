using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTransferObjects;
using AutoMapper.Configuration.Annotations;
using Entities.Exceptions;


namespace Service
{
    internal class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
        {
            try
            {
                var companies = _repository.Company.GetAllCompanies(trackChanges);
                var companiedDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
                return companiedDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllCompanies)} service method {ex}");
                throw;
            }
        }

        public CompanyDto GetCompany(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company == null) 
            {
                throw new CompanyNotFoundException(companyId);
            }
            var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }
    }
}
