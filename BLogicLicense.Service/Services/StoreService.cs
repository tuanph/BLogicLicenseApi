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
        int CheckLicese(string key, string softwareCode);
    }

    public class StoreService : IStoreService
    {
        private IStoreRepository _storeRepository;
        private IProductKeyRepository _productKeyRepository;
        private IUnregisterKeyRepository _unregisterKeyRepository;
        private ISoftwareRepository _softwareRepository;
        private IUnitOfWork _unitOfWork;

        public StoreService(IStoreRepository storeRepository, IUnitOfWork unitOfWork, IProductKeyRepository productKeyRepository, ISoftwareRepository softwareRepository, IUnregisterKeyRepository unregisterKeyRepository)
        {
            this._storeRepository = storeRepository;
            this._productKeyRepository = productKeyRepository;
            this._softwareRepository = softwareRepository;
            this._unregisterKeyRepository = unregisterKeyRepository;
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

        public int CheckLicese(string key, string softwareCode)
        {
            var pk = this._productKeyRepository.GetSingleByCondition(p => p.Key == key);
            if (pk == null)
            {
                //Add/Update Unregister key
                var software = this._softwareRepository.GetSingleByCondition(s => s.Code.ToString() == softwareCode);
                var existsUk = this._unregisterKeyRepository.GetSingleByCondition(u => u.Key == key);
                if (existsUk != null)
                {
                    existsUk.DateConnected = DateTime.Now;
                    this._unregisterKeyRepository.Update(existsUk);
                }
                else
                {
                    UnRegisterKey uk = new UnRegisterKey()
                    {
                        DateConnected = DateTime.Now,
                        Key = key,
                        SoftwareID = software.ID
                    };
                    this._unregisterKeyRepository.Add(uk);
                }
                this.Save();
                return -2;
            }
            if (pk.IsNeverExpried)
            {
                return 9999;
            }
            if (pk.IsLock)
            {
                return -3;
            }

            int countDays = (int)pk.DateExpire.Subtract(DateTime.Today).TotalDays;
            if (countDays < 0) //Expried
            {
                return -1;
            }
            if (countDays <= 3)//waring
                return countDays;
            return countDays;

        }
    }
}
