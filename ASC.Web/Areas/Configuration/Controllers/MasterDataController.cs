using ASC.Business.Interfaces;
using ASC.Model.Models;
using ASC.Web.Areas.Configuration.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASC.Web.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [Authorize(Roles = "Admin")] // Chỉ Admin mới được vào
    public class MasterDataController : Controller
    {
        private readonly IMasterDataOperations _masterData;
        private readonly IMapper _mapper;

        public MasterDataController(IMasterDataOperations masterData, IMapper mapper)
        {
            _masterData = masterData;
            _mapper = mapper;
        }

        // ==========================================
        // 1. QUẢN LÝ NHÓM DANH MỤC (MASTER KEYS)
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> MasterKeys()
        {
            var keyList = await _masterData.GetMasterDataKeysAsync();
            var keyViewModelList = _mapper.Map<List<MasterDataKeyViewModel>>(keyList);

            var viewModel = new MasterKeysViewModel
            {
                MasterDataKeys = keyViewModelList,
                MasterDataKeyInContext = new MasterDataKeyViewModel { IsActive = true }
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MasterKeys(MasterKeysViewModel model)
        {
            // Loại bỏ kiểm tra để tránh lỗi 400
            ModelState.Remove("MasterDataKeyInContext.PartitionKey");
            ModelState.Remove("MasterDataKeyInContext.RowKey");
            ModelState.Remove("MasterDataKeys");

            if (!ModelState.IsValid)
            {
                var keys = await _masterData.GetMasterDataKeysAsync();
                model.MasterDataKeys = _mapper.Map<List<MasterDataKeyViewModel>>(keys);
                return View(model);
            }

            // KHỞI TẠO MỚI HOÀN TOÀN ĐỂ TRÁNH LỖI NULL MAPPPIING
            var masterDataKey = new MasterDataKey();

            // Gán trực tiếp từ model.MasterDataKeyInContext (Đây là nơi chứa dữ liệu bạn nhập từ web)
            masterDataKey.Name = model.MasterDataKeyInContext.Name;
            masterDataKey.IsActive = model.MasterDataKeyInContext.IsActive;

            // Gán 2 khóa quan trọng
            masterDataKey.PartitionKey = model.MasterDataKeyInContext.Name;
            masterDataKey.RowKey = Guid.NewGuid().ToString();

            // Lưu vào Database
            await _masterData.InsertMasterDataKeyAsync(masterDataKey);

            return RedirectToAction("MasterKeys");
        }

        // ==========================================
        // 2. QUẢN LÝ GIÁ TRỊ DANH MỤC (MASTER VALUES)
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> MasterValues(string partitionKey)
        {
            var valueList = await _masterData.GetMasterDataValuesAsync(partitionKey);
            var valueViewModelList = _mapper.Map<List<MasterDataValueViewModel>>(valueList);

            var viewModel = new MasterValuesViewModel
            {
                MasterDataValues = valueViewModelList,
                MasterDataValueInContext = new MasterDataValueViewModel
                {
                    PartitionKey = partitionKey, // Lưu lại mã nhóm để thêm giá trị mới vào đúng nhóm này
                    IsActive = true
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MasterValues(MasterValuesViewModel model)
        {
            ModelState.Remove("MasterDataValues");
            if (!ModelState.IsValid)
            {
                var values = await _masterData.GetMasterDataValuesAsync(model.MasterDataValueInContext.PartitionKey);
                model.MasterDataValues = _mapper.Map<List<MasterDataValueViewModel>>(values);
                return View(model);
            }

            var masterDataValue = _mapper.Map<MasterDataValue>(model.MasterDataValueInContext);
            masterDataValue.RowKey = Guid.NewGuid().ToString();

            await _masterData.InsertMasterDataValueAsync(masterDataValue);

            // Sau khi thêm, quay lại trang danh sách giá trị của đúng Nhóm đó
            return RedirectToAction("MasterValues", new { partitionKey = model.MasterDataValueInContext.PartitionKey });
        }
    }
}