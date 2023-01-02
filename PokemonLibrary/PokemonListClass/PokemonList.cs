using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonLibrary.PokemonListClass
{
    public class PokemonList
    {
        [JsonProperty("pokemon_entries")]
        public List<PokemonEntries> pokemonEntries { get; set; }

        [JsonIgnore]
        public int nbMaxPokemon { get; set; }
    }
}
