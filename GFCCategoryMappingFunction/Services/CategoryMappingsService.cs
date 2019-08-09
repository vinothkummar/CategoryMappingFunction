using System.Linq;
using GFCCategoryMappingFunction.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace GFCCategoryMappingFunction.Services
{
    public interface ICategoryMappingsService
    {
        string UpdateCategoryMappings(string originalString);
    }

    public class CategoryMappingsService : ICategoryMappingsService
    {
        private readonly string _databaseId;
        private readonly string _collectionId;
        private readonly IDocumentClient _client;

        public CategoryMappingsService(IDocumentClient client)
        {
            _databaseId = "CQCData";
            _collectionId = "CategoryMappings";
            _client = client;
        }

        public string UpdateCategoryMappings(string originalString)
        {
            IQueryable<CategoryMap> query =
                _client
                    .CreateDocumentQuery<CategoryMap>(
                        UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
                        new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = true }
                    );
            var categoryMapping = query?.AsEnumerable()?.FirstOrDefault() ?? null;
            var result = categoryMapping?.Categories?.FirstOrDefault(x => x.Key == originalString) ?? null;
            return result?.Value ?? string.Empty;
        }
    }
}
