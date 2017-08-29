using BLogicLicense.Web.Infrastructure.Core;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLogicLicense.Service;
using BLogicLicense.Service.ViewModels.Software;
using AutoMapper;
using BLogicLicense.Model.Models;

namespace BLogicLicense.Web.Controllers
{
    [RoutePrefix("api/software")]
    [Authorize]
    public class SoftwareController : ApiControllerBase
    {
        private ISoftwareService _softwareService;
        public SoftwareController(IErrorService errorService, ISoftwareService softwareService) : base(errorService)
        {
            this._softwareService = softwareService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _softwareService.GetAll();
                List<SoftwareViewModel> modelVm = Mapper.Map<List<Software>, List<SoftwareViewModel>>(model);
                response = request.CreateResponse(HttpStatusCode.OK, modelVm);
                return response;
            });
        }
    }
}
