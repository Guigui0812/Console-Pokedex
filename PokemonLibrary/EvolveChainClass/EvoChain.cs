using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PokemonLibrary.EvolveChainClass
{
    public class EvoChain
    {
        [JsonProperty("chain")]
        public Chain chain { get; set; }
    }
}
