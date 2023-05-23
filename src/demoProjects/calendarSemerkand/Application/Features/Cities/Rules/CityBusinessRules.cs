using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cities.Rules
{
    public class CityBusinessRules
    {
        private readonly ICityRepository _CityRepository;

        public CityBusinessRules(ICityRepository CityRepository)
        {
            _CityRepository = CityRepository;
        }

        public async Task CityNameCanNotBeDuplicatedWhenInserted(string name)
        {
            IPaginate<City> result = await _CityRepository.GetListAsync(b => b.Name == name);
            if (result.Items.Any()) throw new BusinessException("City name exists.");
        }

        public void CityShouldExistWhenRequested(City City)
        {
            if (City == null) throw new BusinessException("Requested City does not exist");
        }
    }
}
