using Api;
using Microsoft.Extensions.Configuration;

namespace Tests
{
    public class ApiTestsStartup : Startup
    {
        public ApiTestsStartup(IConfiguration env) : base(env)
        {
        }

        
    }
}
