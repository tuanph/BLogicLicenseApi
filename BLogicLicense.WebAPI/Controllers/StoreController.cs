using BLogicLicense.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLogicLicense.Service;
using BLogicLicense.Service.ViewModels.Store;
using AutoMapper;
using BLogicLicense.Model.Models;
using BLogicLicense.Common.Exceptions;
using BLogicLicense.Service.ViewModels.CheckLicense;
using BLogicLicense.Web.SignalR;

namespace BLogicLicense.Web.Controllers
{
    [RoutePrefix("api/store")]
    public class StoreController : ApiControllerBase
    {
        private IStoreService _storeService;
        private IUnregisterKeyService _unregisterKeyService;
        public StoreController(IErrorService errorService, IStoreService storeService, IUnregisterKeyService unregisterKeyService) : base(errorService)
        {
            this._storeService = storeService;
            this._unregisterKeyService = unregisterKeyService;
        }

        [Route("getcurrentdate")]
        [HttpGet]
        public HttpResponseMessage GetCurrentDate(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                response = request.CreateResponse(HttpStatusCode.OK, DateTime.Now.ToString());

                return response;
            });
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

        [Route("gellAllWithOutProductKey")]
        [HttpGet]
        public HttpResponseMessage GellAllWithOutProductKey(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _storeService.GellAllWithOutProductKey();
                response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });

        }
        public void UpdateStoreType(List<Store> stores)
        {

            try
            {
                foreach (var store in stores)
                {
                    store.ExpriedType = CheckStoreExpried(store.ProductKeys.OrderBy(k => k.DateExpire).ToList()); // 0:Normal 1:Warning 2:Exiree 3:Empty
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
                    if (pr.DateExpire.Date.Subtract(DateTime.Today).TotalDays < 0 && !pr.IsNeverExpried)  //Software expried
                    {
                        return 2;//Expired
                    }
                    if (pr.DateExpire.Date.Subtract(DateTime.Today).TotalDays <= 3 && !pr.IsNeverExpried)
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
                    List<string> errorPk = new List<string>();
                    newStore = _storeService.EditProductKeys(newStore, ref errorPk);
                    if (newStore.ID == -1)//Duplicated token
                    {
                        string message = string.Empty;
                        foreach (string key in errorPk)
                        {
                            message += $"'{ key}', ";
                        }
                        message = message.Remove(message.Length - 2, 2);
                        return request.CreateResponse(HttpStatusCode.BadRequest, new ErrorReturn($"Key {message} is duplicated."));
                    }
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


        [Route("checklicense/{key}/{softwareCode}")]
        [HttpGet]
        public HttpResponseMessage CheckLicense(HttpRequestMessage request, string key, string softwareCode)
        {
            if (ModelState.IsValid)
            {
                var newStore = new Store();
                try
                {
                    int temp = this._storeService.CheckLicese(key, softwareCode);
                    CheckLicenseResponseViewModel response = new CheckLicenseResponseViewModel();
                    response.ReturnCode = temp.ToString();
                    response.ActionCode = "-1";
                    response.CountDays = temp;
                    response.Key = key;
                    if (temp == -2)
                    {
                        var total = _unregisterKeyService.GetAll().Count;
                        TeduShopHub.PushTotalUnregisterKeyToAllUsers(total, null);
                        response.Message = "ProductID: " + key + "." + Environment.NewLine + "This is unlicensed product. " + Environment.NewLine + "Please contact us at www.blogicsystems.com.";
                    }
                    if (temp == -3)
                    {
                        response.Message = "ProductID: " + key + "." + Environment.NewLine + "Your productID was locked. " + Environment.NewLine + "Please contact us at www.blogicsystems.com.";
                    }
                    if (temp == -1)
                    {
                        response.Message = "ProductID: " + key + "." + Environment.NewLine + "Your productID was expried. " + Environment.NewLine + "Please contact us at www.blogicsystems.com.";
                    }
                    if (temp == 0)
                    {
                        response.Message = "ProductID: " + key + "." + Environment.NewLine + "Your productID will expried today." + Environment.NewLine + "Please contact us at www.blogicsystems.com.";
                    }
                    if (temp > 0 && temp <= 3)
                    {
                        response.Message = "ProductID: " + key + "." + Environment.NewLine + "Your productID will expried in next " + temp.ToString() + " day(s)." + Environment.NewLine + "Please contact us at www.blogicsystems.com.";
                    }
                    if (temp > 3)
                    {
                        response.Message = "Working";
                    }
                    response.DateExpried = DateTime.Today.AddDays(temp);
                    return request.CreateResponse(HttpStatusCode.OK, response);
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
