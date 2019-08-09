using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GFCCategoryMappingFunction.Models;
using GFCCategoryMappingFunction.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
            [Inject]ICategoryMappingsService service)
        {
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
                            var value = service.UpdateCategoryMappings(x.Name);
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
