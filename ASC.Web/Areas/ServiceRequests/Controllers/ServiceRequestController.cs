using ASC.Business.Interfaces;
using ASC.Model.Models;
using ASC.Web.Areas.ServiceRequests.Models;
using ASC.Web.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASC.Web.Areas.ServiceRequests.Controllers
{
    [Area("ServiceRequests")]
    [Authorize]
    public class ServiceRequestController : BaseController
    {
        private readonly IServiceRequestOperations _serviceRequestOps;
        private readonly IMasterDataCacheOperations _masterDataCache;
        private readonly IMapper _mapper;

        public ServiceRequestController(
            IServiceRequestOperations serviceRequestOps,
            IMasterDataCacheOperations masterDataCache,
            IMapper mapper)
        {
            _serviceRequestOps = serviceRequestOps;
            _masterDataCache = masterDataCache;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ServiceRequest()
        {
            var model = await BuildViewModelAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ServiceRequest(NewServiceRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var vm = await BuildViewModelAsync();
                vm.VehicleRegNo = model.VehicleRegNo;
                vm.RequestedDate = model.RequestedDate;
                vm.CustomerContact = model.CustomerContact;
                return View(vm);
            }

            var serviceRequest = _mapper.Map<ServiceRequest>(model);
            serviceRequest.RowKey = Guid.NewGuid().ToString();
            serviceRequest.PartitionKey = User.Identity?.Name; // email customer
            serviceRequest.CustomerCode = User.Identity?.Name; // cũng lưu vào CustomerCode (tương thích DB)
            serviceRequest.Status = "New";
            serviceRequest.IsRead = false;
            serviceRequest.IsDeleted = false;
            serviceRequest.CreatedDate = DateTime.UtcNow;
            serviceRequest.UpdatedDate = DateTime.UtcNow;
            serviceRequest.CreatedBy = User.Identity?.Name;
            serviceRequest.UpdatedBy = User.Identity?.Name;

            await _serviceRequestOps.CreateServiceRequestAsync(serviceRequest);

            TempData["Success"] = "Yêu cầu dịch vụ đã được gửi thành công!";
            return RedirectToAction("Dashboard", "Dashboard");
        }

        private async Task<NewServiceRequestViewModel> BuildViewModelAsync()
        {
            try
            {
                var cache = await _masterDataCache.GetMasterDataCacheAsync();
                var serviceItems = cache.MasterDataKeys
                    .Where(k => k.IsActive)
                    .Select(k => new SelectListItem { Text = k.Name, Value = k.PartitionKey })
                    .ToList();

                return new NewServiceRequestViewModel
                {
                    ServicesList = serviceItems,
                    VehicleTypeList = serviceItems // dùng cùng danh mục; có thể tách riêng nếu cần
                };
            }
            catch
            {
                // Nếu cache lỗi, trả về danh sách rỗng
                return new NewServiceRequestViewModel
                {
                    ServicesList = new List<SelectListItem>(),
                    VehicleTypeList = new List<SelectListItem>()
                };
            }
        }
        [HttpGet]
        public async Task<IActionResult> Detail(string rowKey)
        {
            var request = await _serviceRequestOps.GetServiceRequestByRowKeyAsync(rowKey);
            if (request == null) return NotFound();
            return View(request);
        }
    }
}
