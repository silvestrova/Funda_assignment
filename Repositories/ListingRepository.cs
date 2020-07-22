using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funda_assignment_Silvestrova.Models;
using Funda_assignment_Silvestrova.Services;
using Microsoft.Extensions.Logging;

namespace Funda_assignment_Silvestrova.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private IListingService listingService;
        private ILogger<ListingRepository> logger;
        private int pageSize;
        public ListingRepository(IListingService listingService, ILogger<ListingRepository> logger, int pageSize)
        {
            this.listingService = listingService;
            this.logger = logger;
            this.pageSize = pageSize;
        }

        public IEnumerable<Listing> GetListings(string[] sortParams)
        {
            var result = GetListingsAsync(sortParams).Result;
            return result;
        }
        public async Task<IEnumerable<Listing>> GetListingsAsync(string[] sortParams)
        {
            try
            {
                var result = listingService.GetListingsAsync(sortParams, 0, pageSize).Result;
                var tasks = new List<Task<ListingsResult>>();
                if (result == null || result.Paging==null)
                    throw new Exception("no result from the client");

                for (int i = result.Paging.CurrentPage + 1; i < result.Paging.TotalPages; i++)
                    tasks.Add(listingService.GetListingsAsync(sortParams, i, pageSize));

                var allResults = await Task.WhenAll(tasks);

                return result.Objects.Concat(allResults.SelectMany(x => x.Objects));
            }
            catch(Exception ex)
            {
                logger.LogError($"[ListingRepository]GetListingsAsync: something went wrong when receiving listings. {ex.Message}");
                return await Task.FromResult<IEnumerable<Listing>>(null);
            }
        }
    }
}
