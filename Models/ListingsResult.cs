using Newtonsoft.Json;

namespace Funda_assignment_Silvestrova.Models
{
    public class ListingsResult
    {
        [JsonProperty("Objects")]
        public Listing[] Objects { get; set; }

        [JsonProperty("Paging")]
        public Paging Paging { get; set; }

    }
}
