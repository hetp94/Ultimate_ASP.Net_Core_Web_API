using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;


namespace Ultimate_ASP.Net_Core_Web_API.Profile
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
            .ForCtorParam("FullAddress", opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<EmployeeForCreationDto, Employee>();
        }
    }
}
