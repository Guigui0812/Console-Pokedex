using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonLibrary.PokemonSpeciesClass
{
    public class DescriptionLanguage
    {
        [JsonProperty("name")]
        public string languageName { get; set; }
    }
}
