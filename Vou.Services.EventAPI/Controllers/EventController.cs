using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Vou.Services.EventAPI.Data;
using Vou.Services.EventAPI.Models;
using Vou.Services.EventAPI.Models.Dto;

namespace Vou.Services.EventAPI.Controllers
{
	[Route("api/Event")]
	[ApiController]
	public class EventController : ControllerBase
	{
		private readonly AppDbContext _db;
		private ResponeDto _responeDto;
		private IMapper _mapper;
		public EventController(AppDbContext db, IMapper mapper)
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
				IEnumerable<Event> objList = _db.Event.ToList();
				_responeDto.Result = _mapper.Map<IEnumerable<EventDto>>(objList);
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
				Event objList = _db.Event.First(u => u.Id == id);
				_responeDto.Result = _mapper.Map<EventDto>(objList);
			}
			catch (Exception ex)
			{
				_responeDto.IsSuccess = false;
				_responeDto.Message = ex.Message;
			}
			return _responeDto;
		}

		[HttpPost]
		public ResponeDto Post([FromBody] EventDto EventDto)
		{
			try
			{
				Event obj = _mapper.Map<Event>(EventDto);
				_db.Event.Add(obj);
				_db.SaveChanges();

				_responeDto.Result = _mapper.Map<EventDto>(obj);
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
        public ResponeDto Put([FromBody] EventDto EventDto)
		{
			try
			{
				Event obj = _mapper.Map<Event>(EventDto);
				_db.Event.Update(obj);
				_db.SaveChanges();

				_responeDto.Result = _mapper.Map<EventDto>(obj);
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
				Event obj = _db.Event.First(u => u.Id == id);
				_db.Event.Remove(obj);
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
