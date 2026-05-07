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
            // THÊM NULL CHECK
            if (model.MasterDataKeyInContext == null)
            {
                model.MasterDataKeyInContext = new MasterDataKeyViewModel { IsActive = true };
                var keys0 = await _masterData.GetMasterDataKeysAsync();
                model.MasterDataKeys = _mapper.Map<List<MasterDataKeyViewModel>>(keys0);
                return View(model);
            }

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
            masterDataKey.Name = model.MasterDataKeyInContext.Name;
            masterDataKey.IsActive = model.MasterDataKeyInContext.IsActive;
            masterDataKey.PartitionKey = model.MasterDataKeyInContext.Name;

            // SỬA: Gán RowKey qua property của BaseEntity
            masterDataKey.RowKey = Guid.NewGuid().ToString(); // Dòng này giữ nguyên

            masterDataKey.CreatedDate = DateTime.UtcNow;
            masterDataKey.UpdatedDate = DateTime.UtcNow;
            masterDataKey.CreatedBy = User.Identity?.Name ?? "admin";
            masterDataKey.UpdatedBy = User.Identity?.Name ?? "admin";
            masterDataKey.IsDeleted = false;

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

            var masterDataValue = new MasterDataValue();
            masterDataValue.Name = model.MasterDataValueInContext.Name;
            masterDataValue.Value = model.MasterDataValueInContext.Value;
            masterDataValue.IsActive = model.MasterDataValueInContext.IsActive;
            masterDataValue.PartitionKey = model.MasterDataValueInContext.PartitionKey;
            masterDataValue.RowKey = Guid.NewGuid().ToString();
            masterDataValue.CreatedDate = DateTime.UtcNow;
            masterDataValue.UpdatedDate = DateTime.UtcNow;
            masterDataValue.CreatedBy = User.Identity?.Name ?? "admin";
            masterDataValue.UpdatedBy = User.Identity?.Name ?? "admin";
            masterDataValue.IsDeleted = false;

            await _masterData.InsertMasterDataValueAsync(masterDataValue);

            // Sau khi thêm, quay lại trang danh sách giá trị của đúng Nhóm đó
            return RedirectToAction("MasterValues", new { partitionKey = model.MasterDataValueInContext.PartitionKey });
        }
    }
}