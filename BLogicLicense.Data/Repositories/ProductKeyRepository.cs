using BLogicLicense.Data.Infrastructure;
using BLogicLicense.Model.Models;
using System.Collections.Generic;
using System.Linq;
namespace BLogicLicense.Data.Repositories
{
    public interface IProductKeyRepository : IRepository<ProductKey>
    {

    }

    public class ProductKeyRepository : RepositoryBase<ProductKey>, IProductKeyRepository
    {
        public ProductKeyRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
