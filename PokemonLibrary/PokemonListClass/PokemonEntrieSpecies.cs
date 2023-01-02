using Newtonsoft.Json;

namespace PokemonLibrary.PokemonListClass
{
    public class PokemonEntrieSpecies
    {
        [JsonProperty("name")]
        public string name { get; set; }
    }
}
