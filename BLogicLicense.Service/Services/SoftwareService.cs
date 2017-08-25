using BLogicLicense.Data.Infrastructure;
using BLogicLicense.Data.Repositories;
using BLogicLicense.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace BLogicLicense.Service
{
    public interface ISoftwareService
    {
        List<Software> GetAll();
        void Save();
    }

    public class SoftwareService : ISoftwareService
    {
        private ISoftwareRepository _softwareRepository;

        private IUnitOfWork _unitOfWork;

        public SoftwareService(ISoftwareRepository softwareRepository, IUnitOfWork unitOfWork)
        {
            this._softwareRepository = softwareRepository;
            this._unitOfWork = unitOfWork;
        }

        public List<Software> GetAll()
        {
            var softwares = _softwareRepository.GetAll();
            return softwares.ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
