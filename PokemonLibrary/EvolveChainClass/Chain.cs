using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PokemonLibrary.EvolveChainClass
{
    public class Chain
    {
        [JsonProperty("evolves_to")]
        public List<EvolvesTo> evolves_to { get; set; }

        [JsonProperty("species")]
        public Species species { get; set; }
    }
}
