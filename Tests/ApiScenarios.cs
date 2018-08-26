using Api.Config;
using Api.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ApiScenarios : ApiScenarioBase
    {
        [Fact]
        public async Task First_office_is_president()
        {
            using (var server = CreateServer())
            {
                server.BaseAddress = new System.Uri(BaseUrl);

                //Arrange
                var address = "Atlanta";
                var expectedOffice = "President of the United States";

                // Act
                var officeReponse = await server.CreateClient().GetAsync(Get.RepresentativesBy(address));
                var responseBody = await officeReponse.Content.ReadAsStringAsync();
                var representativeResult = JsonConvert.DeserializeObject<ApiOkResponse>(responseBody);
                var office = JsonConvert.DeserializeObject<RepresentativeViewModel>(representativeResult.Result.ToString());

                // Assert
                Assert.Equal(expectedOffice, office.Representatives.FirstOrDefault().Office);
            }
        }

    }
}
