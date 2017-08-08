using BLogicLicense.Data.Infrastructure;
using BLogicLicense.Model.Models;

namespace BLogicLicense.Data.Repositories
{
    public interface IColorRepository : IRepository<Color>
    {
    }

    public class ColorRepository : RepositoryBase<Color>, IColorRepository
    {
        public ColorRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}