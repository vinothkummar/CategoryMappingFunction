using Newtonsoft.Json;

namespace GFCCategoryMappingFunction.Models
{
    public class InspectionCategory
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("primary")]
        public string Primary { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
