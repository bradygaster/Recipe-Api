using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeRating
{
    public class OpenApiConfiguration : IOpenApiConfigurationOptions
    {
        public OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Version = "1.0.0",
            Title = "Recipe Rating Engine",
            Description = "Functions used to rate recipes in the database",
            Contact = new OpenApiContact()
            {
                Name = "Contoso",
                Email = "recipes@contoso.com"
            }
        };

        public List<OpenApiServer> Servers
        {
            get
            {
                return (new OpenApiServer[] { new OpenApiServer { Url =
                    Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")
                }}).ToList();
            }
            set => throw new NotImplementedException();
        }

        public OpenApiVersionType OpenApiVersion { get => OpenApiVersionType.V2; set => throw new NotImplementedException(); }
        public bool IncludeRequestingHostName { get => true; set => throw new NotImplementedException(); }
    }
}
