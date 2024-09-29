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

        public CompanyDto CreateCompany(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);
            _repository.Company.CreateCompany(companyEntity);
            _repository.Save();

            var companyReturn = _mapper.Map<CompanyDto>(companyEntity);
            return companyReturn;
        }

        public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollaction)
        {
            if (companyCollaction == null)
            {
                throw new CompanyCollectionBadRequest();
            }

            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollaction);

            foreach (var companyEntity in companyEntities)
            {
                _repository.Company.CreateCompany(companyEntity);
            }
            _repository.Save();

            var companyCollectionReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            var ids = string.Join(",", companyCollectionReturn.Select(x => x.Id));

            return (companies: companyCollectionReturn, ids);
        }

        public void DeleteCompany(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            _repository.Company.DeleteCompany(company);
            _repository.Save();
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

        public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> companyIds, bool trackChanges)
        {
            if (companyIds is null)
            {
                throw new IdParametersBadRequestException();
            }

            var companyEntities = _repository.Company.GetByIds(companyIds, trackChanges);
            if (companyIds.Count() != companyEntities.Count())
            {
                throw new CollectionByIdsBadRequestException();
            }

            return _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
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

        public void UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdateDto, bool trackChanges)
        {
            var companyEntity = _repository.Company.GetCompany(companyId, trackChanges);
            if (companyEntity == null)
            {
                throw new CompanyNotFoundException(companyId);
            }
            _mapper.Map(companyForUpdateDto, companyEntity);
            _repository.Save();
        }
    }
}
