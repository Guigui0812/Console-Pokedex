using System.Text.Json.Serialization;

namespace PokemonLibrary
{
    public class Type
    {
        [JsonPropertyName("name")]
        public string name { get; set; }
    }
}
