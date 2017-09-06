using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGame.GameLogic;

namespace WarGame
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to WAR!");
            Console.WriteLine("Enter Your Name:");
            string player1 = Console.ReadLine();
            Console.WriteLine("Enter opponent name");
            string player2 = Console.ReadLine();
            War game = new War(player1, player2);
            game.GameProgressed += GameProgressed;
            game.DealCards();
            Console.WriteLine("Hello {0}, Press Enter to progress. Press 'q' to quit any time", player1);
            string input = Console.ReadLine();
            bool gameEnded = false;
            while (input.ToLower() != "q" && !gameEnded) 
            {
                
                gameEnded = game.Play();
                if(!gameEnded)
                {
                    input = Console.ReadLine();
                }
                
            }
            
            Console.WriteLine("GoodBye!" + game.CurrentReport());
        }
        static void GameProgressed(object sender, EventArgs e)
        {
            Console.WriteLine(sender);

        }
    }
}
