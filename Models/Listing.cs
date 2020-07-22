using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funda_assignment_Silvestrova.Models
{
   public class Listing
    {
        [JsonProperty("MakelaarId")]
        public long MakelaarId { get; set; }

        [JsonProperty("MakelaarNaam")]
        public string MakelaarNaam { get; set; }
    }
}
