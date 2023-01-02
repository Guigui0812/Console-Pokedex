using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonLibrary.PokemonSpeciesClass
{
    public class PokemonSpecies
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("evolution_chain")]
        public PokemonSpeciesEvoChain psEvoChain { get; set; }

        [JsonProperty("flavor_text_entries")]
        public List<Description> descriptionsListe { get; set; }
    }
}
