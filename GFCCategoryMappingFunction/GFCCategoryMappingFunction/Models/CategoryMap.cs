using System.Collections.Generic;
using Newtonsoft.Json;

namespace GFCCategoryMappingFunction.Models
{
    public class CategoryMap
    {
        [JsonProperty("id")]
        public object Id { get; set; }

        [JsonProperty("categories")]
        public IDictionary<string, string> Categories { get; set; }
    }
}
