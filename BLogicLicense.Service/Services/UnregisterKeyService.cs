using BLogicLicense.Data.Infrastructure;
using BLogicLicense.Data.Repositories;
using BLogicLicense.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace BLogicLicense.Service
{
    public interface IUnregisterKeyService
    {
        List<UnRegisterKey> GetListPaging(string filter, int pageIndex, int pageSize, out int totalRow);
        int RegisterKey(ProductKey model);
        void Save();
        List<UnRegisterKey> GetAll();
    }

    public class UnregisterKeyService : IUnregisterKeyService
    {
        private IUnregisterKeyRepository _unregisterKeyRepository;
        private IProductKeyRepository _productKeyRepository;
        private IUnitOfWork _unitOfWork;
        public UnregisterKeyService(IUnitOfWork unitOfWork, IUnregisterKeyRepository unregisterKeyRepository, IProductKeyRepository productKeyRepository)
        {
            this._unregisterKeyRepository = unregisterKeyRepository;
            this._productKeyRepository = productKeyRepository;
            this._unitOfWork = unitOfWork;
        }

        public List<UnRegisterKey> GetAll()
        {
            var unregisterKeys = _unregisterKeyRepository.GetAll().OrderByDescending(uk => uk.DateConnected);
            return unregisterKeys.ToList();
        }

        public List<UnRegisterKey> GetListPaging(string filter, int pageIndex, int pageSize, out int totalRow)
        {
            var unregisterKeys = _unregisterKeyRepository.GetListPaging(filter, pageIndex, pageSize, out totalRow);
            return unregisterKeys.ToList();

        }

        public int RegisterKey(ProductKey viewModel)
        {
            var newKey = this._productKeyRepository.RegisterKey(viewModel);
            var unregisterKey = this._unregisterKeyRepository.GetSingleByCondition(uk => uk.Key == viewModel.Key);
            if (unregisterKey != null)
            {
                //this._unregisterKeyRepository.DeleteMulti(uk => uk.Key == viewModel.Key);
                this._unregisterKeyRepository.Delete(unregisterKey);

            }
            this.Save();
            return newKey.ID;
        }

        public void Save()
        {
            this._unitOfWork.Commit();
        }
    }
}
