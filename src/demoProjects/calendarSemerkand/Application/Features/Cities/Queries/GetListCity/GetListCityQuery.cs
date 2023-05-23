using Application.Features.Cities.Models;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Requests;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cities.Queries.GetListCity
{
    public class GetListCityQuery:IRequest<CityListModel>
    {
        public PageRequest PageRequest { get; set; }
        public class GetListCityQueryHandler : IRequestHandler<GetListCityQuery, CityListModel>
        {
            private readonly ICityRepository _CityRepository;
            private readonly IMapper _mapper;

            public GetListCityQueryHandler(ICityRepository CityRepository, IMapper mapper)
            {
                _CityRepository = CityRepository;
                _mapper = mapper;
            }

            public async Task<CityListModel> Handle(GetListCityQuery request, CancellationToken cancellationToken)
            {
                IPaginate<City> Citys = await _CityRepository.GetListAsync(index: request.PageRequest.Page,size:request.PageRequest.PageSize);

                CityListModel mappedCityListModel = _mapper.Map<CityListModel>(Citys);

                return mappedCityListModel;
            }
        }
    }
}
