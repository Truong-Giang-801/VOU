﻿using Vou.Web.Models;
using Vou.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using Vou.Web.Models.Dto;


namespace Vou.Web.Controllers
{
	public class BrandController : Controller
	{
		private readonly IBrandService _BrandService;
		public BrandController(IBrandService BrandService)
		{
			_BrandService = BrandService;
		}


		public async Task<IActionResult> BrandIndex()
		{
			List<BrandDto>? list = new();

			ResponseDto? response = await _BrandService.GetAllBrandAsync();

			if (response !=null && response.IsSuccess == true)
			{
				list = JsonConvert.DeserializeObject<List<BrandDto>>(Convert.ToString(response.Result));
			}
			else
			{
				TempData["error"] = response?.Message;
			}

			return View(list);
		}

		public async Task<IActionResult> BrandCreate()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> BrandCreate(BrandDto model)
		{
			if (ModelState.IsValid)
			{
				ResponseDto? response = await _BrandService.CreateBrandAsync(model);

				if (response != null && response.IsSuccess == true)
				{
					TempData["success"] = "Brand created successfully";
					return RedirectToAction(nameof(BrandIndex));
				}
				else
				{
					TempData["error"] = response?.Message;
				}
			}
			return View(model);
		}

		public async Task<IActionResult> BrandDelete(int BrandId)
		{
			ResponseDto? response = await _BrandService.GetBrandByIdAsync(BrandId);

			if (response != null && response.IsSuccess == true)
			{
				BrandDto? model = JsonConvert.DeserializeObject<BrandDto>(Convert.ToString(response.Result));
				return View(model);
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> BrandDelete(BrandDto BrandDto)
		{
			ResponseDto? response = await _BrandService.DeleteBrandAsync(BrandDto.Id);

			if (response != null && response.IsSuccess == true)
			{
				TempData["success"] = "Brand deleted successfully";
				return RedirectToAction(nameof(BrandIndex));
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return View(BrandDto);
		}

	}
}