using System.Collections.Generic;

// Classe finale accueillant les données.

namespace PokemonLibrary
{
    public class Pokemon
    {
        public string Numero { get; set; }

        public string Nom { get; set; }

        public List<string> Types { get; set; }

        public List<string> Evolutions { get; set; }

        public string Description { get; set; }

        public bool PokemonIsOk { get; set; }
    }
}
