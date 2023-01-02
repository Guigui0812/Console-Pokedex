using GetPokedexData;
using PokemonLibrary.EvolveChainClass;
using PokemonLibrary.PokemonListClass;
using PokemonLibrary.PokemonSpeciesClass;
using PokemonLibrary.PokemonTypesClass;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PokedexTest
{
    public class UnitTestGetPokemon
    {
        [Fact]
        public async Task GetDataFromAPIAsyncTest()
        {
            // Tentative correcte

            string requete = "https://pokeapi.co/api/v2/pokemon/1/";

            string actualResponse = await GetData.GetDataFromAPIAsync(requete, "test");

            Assert.False(String.IsNullOrEmpty(actualResponse));

            // Tentative incorrecte

            requete = "https://pokeapi.co/api/v2/poken/1/";

            actualResponse = await GetData.GetDataFromAPIAsync(requete, "test");

            Assert.True(String.IsNullOrEmpty(actualResponse));
        }

        [Fact]
        public void ObjectIsTradInPokemonTypeTest()
        {
            string jsonString = "{\"id\": 25,\"name\":\"pikachu\",\"order\": 35,\"types\":[{\"slot\":1,\"type\":{\"name\":\"electric\",\"url\":\"https://pokeapi.co/api/v2/type/13/\"}}]}";
            PokemonTypes pokemonTest = JsonToObject.jsonToObject<PokemonTypes>(jsonString);
            Assert.Equal("PokemonTypes", pokemonTest.GetType().Name);
            Assert.Equal("electric", pokemonTest.types[0].PokeType.name);
        }

        [Fact]
        public void ObjectInEvoChainTest()
        {
            string jsonString = "{\"id\": 7,\"chain\": {\"species\": {\"name\": \"rattata\",\"url\": \"https://pokeapi.co/api/v2/pokemon-species/19/\"},\"evolution_details\": null,\"evolves_to\": [{\"is_baby\": false,\"species\": {\"name\": \"raticate\"},\"evolves_to\": []}]}}";
            EvoChain evoChainTest = JsonToObject.jsonToObject<EvoChain>(jsonString);
            Assert.Equal("EvoChain", evoChainTest.GetType().Name);
            Assert.Equal("rattata", evoChainTest.chain.species.name);
            Assert.Equal("raticate", evoChainTest.chain.evolves_to[0].species.name);
        }

        [Fact]
        public void ObjectInPokemonSpecieTest()
        {
            string jsonString = "{\"flavor_text_entries\":[{\"flavor_text\": \"When several of\nthese POKéMON\ngather, their\felectricity could\nbuild and cause\nlightning storms.\",\"language\": {\"name\": \"en\",\"url\": \"https://pokeapi.co/api/v2/language/9/\"}},{\"flavor_text\": \"Il lui arrive de remettre d’aplomb\nun Pikachu allié en lui envoyant\nune décharge électrique.\",\"language\": {\"name\": \"fr\",\"url\": \"https://pokeapi.co/api/v2/language/5/\"}}],\"id\":413,\"name\":\"wormadam\",\"order\":441,\"gender_rate\":8,\"capture_rate\":45,\"evolution_chain\":{\"url\":\"https://pokeapi.co/api/v2/evolution-chain/213/\"}}";
            PokemonSpecies pokeSpecie = JsonToObject.jsonToObject<PokemonSpecies>(jsonString);
            Assert.Equal("PokemonSpecies", pokeSpecie.GetType().Name);
            Assert.Equal("https://pokeapi.co/api/v2/evolution-chain/213/", pokeSpecie.psEvoChain.evoChainUrl);
        }

        [Fact]
        public void ObjectInPokemonListTest()
        {
            string jsonString = "{\"pokemon_entries\": [{ \"entry_number\": 1,\"pokemon_species\": { \"name\": \"bulbasaur\",\"url\": \"https://pokeapi.co/api/v2/pokemon-species/1/\"} },{ \"entry_number\": 2,\"pokemon_species\": { \"name\": \"ivysaur\",\"url\": \"https://pokeapi.co/api/v2/pokemon-species/2/\"} },{ \"entry_number\": 3,\"pokemon_species\": { \"name\": \"venusaur\",\"url\": \"https://pokeapi.co/api/v2/pokemon-species/3/\" } }]}";
            PokemonList pokeList = JsonToObject.jsonToObject<PokemonList>(jsonString);
            Assert.Equal("PokemonList", pokeList.GetType().Name);
            Assert.Equal("bulbasaur", pokeList.pokemonEntries[0].pokemonEntrySpecie.name);
            Assert.Equal("ivysaur", pokeList.pokemonEntries[1].pokemonEntrySpecie.name); 
            Assert.Equal("venusaur", pokeList.pokemonEntries[2].pokemonEntrySpecie.name);
        }

        [Fact]
        public void GetPokemonDescriptionTest()
        {
            string jsonString = "{\"flavor_text_entries\":[{\"flavor_text\": \"When several of\nthese POKéMON\ngather, their\felectricity could\nbuild and cause\nlightning storms.\",\"language\": {\"name\": \"en\",\"url\": \"https://pokeapi.co/api/v2/language/9/\"}},{\"flavor_text\": \"Il lui arrive de remettre d’aplomb\nun Pikachu allié en lui envoyant\nune décharge électrique.\",\"language\": {\"name\": \"fr\",\"url\": \"https://pokeapi.co/api/v2/language/5/\"}}],\"id\":413,\"name\":\"wormadam\",\"order\":441,\"gender_rate\":8,\"capture_rate\":45,\"evolution_chain\":{\"url\":\"https://pokeapi.co/api/v2/evolution-chain/213/\"}}";
            PokemonSpecies pokeSpecie = JsonToObject.jsonToObject<PokemonSpecies>(jsonString);
            string description = GetData.GetPokemonDescription(pokeSpecie.descriptionsListe);
            Assert.Equal("Il lui arrive de remettre d’aplomb\nun Pikachu allié en lui envoyant\nune décharge électrique.", description);
        }
    }
}
