using BLogicLicense.Data.Infrastructure;
using BLogicLicense.Model.Models;
using System.Collections.Generic;
using System.Linq;
namespace BLogicLicense.Data.Repositories
{
    public interface ISoftwareRepository : IRepository<Software>
    {
        IEnumerable<Software> GetAll();
    }

    public class SoftwareRepository : RepositoryBase<Software>, ISoftwareRepository
    {
        public SoftwareRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Software> GetAll()
        {
            IQueryable<Software> query = null;
            query = (from a in DbContext.Softwares
                     select a).OrderBy(a => a.Code);

            return query.ToList();
        }
    }
}
