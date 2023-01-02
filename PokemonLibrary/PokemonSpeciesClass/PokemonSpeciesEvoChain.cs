using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonLibrary.PokemonSpeciesClass
{
    public class PokemonSpeciesEvoChain
    {
        [JsonProperty("url")]
        public string evoChainUrl { get; set; }
    }
}
