using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonLibrary.EvolveChainClass
{
    public class EvolvesTo
    {
        [JsonProperty("evolves_to")]
        public List<EvolvesTo> evolves_to { get; set; }

        [JsonProperty("species")]
        public Species species { get; set; }
    }
}
