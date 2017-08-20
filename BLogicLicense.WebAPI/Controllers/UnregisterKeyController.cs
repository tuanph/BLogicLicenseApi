﻿using AutoMapper;
using BLogicLicense.Model.Models;
using BLogicLicense.Service;
using BLogicLicense.Web.Infrastructure.Core;
using BLogicLicense.Web.Models.UnregisterKey;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BLogicLicense.Web.Controllers
{
    [RoutePrefix("api/unregisterKey")]
    public class UnregisterKeyController : ApiControllerBase
    {
        private IUnregisterKeyService _unregisterKeyService;
        public UnregisterKeyController(IErrorService errorService, IUnregisterKeyService unregisterKeyService) : base(errorService) => _unregisterKeyService = unregisterKeyService;

        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int pageIndex, int pageSize, string filter = "")
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int totalRow = 0;

                var model = _unregisterKeyService.GetListPaging(filter, pageIndex, pageSize, out totalRow);

                List<UnregisterKeyViewModel> modelVm = Mapper.Map<List<UnRegisterKey>, List<UnregisterKeyViewModel>>(model);

                PaginationSet<UnregisterKeyViewModel> pagedSet = new PaginationSet<UnregisterKeyViewModel>()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalRows = totalRow,
                    Items = modelVm,
                };

                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }
    }
}
