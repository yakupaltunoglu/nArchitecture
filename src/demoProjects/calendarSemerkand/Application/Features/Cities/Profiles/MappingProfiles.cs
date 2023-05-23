using Application.Features.Cities.Commands.CreateCity;
using Application.Features.Cities.Dtos;
using Application.Features.Cities.Models;
using AutoMapper;
using Core.Persistence.Paging;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cities.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<City,CreatedCityDto>().ReverseMap();
            CreateMap<City,CreateCityCommand>().ReverseMap();
            CreateMap<IPaginate<City>, CityListModel>().ReverseMap();
            CreateMap<City,CityListDto>().ReverseMap();
            CreateMap<City, CityGetByIdDto>().ReverseMap();
        }
    }
}
