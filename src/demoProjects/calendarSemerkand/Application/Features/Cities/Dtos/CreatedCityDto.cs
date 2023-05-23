using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cities.Dtos
{
    public class CreatedCityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime PostDate { get; set; }
    }
}
