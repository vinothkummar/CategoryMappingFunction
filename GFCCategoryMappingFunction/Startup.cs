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
                .AddJsonFile("Content/local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            //var applicationSettings = configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();
            //var securitySettings = configuration.GetSection("SecuritySettings").Get<SecuritySettings>();

            //services.TryAddSingleton<IApplicationSettings>(applicationSettings);
            //services.TryAddSingleton<ISecuritySettings>(securitySettings);

            var CosmosDBEndpointUrl = "https://location-search-prod-cosmos.documents.azure.com:443/";
            var AuthorizationKey = "EU8tabfOthQcICMMKIc3k33Z4U8jpK5BGYDCKNf3Fu43TUgStRDTMVNd4v7VvOW7ieM8TxLVuu1pB28AV64ZhQ==";

            services.TryAddSingleton<IDocumentClient>(
                new DocumentClient(
                    new Uri(CosmosDBEndpointUrl),
                    AuthorizationKey));

            services.TryAddScoped(typeof(ICategoryMappingsService), typeof(CategoryMappingsService));
            //services.TryAddScoped(typeof(ISecuritySettingsService), typeof(SecuritySettingsService));
        }
    }
}
