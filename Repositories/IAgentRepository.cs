using Funda_assignment_Silvestrova.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funda_assignment_Silvestrova.Repositories
{
    public interface IAgentRepository
    {
        IEnumerable<Agent> GetOrderedAgents(string [] sortParams);
    }
}
