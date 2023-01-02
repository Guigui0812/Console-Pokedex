using GetPokedexData;
using PokemonLibrary;
using PokemonLibrary.PokemonListClass;
using System;
using System.Threading.Tasks;

namespace PokedexProjectUI
{
    public static class UserInterface
    {
        public static async Task SearchAnElement() // Fonction de recherche d'un élément.
        {
            Console.Write("\nSaisissez le nom ou le numéro du pokemon que vous voulez rechercher : ");
            string element = Console.ReadLine();

            if (!String.IsNullOrEmpty(element)) // Gestion du cas d'une entrée utilisateur vide.
            {
                await DisplayElementInfo(element);
            }
            else
            {
                Console.WriteLine("La valeur saisie est incorrecte, réessayez.");
            }

            Console.ReadLine();
        }

        public static async Task DisplayElementInfo(string element)
        {
            // Affichage infos de base : 

            Console.Clear();

            Pokemon PokeObtnd = await GetData.GetPokemonFromAPIAsync(element); // récupération d'un élément.

            // Gestion du cas d'un élément incorrect.
            if (PokeObtnd.PokemonIsOk)
            {
                Console.WriteLine("Présentation du " + PokeObtnd.GetType().Name + " :\n- Nom : " + PokeObtnd.Nom + "\n- Numéro : " + PokeObtnd.Numero + "\n");
                Console.WriteLine("Type(s) :");

                foreach (string type in PokeObtnd.Types)
                {
                    Console.WriteLine("- " + type);
                }

                // Affichage de la description de l'élément :
                Console.WriteLine("\nDescription :");
                Console.WriteLine(PokeObtnd.Description);

                // Affichage de la chaîne d'évolution : 

                Console.WriteLine("\nChaine d'évolution de ce Pokemon :");

                foreach (string evo in PokeObtnd.Evolutions)
                {
                    Console.WriteLine("- " + evo);
                }

                Console.WriteLine("\nCliquez sur ENTRER pour quitter l'affichage");

            }
            else
            {
                Console.WriteLine("Erreur lors de la récupération, les informations souhaitées n'existent pas ou ne sont pas accessibles");
            }
        }

        // Listing des pokemon
        public static async Task DiplayListOfElement()
        {
            string nav;
            int pokeNb = 1, offset = 0;
            bool displayList = true; // booléen d'affichage de la liste.
            const int limitOfElementDisplay = 20; // Nombre d'éléments que l'utilisateur verra à listés à l'écran.

            // Boucle d'affichage de la liste.
            while (displayList)
            {
                Console.Clear();
                bool correctEvnt = false; // booléen validant la survenue d'un événement.

                PokemonList pokemonList = await GetData.GetPageofPokemonAsync(offset, limitOfElementDisplay); // Récupération des éléments à lister.

                Console.WriteLine("Liste des pokemon :");

                foreach (PokemonEntries res in pokemonList.pokemonEntries)
                {
                    Console.WriteLine("- " + pokeNb.ToString() + " : " + res.pokemonEntrySpecie.name);
                    pokeNb++;
                }

                Console.WriteLine("\nD pour suivant, Q pour précédent, E pour quitter, ou choisir un pokemon entre 1 et 20 pour afficher ses infos :");

                // Boucle événementielle
                while (!correctEvnt)
                {
                    nav = Console.ReadLine();

                    bool success = Int32.TryParse(nav, out int tmpNav); // Traduction des caractères saisis en entier.

                    // Sélection d'un élément en liste et vérification des bornes.
                    if (success && (tmpNav <= pokemonList.pokemonEntries.Count && tmpNav > 0))
                    {
                        await DisplayElementInfo(pokemonList.pokemonEntries[tmpNav - 1].entryNumber); // Affichage des détails de l'élément.
                        correctEvnt = true; 
                        Console.ReadLine(); 
                    }
                    else
                    {
                        // Evénement permettant d'afficher la page suivante.
                        if (nav == "D" || nav == "d")
                        {
                            if (offset <= (pokemonList.nbMaxPokemon - limitOfElementDisplay)) // Gestion du cas de fin de liste.
                            {
                                offset = offset + limitOfElementDisplay;
                                correctEvnt = true;
                            }                   
                        }
                        else if (nav == "Q" || nav == "q") // Evénement permettant d'afficher la page précédente.
                        {
                            if (offset > 0) // Gestion du cas de début de liste.
                            {
                                offset = offset - limitOfElementDisplay;
                                correctEvnt = true;
                            }
                            
                        }
                        else if (nav == "E" || nav == "e") // Evenement permettant de quitter le listing.
                        {
                            correctEvnt = true;
                            displayList = false;
                        }
                    }
                }

                pokeNb = 1;                
            }
        }

        // Menu de l'application Pokédex.
        public static async Task DisplayMenu()
        {
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("| Bienvenue dans le Pokédex conçu par Guillaume Rohee et Gautier LeBouquin |");
            Console.WriteLine("----------------------------------------------------------------------------\n");
            bool run = true;

            while(run)
            {
                Console.Write("Choisir la fonctionnalité : \n - 1 : Rechercher un Pokemon \n - 2 : Lister les pokemons du pokedex\n\nQuittez le programme en cliquant sur E.\n\nVotre choix : ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    await UserInterface.SearchAnElement();
                }
                else if (choice == "2")
                {
                    await UserInterface.DiplayListOfElement();
                }
                else if (choice == "E" || choice == "e")
                {
                    run = false;
                }
                else
                {
                    Console.WriteLine("La valeur saisie n'est pas correcte, réessayez.\n");
                }

                Console.Clear();
            }
        }
    }
}