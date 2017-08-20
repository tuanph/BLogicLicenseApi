using BLogicLicense.Data.Infrastructure;
using BLogicLicense.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace BLogicLicense.Data.Repositories
{
    public interface IUnregisterKeyRepository : IRepository<UnRegisterKey>
    {
        IEnumerable<UnRegisterKey> GetListPaging(string filter, int pageIndex, int pageSize, out int totalRow);
    }
    public class UnregisterKeyRepository : RepositoryBase<UnRegisterKey>, IUnregisterKeyRepository
    {
        public UnregisterKeyRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<UnRegisterKey> GetListPaging(string filter, int pageIndex, int pageSize, out int totalRow)
        {
            IQueryable<UnRegisterKey> query = null;
            if (string.IsNullOrWhiteSpace(filter))
            {
                query = (from a in DbContext.UnRegisterKeys
                         select a).OrderBy(a => a.DateConnected);
            }
            else
            {
                query = (from a in DbContext.UnRegisterKeys
                         where a.Key.Contains(filter) || a.DeviceID.Contains(filter) || a.DeviceName.Contains(filter)
                         select a).OrderBy(a => a.DateConnected);
            }
            totalRow = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
