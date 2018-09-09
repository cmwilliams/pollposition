using System.Collections.Generic;

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
