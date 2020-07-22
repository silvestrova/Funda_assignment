using Newtonsoft.Json;

namespace Funda_assignment_Silvestrova.Models
{
    public class Paging
    {
        [JsonProperty("AantalPaginas")]
        public int TotalPages { get; set; }

        [JsonProperty("HuidigePagina")]
        public int CurrentPage { get; set; }

    }
}
