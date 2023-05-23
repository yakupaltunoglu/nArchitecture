using Application.Features.Cities.Dtos;
using Application.Features.Cities.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cities.Commands.CreateCity
{
    public partial class CreateCityCommand:IRequest<CreatedCityDto>
    {
        public string Name { get; set; }

        public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, CreatedCityDto>
        {
            private readonly ICityRepository _CityRepository;
            private readonly IMapper _mapper;
            private readonly CityBusinessRules _CityBusinessRules;

            public CreateCityCommandHandler(ICityRepository CityRepository, IMapper mapper, CityBusinessRules CityBusinessRules)
            {
                _CityRepository = CityRepository;
                _mapper = mapper;
                _CityBusinessRules = CityBusinessRules;
            }

            public async Task<CreatedCityDto> Handle(CreateCityCommand request, CancellationToken cancellationToken)
            {
                await _CityBusinessRules.CityNameCanNotBeDuplicatedWhenInserted(request.Name);

                City mappedCity = _mapper.Map<City>(request);
                City createdCity = await _CityRepository.AddAsync(mappedCity);
                CreatedCityDto createdCityDto = _mapper.Map<CreatedCityDto>(createdCity);

                return createdCityDto;

            }
        }
    }
}
