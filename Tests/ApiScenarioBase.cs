using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;


namespace Tests
{
    public class ApiScenarioBase
    {
        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(ApiScenarioBase)).Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("appsettings.json", optional: true)
                    .AddEnvironmentVariables();
                }).UseStartup<ApiTestsStartup>();

            return new TestServer(hostBuilder);
        }

        public string BaseUrl { get { return "http://localhost:5209/"; } }

        public static class Get
        {
            public static string RepresentativesBy(string address)
            {
                return $"api/representatives?address={address}";
            }
        }
    }
}
