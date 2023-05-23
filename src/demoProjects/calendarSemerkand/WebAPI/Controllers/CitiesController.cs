using Application.Features.Cities.Models;
using Application.Features.Cities.Commands.CreateCity;
using Application.Features.Cities.Dtos;
using Application.Features.Cities.Queries.GetByIdCity;
using Application.Features.Cities.Queries.GetListCity;
using Core.Application.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCityCommand createCityCommand)
        {
            CreatedCityDto result = await Mediator.Send(createCityCommand);
            return Created("", result);
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListCityQuery getListCityQuery = new() { PageRequest = pageRequest };
            CityListModel result = await Mediator.Send(getListCityQuery);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] GetByIdCityQuery getByIdIdCityQuery)
        {
            CityGetByIdDto CityGetByIdDto = await Mediator.Send(getByIdIdCityQuery);
            return Ok(CityGetByIdDto);
        }
    }
}
