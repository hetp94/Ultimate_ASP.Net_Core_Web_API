using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public IEnumerable<Company> GetAllCompanies(bool trackChanges)
        {
            return  FindAll(trackChanges).OrderBy(x => x.Name).ToList();
        }

        public Company GetCompany(Guid companyId, bool trackChanges)
        {
            return FindByCondition(x => x.Id == companyId, trackChanges).SingleOrDefault();
        }
    }
}
