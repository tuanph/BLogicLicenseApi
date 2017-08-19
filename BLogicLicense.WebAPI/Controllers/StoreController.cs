using BLogicLicense.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLogicLicense.Service;
using BLogicLicense.Web.Models.Store;
using AutoMapper;
using BLogicLicense.Model.Models;
using BLogicLicense.Common.Exceptions;

namespace BLogicLicense.Web.Controllers
{
    [RoutePrefix("api/store")]
    //[Authorize]
    public class StoreController : ApiControllerBase
    {
        private IStoreService _storeService;
        public StoreController(IErrorService errorService, IStoreService storeService) : base(errorService)
        {
            this._storeService = storeService;
        }

        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int pageIndex, int pageSize, string filter = "")
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int totalRow = 0;

                var model = _storeService.GetListPaging(filter, pageIndex, pageSize, out totalRow);
                this.UpdateStoreType(model);
                List<StoreViewModel> modelVm = Mapper.Map<List<Store>, List<StoreViewModel>>(model);

                PaginationSet<StoreViewModel> pagedSet = new PaginationSet<StoreViewModel>()
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

        public void UpdateStoreType(List<Store> stores)
        {

            try
            {
                foreach (var store in stores)
                {
                    store.ExpriedType = CheckStoreExpried(store.ProductKeys); // 0:Normal 1:Warning 2:Exiree 3:Empty
                }
            }
            catch (Exception ex)
            {
                //logger.Error("UpdateExpriedCustomer: " + ex.ToString() + "----" + ex.InnerException);
            }
        }

        private int CheckStoreExpried(ICollection<ProductKey> productKeys)
        {
            try
            {
                if (productKeys == null || productKeys.Count == 0) //Epmty Customer have not any Software
                    return 3;
                foreach (var pr in productKeys)
                {
                    if (pr.DateExpire.Date.Subtract(DateTime.Today).TotalDays < 0 || pr.IsLock) //Software expried
                    {
                        return 2;//Expired
                    }
                    if (pr.DateExpire.Date.Subtract(DateTime.Today).TotalDays <= 3)
                    {
                        return 1; //Warning
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return 0;//Normal
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Create(HttpRequestMessage request, StoreViewModel storeVm)
        {
            if (ModelState.IsValid)
            {
                var newStore = new Store();
                try
                {
                    //TDestination Map<TSource, TDestination>(TSource source);
                    newStore = new Store()
                    {
                        Name = storeVm.Name,
                        Token = storeVm.Token,
                        Address = storeVm.Address,
                        CreatedDate = DateTime.Now,
                        ExpriedType = 3,
                        Phone = storeVm.Phone,
                        Agent = storeVm.Agent,
                        Email = storeVm.Email,
                        IsDeleted = false,
                        ProductKeys = storeVm.ProductKeys
                    };
                    newStore = _storeService.Create(newStore);
                    if (newStore.ID == -1)//Duplicated token
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, new ErrorReturn($"Token '{newStore.Token}' is duplicated."));
                    }
                    _storeService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, newStore);

                }
                catch (Exception dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpPut]
        [Route("editProductKey")]
        public HttpResponseMessage EditProductKey(HttpRequestMessage request, StoreViewModel storeVm)
        {
            if (ModelState.IsValid)
            {
                var newStore = new Store();
                try
                {
                    //TDestination Map<TSource, TDestination>(TSource source);
                    newStore = new Store()
                    {
                        ProductKeys = storeVm.ProductKeys,
                        ID = storeVm.ID
                    };
                    newStore = _storeService.EditProductKeys(newStore);
                    //_storeService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, newStore);
                }
                catch (Exception dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpPut]
        [Route("editBasic")]
        public HttpResponseMessage EditBasic(HttpRequestMessage request, StoreViewModel storeVm)
        {
            if (ModelState.IsValid)
            {
                var newStore = new Store();
                try
                {
                    //TDestination Map<TSource, TDestination>(TSource source);
                    newStore = new Store()
                    {
                        Name = storeVm.Name,
                        Token = storeVm.Token,
                        Address = storeVm.Address,
                        CreatedDate = DateTime.Now,
                        ExpriedType = 3,
                        Phone = storeVm.Phone,
                        Agent = storeVm.Agent,
                        Email = storeVm.Email,
                        IsDeleted = false,
                        ID = storeVm.ID
                    };
                    newStore = _storeService.EditStore(newStore);
                    return request.CreateResponse(HttpStatusCode.OK, newStore);
                }
                catch (Exception dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }


        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this._storeService.Delete(id);
                    return request.CreateResponse(HttpStatusCode.OK, id);
                }
                catch (Exception dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
    }
}
