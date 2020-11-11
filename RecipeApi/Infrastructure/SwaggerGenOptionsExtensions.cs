using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace Microsoft.Extensions.Hosting
{
    public static class SwaggerGenOptionsExtensions
    {
        public static SwaggerGenOptions EnableXmlDocComments(this SwaggerGenOptions options)
        {
            // generate the xml docs that'll drive the swagger docs
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            return options;
        }
    }
}
