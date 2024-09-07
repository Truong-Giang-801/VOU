using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Vou.Services.BrandAPI.Data;
using Vou.Services.BrandAPI.Models;
using Vou.Services.BrandAPI.Models.Dto;

namespace Vou.Services.BrandAPI.Controllers
{
	[Route("api/brand")]
	[ApiController]
	[Authorize]
	public class BrandController : ControllerBase
	{
		private readonly AppDbContext _db;
		private ResponeDto _responeDto;
		private IMapper _mapper;
		public BrandController(AppDbContext db, IMapper mapper)
		{
			_db = db;
			_responeDto = new ResponeDto();
			_mapper = mapper;

		}

		[HttpGet]
        [Authorize(Roles = "ADMIN")]
        public ResponeDto Get()
		{
			try
			{
				IEnumerable<Brand> objList = _db.Brand.ToList();
				_responeDto.Result = _mapper.Map<IEnumerable<BrandDto>>(objList);
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
        [Authorize(Roles = "ADMIN")]
        public ResponeDto Get(int id)
		{
			try
			{
				Brand objList = _db.Brand.First(u => u.Id == id);
				_responeDto.Result = _mapper.Map<BrandDto>(objList);
			}
			catch (Exception ex)
			{
				_responeDto.IsSuccess = false;
				_responeDto.Message = ex.Message;
			}
			return _responeDto;
		}

		[HttpPost]
		[Authorize(Roles = "ADMIN")]
		public ResponeDto Post([FromBody] BrandDto brandDto)
		{
			try
			{
				Brand obj = _mapper.Map<Brand>(brandDto);
				_db.Brand.Add(obj);
				_db.SaveChanges();

				_responeDto.Result = _mapper.Map<BrandDto>(obj);
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
        [Authorize(Roles = "ADMIN")]
        public ResponeDto Put([FromBody] BrandDto brandDto)
		{
			try
			{
				Brand obj = _mapper.Map<Brand>(brandDto);
				_db.Brand.Update(obj);
				_db.SaveChanges();

				_responeDto.Result = _mapper.Map<BrandDto>(obj);
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
        [Authorize(Roles = "ADMIN")]
        public ResponeDto Delete(int id)
		{
			try
			{
				Brand obj = _db.Brand.First(u => u.Id == id);
				_db.Brand.Remove(obj);
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
