using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGame.GameLogic;

namespace WarGame
{
    /// <summary>
    /// Author: Sayeed S. Alam, Ph.D.
    /// Email: salam011@cis.fiu.edu
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to WAR!"); //Welcome message
            Console.WriteLine("Enter Your Name:");
            string player1 = Console.ReadLine();
            Console.WriteLine("Enter opponent name");
            string player2 = Console.ReadLine();
            War game = new War(player1, player2);
            game.GameProgressed += GameProgressed; // for printing using delegates
            game.DealCards();
            Console.WriteLine("Hello {0}, Press Enter to progress. Press 'q' to quit any time", player1);
            string input = Console.ReadLine();
            bool gameEnded = false;
            while (input.ToLower() != "q" && !gameEnded) //The game will end when pressing q or automatically
            {
                
                gameEnded = game.Play();
                if(!gameEnded)
                {
                    input = Console.ReadLine();
                }
                
            }
            
            Console.WriteLine("GoodBye!" + game.CurrentReport());
        }
        /// <summary>
        /// Delegate for printing
        /// </summary>
        /// <param name="sender">The progress string</param>
        /// <param name="e">Event Argument</param>
        static void GameProgressed(object sender, EventArgs e)
        {
            Console.WriteLine(sender);

        }
    }
}
