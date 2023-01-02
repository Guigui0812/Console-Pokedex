using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonLibrary.EvolveChainClass
{
    public class Species
    {
        [JsonProperty("name")]
        public string name { get; set; }
    }
}
