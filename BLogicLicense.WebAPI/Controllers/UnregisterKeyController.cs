using AutoMapper;
using BLogicLicense.Common.Exceptions;
using BLogicLicense.Model.Models;
using BLogicLicense.Service;
using BLogicLicense.Web.Infrastructure.Core;
using BLogicLicense.Service.ViewModels.UnregisterKey;
using BLogicLicense.Web.SignalR;
using Microsoft.AspNet.Identity;
using System;
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
        public UnregisterKeyController(IErrorService errorService, IUnregisterKeyService unregisterKeyService) : base(errorService)
        {
            _unregisterKeyService = unregisterKeyService;
        }


        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            try
            {
                return CreateHttpResponse(request, () =>
                {
                    HttpResponseMessage response = null;
                    var model = _unregisterKeyService.GetAll();
                    response = request.CreateResponse(HttpStatusCode.OK, model);
                    return response;
                });
            }
            catch (Exception ex)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, new ErrorReturn(ex.Message));
            }
        }

        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int pageIndex, int pageSize, string filter = "")
        {
            try
            {
                return CreateHttpResponse(request, () =>
                {
                    HttpResponseMessage response = null;
                    int totalRow = 0;

                    var model = _unregisterKeyService.GetListPaging(filter, pageIndex, pageSize, out totalRow);

                    List<UnregisterKeyViewModel> modelVm = Mapper.Map<List<UnRegisterKey>, List<UnregisterKeyViewModel>>(model);
                    modelVm.ForEach(k =>
                    {
                        k.DateExpried = DateTime.Today.ToString("MM/dd/yyyy");
                        k.StoreID = 1;
                    });

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
            catch (Exception ex)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, new ErrorReturn(ex.Message));
            }
        }
        [Route("registerkey")]
        [HttpPost]
        public HttpResponseMessage RegisterKey(HttpRequestMessage request, RegisterKeyViewModel viewModel)
        {
            try
            {
                return CreateHttpResponse(request, () =>
                {
                    HttpResponseMessage response = null;
                    ProductKey model = new ProductKey()
                    {
                        DateExpire = viewModel.DateExpried,
                        Key = viewModel.Key,
                        SoftwareID = viewModel.SoftwareID,
                        StoreID = viewModel.StoreID,
                        LastRenewal = DateTime.Now,
                        IsLock = false
                    };

                    int id = _unregisterKeyService.RegisterKey(model);
                    if (id < 0)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, new ErrorReturn($"Key '{viewModel.Key}' is duplicated."));
                    }
                    response = request.CreateResponse(HttpStatusCode.OK, id);
                    //push notification
                    var total = _unregisterKeyService.GetAll().Count;
                    TeduShopHub.PushTotalUnregisterKeyToAllUsers(total, null);
                    return response;
                });
            }
            catch (Exception ex)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, new ErrorReturn(ex.Message));
            }

        }
    }
}
