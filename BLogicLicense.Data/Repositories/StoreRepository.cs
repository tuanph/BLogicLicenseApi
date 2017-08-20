using BLogicLicense.Data.Infrastructure;
using BLogicLicense.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace BLogicLicense.Data.Repositories
{
    public interface IStoreRepository : IRepository<Store>
    {
        IEnumerable<Store> GetListPaging(string filter, int pageIndex, int pageSize, out int totalRow);
        Store UpdateProductKeyForStore(Store newStore);
        Store EditStore(Store newStore);
        IEnumerable<Store> GellAllWithOutProductKey();
    }

    public class StoreRepository : RepositoryBase<Store>, IStoreRepository
    {
        public StoreRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Store> GetListPaging(string filter, int pageIndex, int pageSize, out int totalRow)
        {
            IQueryable<Store> query = null;
            if (string.IsNullOrWhiteSpace(filter))
            {
                query = (from a in DbContext.Stores.Include("ProductKeys")
                         where a.IsDeleted == false
                         select a).OrderBy(a => a.Token).ThenBy(a => a.Name);
            }
            else
            {

                query = (from a in DbContext.Stores.Include("ProductKeys")
                         where a.IsDeleted == false && (a.Name.Contains(filter) || a.Token.Contains(filter) || a.Phone.Contains(filter)
                         || a.Email.Contains(filter) || a.Agent.Contains(filter)) || a.ProductKeys.Any(pk => pk.Key.Contains(filter))
                         select a).OrderBy(a => a.Token).ThenBy(a => a.Name);
            }
            totalRow = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public IEnumerable<Store> GellAllWithOutProductKey()
        {
            IQueryable<Store> query = null;

            query = (from a in DbContext.Stores
                     where a.IsDeleted == false
                     select a).OrderBy(a => a.Token).ThenBy(a => a.Name);
            return query.ToList();

        }

        public Store UpdateProductKeyForStore(Store newStore)
        {
            var existsStore = this.GetSingleByCondition(p => p.ID == newStore.ID, new string[] { "ProductKeys" });
            if (existsStore != null)
            {
                foreach (var existingChild in existsStore.ProductKeys.ToList())
                {
                    DbContext.Entry(existingChild).State = System.Data.Entity.EntityState.Deleted;
                }
                foreach (ProductKey newChild in newStore.ProductKeys.ToList())
                {
                    newChild.LastRenewal = DateTime.Now;
                    newChild.Store = null;
                    existsStore.ProductKeys.Add(newChild);
                }

            }
            DbContext.SaveChanges();
            return newStore;
        }
        public Store EditStore(Store newStore)
        {
            var existsStore = this.GetSingleByCondition(p => p.ID == newStore.ID, new string[] { "ProductKeys" });
            if (existsStore != null)
            {
                existsStore.Address = newStore.Address;
                existsStore.Agent = newStore.Agent;
                existsStore.Email = newStore.Email;
                existsStore.Name = newStore.Name;
                existsStore.Phone = newStore.Phone;
                existsStore.Token = newStore.Token;
            }
            DbContext.Entry(existsStore).State = System.Data.Entity.EntityState.Modified;
            DbContext.SaveChanges();
            return newStore;
        }


    }
}
