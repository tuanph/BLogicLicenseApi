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

    }

    public class UnregisterKeyService : IUnregisterKeyService
    {
        private IUnregisterKeyRepository _unregisterKeyRepository;
        private IUnitOfWork _unitOfWork;
        public UnregisterKeyService(IUnitOfWork unitOfWork, IUnregisterKeyRepository unregisterKeyRepository)
        {
            this._unregisterKeyRepository = unregisterKeyRepository;
            this._unitOfWork = unitOfWork;
        }
        public List<UnRegisterKey> GetListPaging(string filter, int pageIndex, int pageSize, out int totalRow)
        {
            var unregisterKeys = _unregisterKeyRepository.GetListPaging(filter, pageIndex, pageSize, out totalRow);
            return unregisterKeys.ToList();

        }
    }
}
