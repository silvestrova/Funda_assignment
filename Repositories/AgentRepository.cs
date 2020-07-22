using System.Collections.Generic;
using System.Linq;
using Funda_assignment_Silvestrova.Models;
using Microsoft.Extensions.Logging;

namespace Funda_assignment_Silvestrova.Repositories
{
    public class AgentRepository : IAgentRepository
    {
        private readonly IListingRepository listingRepository;
        private readonly ILogger<AgentRepository> logger;
        public AgentRepository(IListingRepository listingRepository, ILogger<AgentRepository> logger)
        {
            this.listingRepository = listingRepository;
            this.logger = logger;
        }
        public IEnumerable<Agent> GetOrderedAgents(string [] sortParams)
        {
            logger.LogDebug("[AgentRepository]GetOrderedAgents: getting listings");
            var listings = listingRepository.GetListings(sortParams);
            if (listings == null)
            {
                logger.LogError("[AgentRepository]GetOrderedAgents: null listings were obtained");
                return null;

            }
            var agentListings = listings.GroupBy(x => x.MakelaarId);
            var agents = agentListings.Select(x=>MapListingToAgent(x));
            return agents.OrderByDescending(x => x.ListingsAmount);
        }
        private Agent MapListingToAgent(IGrouping<long, Listing> group)
        {
            var listingInfo = group.FirstOrDefault();
            return new Agent()
            {
                AgentId = listingInfo.MakelaarId.ToString(),
                AgentName = listingInfo.MakelaarNaam,
                ListingsAmount = group.Count()
            };
        }
    }
}
