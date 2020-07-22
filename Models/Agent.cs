using System;
using System.Collections.Generic;
using System.Text;

namespace Funda_assignment_Silvestrova.Models
{
    public class Agent
    {
        public string AgentId { get; set; }
        public string AgentName { get; set; }
        public int ListingsAmount { get; set; }
        public string GetAgentDescription()
        {
            return $"{AgentId}: {AgentName}";
        }
    }
}
