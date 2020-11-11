using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace RecipeApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCosmosRepository(Configuration);
            services.AddSwaggerGen(c =>
            {
                // turn on Open API doc generation using Swashbuckle
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RecipeApi", Version = "v1" });

                // read the XML doc comments into the Open API doc as documentation
                c.EnableXmlDocComments();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // customize the swagger URL and format
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "swagger/{documentName}/swagger.json";
                    c.SerializeAsV2 = true;
                });

                // turn on the nice UI page
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RecipeApi"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
