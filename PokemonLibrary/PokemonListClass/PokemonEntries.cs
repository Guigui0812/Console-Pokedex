using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonLibrary.PokemonListClass
{
    public class PokemonEntries
    {
        [JsonProperty("entry_number")]
        public string entryNumber { get; set; }

        [JsonProperty("pokemon_species")]
        public PokemonEntrieSpecies pokemonEntrySpecie { get; set; }
    }
}
