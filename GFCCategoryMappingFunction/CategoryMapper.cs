using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GFCCategoryMappingFunction.Models;
using GFCCategoryMappingFunction.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;

namespace GFCCategoryMappingFunction
{
    public static class CategoryMapper
    {
        [FunctionName("CategoryMapper")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log,
            [Inject] ICategoryMappingsService mappingSvc)
        {
            //Let's get our public key
            var publicApiKey = "490f1224-1a44-4243-9e0e-5e78e5463517-8eba491a-7472-44fe-8bc2-eb64d81ccf1e";

            //Check the key against what we have stored and reject the call if not using the correct key
            if (req.Headers.TryGetValue("publicKey", out var headerValues))
            {
                var publicKey = headerValues.First();
                if (publicKey != publicApiKey)
                {
                    return new BadRequestObjectResult("Forbidden");
                }
            }
            else
            {
                return new BadRequestObjectResult("Forbidden");
            }

            if (req.ContentLength > 0)
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync().ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(requestBody))
                {
                    var inspectionCategories = JsonConvert.DeserializeObject<List<InspectionCategory>>(requestBody);
                    if (inspectionCategories?.Count > 0)
                    {
                        var result = new List<string>();
                        inspectionCategories.ForEach(x =>
                        {
                            var value = mappingSvc.UpdateCategoryMappings(x.Name);
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                result.Add(value);
                            }
                        });
                        return new OkObjectResult(result);
                    }
                }
            }
            return new BadRequestObjectResult("No inspection categories found");
        }

    }
}
