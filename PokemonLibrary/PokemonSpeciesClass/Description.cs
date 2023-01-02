using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonLibrary.PokemonSpeciesClass
{
    public class Description
    {
        [JsonProperty("flavor_text")]
        public string descriptionString { get; set; }

        [JsonProperty("language")]
        public DescriptionLanguage Language { get; set; }

    }
}
