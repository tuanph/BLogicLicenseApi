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
        Store Edit(Store newStore);
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
            newStore.CreatedDate = DateTime.Now;
            _storeRepository.Add(newStore);
            _unitOfWork.Commit();
            return newStore;
        }

        public Store Edit(Store newStore)
        {
            //delete or update productkey first
            //var oldProductKeys = _productKeyRepository.GetAll().Where(p => p.StoreID == newStore.ID);
            //var existsStore = _storeRepository.GetSingleByCondition(p => p.ID == newStore.ID, new string[] { "ProductKeys" });

            //if (existsStore != null)
            //{
            //    //existsStore.ProductKeys = newStore.ProductKeys;
            //    foreach (var oldPk in existsStore.ProductKeys)
            //    {
            //        existsStore.ProductKeys.Remove(oldPk);
            //    }
            //    for (int i = existsStore.ProductKeys.Count - 1; i >= 0; i--)
            //    {
            //        //existsStore.ProductKeys.Remove(existsStore.ProductKeys.find);
            //    }

            //    foreach (var newPk in newStore.ProductKeys)
            //    {
            //        existsStore.ProductKeys.Add(newPk);
            //    }
            //}
            //_productKeyRepository.DeleteMulti(p=>p.StoreID==newStore.ID);

            //foreach (var pk in newStore.ProductKeys)
            //{
            //    pk.LastRenewal = DateTime.Now;
            //    var productKey = _productKeyRepository.Add(pk);
            //}
            //_unitOfWork.Commit();
            //_storeRepository.Update(newStore);
            //_unitOfWork.Commit();
            return _storeRepository.UpdateProductKeyForStore(newStore);
        }

        public List<Store> GetListPaging(string filter, int pageIndex, int pageSize, out int totalRow)
        {
            var stores = _storeRepository.GetAll(filter, pageIndex, pageSize, out totalRow);
            return stores.ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
