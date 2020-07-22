using Funda_assignment_Silvestrova.Configuration;
using Funda_assignment_Silvestrova.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Funda_assignment_Silvestrova.Services
{
    public class ListingService:IListingService
    {
        private readonly HttpClient httpClient;
        private readonly IConfigurationHelper configurationHelper;
        private readonly ILogger<ListingService> logger;

        public ListingService(HttpClient httpClient, IConfigurationHelper configurationHelper,
            ILogger<ListingService> logger)
        {
            this.httpClient = httpClient;
            this.configurationHelper = configurationHelper;
            this.logger = logger;
        }

        public async Task<ListingsResult> GetListingsAsync(string [] sortParams, int pageNumber, int pageSize)
        {
            var uri = configurationHelper.CombineListingsRequestUrl(sortParams, pageNumber, pageSize);
            var response = await httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                logger.LogDebug($"[ListingService]GetListingsAsync: obtained listings for {pageNumber} page sucessfuly");
                var content = response.Content.ReadAsStringAsync().Result;         
                var listings = JsonConvert.DeserializeObject<ListingsResult>(content);
               
                return listings;
            }
            logger.LogError($"[ListingService]GetListingsAsync: failed to get result for {uri} request");
            return await Task.FromResult<ListingsResult>(null);
        }
    }
}
