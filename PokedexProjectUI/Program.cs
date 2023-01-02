using System;
using System.Threading.Tasks;

namespace PokedexProjectUI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await UserInterface.DisplayMenu();
        }
    }
}