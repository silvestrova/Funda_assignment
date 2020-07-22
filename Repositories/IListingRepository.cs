using Funda_assignment_Silvestrova.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funda_assignment_Silvestrova.Repositories
{
    public interface IListingRepository
    {
        IEnumerable<Listing> GetListings(string [] sortParams);
        Task<IEnumerable<Listing>> GetListingsAsync(string[] sortParams);
    }
}
