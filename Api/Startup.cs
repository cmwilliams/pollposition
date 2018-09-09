using Api.Config;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NJsonSchema;
using NSwag;
using NSwag.AspNetCore;
using System.Reflection;
using System.Threading.Tasks;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<EnvironmentConfig>(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var settings = Configuration.Get<EnvironmentConfig>();

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = settings.RedisConnectionString;
                options.InstanceName = "pollposition";
            });

            services.AddMediatR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseExceptionHandler("/error/500");

            app.UseStaticFiles();           

            // Enable the Swagger UI middleware and the Swagger generator
            app.UseSwaggerUi3(typeof(Startup).GetTypeInfo().Assembly, settings =>
            {
                
                settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;
                settings.PostProcess = document =>
                {
                    document.Schemes.Add(SwaggerSchema.Http);
                    document.Schemes.Add(SwaggerSchema.Https);
                    document.Info.Version = "v1";
                    document.Info.Title = "Poll Position";
                    document.Info.Description = "API that returns elected officials by address";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new SwaggerContact
                    {
                        Name = "Mike Williams",
                        Email = "cmwilliams@gmail.com",
                        Url = "https://mikewilliams.io",
                    };
                };
            });

            app.UseMvc();

            //Make swagger the default page
            app.Run(c => {
                c.Response.Redirect("swagger");
                return Task.CompletedTask;
            });

        }
    }
}
