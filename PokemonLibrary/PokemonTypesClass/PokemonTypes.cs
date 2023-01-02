using PokemonLibrary.EvolveChainClass;
using PokemonLibrary.PokemonSpeciesClass;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PokemonLibrary.PokemonTypesClass
{
    public class PokemonTypes
    {
        [JsonPropertyName("types")]
        public List<Slot> types { get; set; }
    }
}
