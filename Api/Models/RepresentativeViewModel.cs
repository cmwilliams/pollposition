using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class RepresentativeViewModel
    {
        public List<Representative> Representatives { get; set; }

        public RepresentativeViewModel()
        {
            Representatives = new List<Representative>();
        }
    }
}
