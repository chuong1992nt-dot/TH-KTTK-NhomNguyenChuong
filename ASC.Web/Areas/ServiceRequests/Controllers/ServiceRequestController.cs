using ASC.Business.Interfaces;
using ASC.Model.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceRequestModel = ASC.Model.Models.ServiceRequest;
using ASC.Web.Areas.ServiceRequests.Models;

namespace ASC.Web.Areas.ServiceRequests.Controllers
{
    [Area("ServiceRequests")]
    [Authorize]
    public class ServiceRequestController : Controller
    {
        private readonly IServiceRequestOperations _serviceRequestOperations;
        private readonly IMasterDataCacheOperations _masterDataCache;
        private readonly IMapper _mapper;

        public ServiceRequestController(
            IServiceRequestOperations serviceRequestOperations,
            IMasterDataCacheOperations masterDataCache,
            IMapper mapper)
        {
            _serviceRequestOperations = serviceRequestOperations;
            _masterDataCache = masterDataCache;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ServiceRequest()
        {
            var cache = await _masterDataCache.GetMasterDataCacheAsync();

            var viewModel = new NewServiceRequestViewModel
            {
                ServicesList = cache.MasterDataKeys
                    .Select(k => new SelectListItem
                    {
                        Text = k.Name,
                        Value = k.PartitionKey
                    }).ToList(),

                VehicleTypeList = cache.MasterDataKeys
                    .Select(k => new SelectListItem
                    {
                        Text = k.Name,
                        Value = k.PartitionKey
                    }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ServiceRequest(NewServiceRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var cache = await _masterDataCache.GetMasterDataCacheAsync();
                model.ServicesList = cache.MasterDataKeys
                    .Select(k => new SelectListItem
                    {
                        Text = k.Name,
                        Value = k.PartitionKey
                    }).ToList();
                model.VehicleTypeList = model.ServicesList;
                return View(model);
            }

            var serviceRequest = _mapper.Map<ServiceRequest>(model);
            serviceRequest.PartitionKey = User.Identity?.Name ?? "customer";
            serviceRequest.RowKey = Guid.NewGuid().ToString();
            serviceRequest.Status = "New";
            serviceRequest.CreatedDate = DateTime.UtcNow;
            serviceRequest.UpdatedDate = DateTime.UtcNow;
            serviceRequest.CreatedBy = User.Identity?.Name ?? "customer";
            serviceRequest.UpdatedBy = User.Identity?.Name ?? "customer";
            serviceRequest.IsDeleted = false;
            serviceRequest.IsRead = false;

            await _serviceRequestOperations.CreateServiceRequestAsync(serviceRequest);

            return RedirectToAction("ServiceRequest");
        }
    }
}