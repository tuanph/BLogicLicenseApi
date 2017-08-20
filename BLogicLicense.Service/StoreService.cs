using BLogicLicense.Data.Infrastructure;
using BLogicLicense.Data.Repositories;
using BLogicLicense.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLogicLicense.Service
{
    public interface IStoreService
    {
        List<Store> GetListPaging(string filter, int pageIndex, int pageSize, out int totalRow);
        void Save();
        Store Create(Store newStore);
        Store EditProductKeys(Store newStore, ref List<string> arrErrorPk);
        Store EditStore(Store newStore);
        string Delete(string storeId);
        List<Store> GellAllWithOutProductKey();
    }

    public class StoreService : IStoreService
    {
        private IStoreRepository _storeRepository;
        private IProductKeyRepository _productKeyRepository;
        private IUnitOfWork _unitOfWork;

        public StoreService(IStoreRepository storeRepository, IUnitOfWork unitOfWork, IProductKeyRepository productKeyRepository)
        {
            this._storeRepository = storeRepository;
            this._productKeyRepository = productKeyRepository;
            this._unitOfWork = unitOfWork;
        }

        public Store Create(Store newStore)
        {
            //Check duplicate Customer token
            var existsStore = this._storeRepository.GetSingleByCondition(s => s.Token == newStore.Token && !s.IsDeleted);
            if (existsStore != null)
            {
                newStore.ID = -1;
                return newStore;
            }
            newStore.CreatedDate = DateTime.Now;
            _storeRepository.Add(newStore);
            _unitOfWork.Commit();
            return newStore;
        }

        public string Delete(string storeId)
        {
            //Check store has product keys
            var productKeys = this._productKeyRepository.GetMulti(pk => pk.StoreID.ToString() == storeId).ToList();
            if (productKeys.Count == 0)//empty store. Remove from database
            {
                this._storeRepository.DeleteMulti(s => s.ID.ToString() == storeId);
            }
            else
            {
                foreach (var pk in productKeys)
                {
                    pk.IsLock = true;
                    pk.LastRenewal = DateTime.Now;
                }
                var existsedStore = this._storeRepository.GetSingleById(Convert.ToInt32(storeId));
                existsedStore.IsDeleted = true;
            }
            this.Save();
            return storeId;
        }

        public Store EditProductKeys(Store newStore, ref List<string> arrErrorPk)
        {
            //Check productkey exists
            foreach (var pk in newStore.ProductKeys)
            {
                var existsKey = this._productKeyRepository.GetSingleByCondition(p => p.Key == pk.Key && p.ID != pk.ID);
                if (existsKey != null)
                {
                    arrErrorPk.Add(pk.Key);
                    newStore.ID = -1;
                }

            }
            if (newStore.ID == -1)
                return newStore;
            return _storeRepository.UpdateProductKeyForStore(newStore);
        }

        public Store EditStore(Store newStore)
        {
            return _storeRepository.EditStore(newStore);
        }

        public List<Store> GetListPaging(string filter, int pageIndex, int pageSize, out int totalRow)
        {
            var stores = _storeRepository.GetListPaging(filter, pageIndex, pageSize, out totalRow);
            return stores.ToList();
        }

        public List<Store> GellAllWithOutProductKey()
        {
            var stores = _storeRepository.GellAllWithOutProductKey();
            return stores.ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
