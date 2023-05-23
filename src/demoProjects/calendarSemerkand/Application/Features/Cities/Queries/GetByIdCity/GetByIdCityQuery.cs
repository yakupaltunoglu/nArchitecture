using Application.Features.Cities.Dtos;
using Application.Features.Cities.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cities.Queries.GetByIdCity
{
    public class GetByIdCityQuery:IRequest<CityGetByIdDto>
    {
        public int Id { get; set; }
        public class GetByIdCityQueryHandler : IRequestHandler<GetByIdCityQuery, CityGetByIdDto>
        {
            private readonly ICityRepository _CityRepository;
            private readonly IMapper _mapper;
            private readonly CityBusinessRules _CityBusinessRules;

            public GetByIdCityQueryHandler(ICityRepository CityRepository, IMapper mapper, CityBusinessRules CityBusinessRules)
            {
                _CityRepository = CityRepository;
                _mapper = mapper;
                _CityBusinessRules = CityBusinessRules;
            }

            public async Task<CityGetByIdDto> Handle(GetByIdCityQuery request, CancellationToken cancellationToken)
            {
               City? City =  await _CityRepository.GetAsync(b=>b.Id==request.Id);

               _CityBusinessRules.CityShouldExistWhenRequested(City);

               CityGetByIdDto CityGetByIdDto = _mapper.Map<CityGetByIdDto>(City);
               return CityGetByIdDto;
            }
        }
    }
}
