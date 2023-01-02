using System;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using PokemonLibrary;
using PokemonLibrary.EvolveChainClass;
using PokemonLibrary.PokemonListClass;
using PokemonLibrary.PokemonSpeciesClass;
using System.Collections.Generic;
using System.Linq;
using PokemonLibrary.PokemonTypesClass;

namespace GetPokedexData
{
    // Classe statique gérant la récupération des données depuis la PokeAPI.
    public static class GetData
    {
        private static readonly HttpClient httpClient = new HttpClient(); // Instanciation d'un seul HTTPClient pour éviter un épuisement du nombre de sockets disponibles.
        public static ObjectCache Cache { get;  } = MemoryCache.Default; // Instance du cache valable pour la totalité de la session à l'aise de MemoryCache.

        // Méthode permettant la récupération des données depuis l'API à l'aide d'une requête HTTP GET.
        public static async Task<string> GetDataFromAPIAsync(string requete, string infoToStock)
        {
            HttpResponseMessage response; // variable contenant la réponse de l'API.

            // Try - Catch permettant de gérer les erreurs liées aux requêtes HTTP.
            try
            {
                response = await httpClient.GetAsync(requete);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException) // Dans le cas où une requête survient, la méthode return "null", permettant d'identifier la survenance d'une erreur.
            {
                return null; 
                throw;
            }

            // Conversion de la réponse HTTP en chaine de caractère et placement de son contenu dans le cache de l'application. 
            // L'intérêt est d'éviter des appels successifs à l'API pour des données déjà collectées.
            string contentResponse = await response.Content.ReadAsStringAsync(); 
            CacheItem cacheData = new CacheItem(infoToStock, contentResponse); // La donnée est placée dans CacheItem, qui sera lui-même stocké dans le cache.
            CacheItemPolicy policy = new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(60) };
            Cache.Add(cacheData, policy);
            return contentResponse;
        }

        // Méthode permettant de récupérer des données en faisant appel au cache ou à HTTP GET.
        public static async Task<string> GetDataAsync(string url, string infoToDisplay)
        {
            // Récupération dans le cache si la donnée souhaitée y est disponible.
            // Dans le cas inverse, la donnée est récupérer depuis l'API.
            if (Cache.Contains(infoToDisplay))
            {
                string response = Cache.Get(infoToDisplay).ToString();
                return response;                
            }
            else
            {
                string httpResponse = await GetDataFromAPIAsync(url, infoToDisplay);
                return httpResponse;
            }
        }

        // Méthode chargée de récupérer et d'assembler les différentes informations relatives à un Pokemon.
        public static async Task<Pokemon> GetPokemonFromAPIAsync(string pokemonName)
        {
            string pokeToDisplay = pokemonName.ToLower(); // Permet de gérer les problèmes liés à la casse. 

            Pokemon myPokemon = new Pokemon();

            // Gestion du cas d'une recherche d'un pokemon incorrect.
            try
            {
                // Assemblage de l'objet Pokemon final qui sera envoyé à l'UI.
                PokemonSpecies PokemonSpecie = await GetData.GetPokemonSpeciesAsync(pokeToDisplay);
                myPokemon.Numero = PokemonSpecie.id;
                myPokemon.Nom = PokemonSpecie.name;
                myPokemon.Types = await GetData.GetPokemonTypesAsync(myPokemon.Numero);
                myPokemon.Evolutions = await GetData.GetEvoChainFromAPIAsync(PokemonSpecie, myPokemon.Numero);
                myPokemon.Description = GetPokemonDescription(PokemonSpecie.descriptionsListe);
                myPokemon.PokemonIsOk = true;
            }
            catch
            {
                // Si une erreur est détectée, un booléen à l'intérieur de la classe Pokemon signale l'erreur. 
                // L'UI pourra donc éviter d'afficher des données erronées.
                myPokemon.PokemonIsOk = false; 
            }

            return myPokemon;
        }

        // Méthode permettant la récupération des types d'un Pokemon.
        public static async Task<List<string>> GetPokemonTypesAsync(string pokeId)
        {
            string httpRequete = "https://pokeapi.co/api/v2/pokemon/" + pokeId + "/";
            string httpResponse = await GetDataAsync(httpRequete, pokeId + "_type"); // Récupération
            PokemonTypes DeserializedPokemonTypes = JsonToObject.jsonToObject<PokemonTypes>(httpResponse); // Conversion en objet.

            // Conversion en liste de chaine de caractères.
            List<string> types = new List<string>();

            foreach (Slot slot in DeserializedPokemonTypes.types)
            {
                types.Add(slot.PokeType.name);
            }

            return types;
        }

        // Méthode permettant la récupération et la transformation en objet de la catégorie PokemonSpecie relative à un pokemon.
        // Nécessaire à la récupération du nom, de l'id, de la chaîne d'évolution et de la descritpion.
        public static async Task<PokemonSpecies> GetPokemonSpeciesAsync(string pokeToDisplay)
        {
            string httpRequete = "https://pokeapi.co/api/v2/pokemon-species/" + pokeToDisplay + "/";
            string httpResponse = await GetDataAsync(httpRequete, pokeToDisplay + "_specie"); // Récupération
            PokemonSpecies DeserializedPokemonSpecies = JsonToObject.jsonToObject<PokemonSpecies>(httpResponse); // Conversion.
            return DeserializedPokemonSpecies;
        }

        // Méthode permettant la récupération des évolutions d'un Pokemon.
        public static async Task<List<string>> GetEvoChainFromAPIAsync(PokemonSpecies pokemonSpec, string pokeToDisplay)
        {
            string httpRequete = pokemonSpec.psEvoChain.evoChainUrl;
            string httpResponse = await GetDataAsync(httpRequete, pokeToDisplay+"_evochain"); // Récupération
            EvoChain DeserializedEvoChain = JsonToObject.jsonToObject<EvoChain>(httpResponse); // Conversion

            // Conversion en liste de chaine de caractères.

            List<string> Evolutions = new List<string>();

            Evolutions.Add(DeserializedEvoChain.chain.species.name);

            foreach (EvolvesTo test in DeserializedEvoChain.chain.evolves_to)
            {
                Evolutions.Add(test.species.name);

                foreach (EvolvesTo test2 in test.evolves_to)
                {
                    Evolutions.Add(test2.species.name);
                }
            }

            return Evolutions;
        }

        // Méthode permettant la récupération et la transformation en objet d'une liste d'un certain nombre de pokemons.
        public static async Task<PokemonList> GetPageofPokemonAsync(int offset, int limit)
        {
            PokemonList pokemonList = await GetData.GetListofPokeFromAPIAsync(); // récupération de tous les pokemon.

            // Gestion du cas où le nombre de pokemon restant à afficher en dernière page n'est pas à égal à la limite fixée par la solution d'affichage.
            if ((offset + limit) > pokemonList.nbMaxPokemon) // Si le nombre maximum de pokemon est dépassé, on recalcule la limite pour qu'elle corresponde au nombre réel.
            {
                int newlimit = limit - ((offset + limit) - pokemonList.nbMaxPokemon);
                limit = newlimit;
            }

            // Constitution d'une liste permettant de retourner un certain nombre de pokemon.

            List<PokemonEntries> pokemonToDisplay = new List<PokemonEntries>();
          
            for (int i = offset; i < offset + limit; i++)
            {
                pokemonToDisplay.Add(pokemonList.pokemonEntries[i]);
            }

            pokemonList.pokemonEntries = pokemonToDisplay;
           
            return pokemonList;
        }

        // Méthode permettant la récupération de l'ensemble des pokemon.
        public static async Task<PokemonList> GetListofPokeFromAPIAsync()
        {
            string httpRequete = "https://pokeapi.co/api/v2/pokedex/1/";
            string httpResponse = await GetDataAsync(httpRequete, "poke_page"); // Récupération

            PokemonList DeserializedPokemonList = JsonToObject.jsonToObject<PokemonList>(httpResponse); // Conversion
            DeserializedPokemonList.nbMaxPokemon = DeserializedPokemonList.pokemonEntries.Count;
            return DeserializedPokemonList;
        }

        // Méthode permettant la récupération de la description d'un pokemon.
        public static string GetPokemonDescription(List<Description> descriptList)
        {
            string description;

            // Récupération d'une description. Dans le cas où elle n'existerait pas, un message d'erreur la remplacera. 
            try
            {
                description = descriptList.First(pokeDesc => pokeDesc.Language.languageName == "fr").descriptionString; // Linq permettant de récupérer une description
                return description;
            }
            catch
            {
                description = "La description de ce Pokemon n'est pas disponible";
                return description;
            }        
        }
    }
}