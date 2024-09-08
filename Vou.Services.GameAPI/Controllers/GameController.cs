using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Vou.Services.GameAPI.Data;
using Vou.Services.GameAPI.Models;
using Vou.Services.GameAPI.Models.Dto;

namespace Vou.Services.GameAPI.Controllers
{
	[Route("api/Game")]
	[ApiController]
	public class GameController : ControllerBase
	{
		private readonly AppDbContext _db;
		private ResponeDto _responeDto;
		private IMapper _mapper;
		public GameController(AppDbContext db, IMapper mapper)
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
				IEnumerable<Game> objList = _db.Game.ToList();
				_responeDto.Result = _mapper.Map<IEnumerable<GameDto>>(objList);
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
				Game objList = _db.Game.First(u => u.Id == id);
				_responeDto.Result = _mapper.Map<GameDto>(objList);
			}
			catch (Exception ex)
			{
				_responeDto.IsSuccess = false;
				_responeDto.Message = ex.Message;
			}
			return _responeDto;
		}

		[HttpPost]
		public ResponeDto Post([FromBody] GameDto GameDto)
		{
			try
			{
				Game obj = _mapper.Map<Game>(GameDto);
				_db.Game.Add(obj);
				_db.SaveChanges();

				_responeDto.Result = _mapper.Map<GameDto>(obj);
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
        public ResponeDto Put([FromBody] GameDto GameDto)
		{
			try
			{
				Game obj = _mapper.Map<Game>(GameDto);
				_db.Game.Update(obj);
				_db.SaveChanges();

				_responeDto.Result = _mapper.Map<GameDto>(obj);
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
				Game obj = _db.Game.First(u => u.Id == id);
				_db.Game.Remove(obj);
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
