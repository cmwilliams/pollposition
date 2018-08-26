using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class Official
    {
        public string Name { get; set; }
        public string Party { get; set; }
        public string PhotoUrl { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Phone> PhoneNumbers { get; set; }
        public List<Url> Urls { get; set; }
        public List<Email> EmailAddresses { get; set; }

        public Official()
        {
            Addresses = new List<Address>();
            PhoneNumbers = new List<Phone>();
            Urls = new List<Url>();
            EmailAddresses = new List<Email>();
        }
    }
}
