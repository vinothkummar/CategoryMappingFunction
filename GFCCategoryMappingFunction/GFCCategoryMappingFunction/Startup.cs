using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GFCCategoryMappingFunction;
using GFCCategoryMappingFunction.Models;
using GFCCategoryMappingFunction.Services;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Startup))]
namespace GFCCategoryMappingFunction
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddDependencyInjection(ConfigureServices);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var applicationSettings = configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();

            services.TryAddSingleton<IApplicationSettings>(applicationSettings);

            services.TryAddSingleton<IDocumentClient>(
                new DocumentClient(
                    new Uri(applicationSettings.CosmosDBEndpointUrl),
                    applicationSettings.AuthorizationKey));

            services.TryAddScoped(typeof(ICategoryMappingsService), typeof(CategoryMappingsService));

        }
    }
}
