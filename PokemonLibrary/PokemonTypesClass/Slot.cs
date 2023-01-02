using System.Text.Json;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace PokemonLibrary
{
    public class Slot
    {

        [JsonProperty("type")]
        public Type PokeType { get; set; }
    }
}
