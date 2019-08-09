using System;
using System.Collections.Generic;
using System.Text;

namespace GFCCategoryMappingFunction.Models
{
    public interface IApplicationSettings
    {
        string CosmosDBEndpointUrl { get; set; }
        string AuthorizationKey { get; set; }
        string DatabaseId { get; set; }
        string CollectionId { get; set; }
    }

    public class ApplicationSettings : IApplicationSettings
    {
        public string CosmosDBEndpointUrl { get; set; }
        public string AuthorizationKey { get; set; }
        public string DatabaseId { get; set; }
        public string CollectionId { get; set; }
    }
}
