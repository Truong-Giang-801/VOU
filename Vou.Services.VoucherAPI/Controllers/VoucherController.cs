using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Vou.Services.VoucherAPI.Data;
using Vou.Services.VoucherAPI.Models;
using Vou.Services.VoucherAPI.Models.Dto;

namespace Vou.Services.VoucherAPI.Controllers
{
	[Route("api/Voucher")]
	[ApiController]
	public class VoucherController : ControllerBase
	{
		private readonly AppDbContext _db;
		private ResponeDto _responeDto;
		private IMapper _mapper;
		public VoucherController(AppDbContext db, IMapper mapper)
		{
			_db = db;
			_responeDto = new ResponeDto();
			_mapper = mapper;

		}

		[HttpGet]
        public ResponeDto Get()
		{
			try
			{
				IEnumerable<Voucher> objList = _db.Voucher.ToList();
				_responeDto.Result = _mapper.Map<IEnumerable<VoucherDto>>(objList);
			}
			catch (Exception ex)
			{
				_responeDto.IsSuccess = false;
				_responeDto.Message = ex.Message;
			}
			return _responeDto;
		}
		[HttpGet]
		[Route("{id:int}")]
        public ResponeDto Get(int id)
		{
			try
			{
				Voucher objList = _db.Voucher.First(u => u.Id == id);
				_responeDto.Result = _mapper.Map<VoucherDto>(objList);
			}
			catch (Exception ex)
			{
				_responeDto.IsSuccess = false;
				_responeDto.Message = ex.Message;
			}
			return _responeDto;
		}

		[HttpPost]
		public ResponeDto Post([FromBody] VoucherDto VoucherDto)
		{
			try
			{
				Voucher obj = _mapper.Map<Voucher>(VoucherDto);
				_db.Voucher.Add(obj);
				_db.SaveChanges();

				_responeDto.Result = _mapper.Map<VoucherDto>(obj);
			}
			catch (Exception ex)
			{
				_responeDto.IsSuccess = false;
				_responeDto.Message = ex.Message;
			}
			return _responeDto;
		}
		[HttpPut]
        [Route("{id:int}")]
        public ResponeDto Put([FromBody] VoucherDto VoucherDto)
		{
			try
			{
				Voucher obj = _mapper.Map<Voucher>(VoucherDto);
				_db.Voucher.Update(obj);
				_db.SaveChanges();

				_responeDto.Result = _mapper.Map<VoucherDto>(obj);
			}
			catch (Exception ex)
			{
				_responeDto.IsSuccess = false;
				_responeDto.Message = ex.Message;
			}
			return _responeDto;
		}

		[HttpDelete]
        [Route("{id:int}")]
        public ResponeDto Delete(int id)
		{
			try
			{
				Voucher obj = _db.Voucher.First(u => u.Id == id);
				_db.Voucher.Remove(obj);
				_db.SaveChanges();

			}
			catch (Exception ex)
			{
				_responeDto.IsSuccess = false;
				_responeDto.Message = ex.Message;
			}
			return _responeDto;
		}
	}
}
