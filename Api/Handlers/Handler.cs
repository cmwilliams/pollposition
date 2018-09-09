using Api.Config;
using Api.Models;
using Api.Models.Queries;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Handlers
{
    public class Handler : IRequestHandler<RepresentativeQuery, RepresentativeViewModel>
    {
        private readonly EnvironmentConfig _configuration;
        private readonly IDistributedCache _cache;

        public Handler(IOptions<EnvironmentConfig> configuration, IDistributedCache cache)
        {
            _configuration = configuration.Value;
            _cache = cache;
        }

        public Task<RepresentativeViewModel> Handle(RepresentativeQuery request, CancellationToken cancellationToken)
        {
            var vm = new RepresentativeViewModel();

            try
            {
                //Check the cache
                var cacheKey = request.Address;
                var representatives = _cache.GetString(cacheKey);

                if (!string.IsNullOrEmpty(representatives))
                {
                    return Task.FromResult(JsonConvert.DeserializeObject<RepresentativeViewModel>(representatives));

                }
                else
                {
                    //Call the Googles
                    var civicInfoService = new Google.Apis.CivicInfo.v2.CivicInfoService();
                    var representativeRequest = civicInfoService.Representatives.RepresentativeInfoByAddress();
                    representativeRequest.Address = request.Address;
                    representativeRequest.Key = _configuration.GoogleKey;
                    var representativeResponse = representativeRequest.Execute();

                    //Transformation of response into the model
                    var offices = representativeResponse.Offices.ToList();
                    var officials = representativeResponse.Officials.ToArray();

                    foreach (var office in offices)
                    {
                        var representative = new Representative
                        {
                            Office = office.Name,
                            Levels = office.Levels != null ? office.Levels.Select(govtLevel => new Level { GovtLevel = govtLevel }).ToList() : new List<Level>()
                        };

                        foreach (var indice in office.OfficialIndices)
                        {

                            var official = officials[Convert.ToInt64(indice)];

                            representative.Officials.Add(new Official
                            {
                                Name = official.Name,

                                Addresses = official.Address != null ? official.Address.Select(c => new Address
                                {
                                    Street = c.Line1,
                                    City = c.City,
                                    State = c.State,
                                    Zip = c.Zip
                                }).ToList() : new List<Address>(),

                                Party = official.Party,

                                PhoneNumbers = official.Phones != null ? official.Phones.Select(number => new Phone
                                {
                                    PhoneNumber = number
                                }).ToList() : new List<Phone>(),

                                Urls = official.Urls != null ? official.Urls.Select(url => new Url
                                {
                                    UrlAddress = url
                                }).ToList() : new List<Url>(),

                                PhotoUrl = official.PhotoUrl
                            });

                        }

                        vm.Representatives.Add(representative);
                    }

                    _cache.SetString(cacheKey, JsonConvert.SerializeObject(vm));

                    return Task.FromResult(vm);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}

