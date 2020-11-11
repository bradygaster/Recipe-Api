using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCosmosRepository(this IServiceCollection services,
            IConfiguration Configuration)
        {
            services.AddCosmosRepository(Configuration, config =>
            {
                config.CosmosConnectionString = Configuration.GetConnectionString("RecipesConnectionString");
                config.DatabaseId = "RecipeDatabase";
                config.ContainerPerItemType = true;
            });

            return services;
        }
    }
}
