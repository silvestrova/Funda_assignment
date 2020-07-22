using Funda_assignment_Silvestrova.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funda_assignment_Silvestrova.Services
{
    public interface IListingService
    {
        Task<ListingsResult> GetListingsAsync(string [] sortParams, int pageNumber, int pageSize);
    }
}
