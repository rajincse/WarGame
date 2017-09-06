using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGame.Entity;


namespace WarGame.GameLogic
{
    /// <summary>
    /// The main class for conducting the game
    /// </summary>
    public class War
    {
        private string player1Name;
        private string player2Name;
        private Queue<Card> player1Cards;
        private Queue<Card> player2Cards;

        public Queue<Card> Player1Cards { get => player1Cards; }
        public Queue<Card> Player2Cards { get => player2Cards ; }

        public event EventHandler GameProgressed;// delegate for printing
        public War(string player1Name, string player2Name)
        {
            this.player1Name = player1Name;
            this.player2Name = player2Name;
        }

        /// <summary>
        /// Shuffles all the cards and distributes to the players
        /// </summary>
        public void DealCards()
        {
            List<Card> deck = new List<Card>();
            // dealing decks
            for(Suit s=Suit.Club;s<= Suit.Spade;s++)
            {
                for(int faceval = 2; faceval < 15; faceval++)
                {
                    deck.Add(new Card(s, faceval));
                }
            }
            
            Random random = new Random();
            int halfSize = deck.Count / 2;
            //shuffling
            deck =deck.OrderBy<Card, int>(card => random.Next()).ToList<Card>();
            //distributing to the players
            player1Cards = new Queue<Card>(deck.GetRange(0, halfSize));
            player2Cards = new Queue<Card>(deck.GetRange(halfSize, halfSize));
        }
        /// <summary>
        /// Plays one step of play. E.g. dealing one card or handling war
        /// </summary>
        /// <returns>True if the game is ended else false</returns>
        public bool Play()
        {
            StringBuilder progress = new StringBuilder(); // string container for progress message
            if (player1Cards.Count==0 || player2Cards.Count==0) // check whether the game has ended
            {
                progress.Append(CurrentReport());
                GameReport(progress.ToString());
                return true;
            }
            
            Queue<Card> onTheTable = new Queue<Card>();// container for cards on the table
            //Deal one card from each player
            Card player1Card = player1Cards.Dequeue();
            Card player2Card = player2Cards.Dequeue();
            onTheTable.Enqueue(player1Card);
            onTheTable.Enqueue(player2Card);
            //progress
            progress.Append(player1Card.ToString()+"("+this.player1Name+")");
            progress.Append(" ");
            progress.Append(player2Card.ToString() + "(" + this.player2Name + ")");
            progress.Append("\r\n");
            //Handle war
            while (player1Card.CompareTo(player2Card) ==0)
            {
                //War!                
                progress.Append("WAR!!!\r\n");
                if (player1Cards.Count == 0 || player2Cards.Count == 0) // Check whether any player ran out of cards
                {
                    progress.Append(CurrentReport());
                    GameReport(progress.ToString());
                    return true;
                }
                //facedown put single card face down
                progress.Append("Facedown");
                progress.Append("\r\n");
                progress.Append("_ _");
                progress.Append("\r\n");
                onTheTable.Enqueue(player1Cards.Dequeue());
                onTheTable.Enqueue(player2Cards.Dequeue());
                //faceup
                if (player1Cards.Count == 0 || player2Cards.Count == 0) // check again
                {
                    progress.Append(CurrentReport());
                    GameReport(progress.ToString());
                    return true;
                }
                player1Card = player1Cards.Dequeue();
                player2Card = player2Cards.Dequeue();
                onTheTable.Enqueue(player1Card);
                onTheTable.Enqueue(player2Card);
                progress.Append("FaceUp");
                progress.Append("\r\n");
                progress.Append(player1Card.ToString() + "(" + this.player1Name + ")");
                progress.Append(" ");
                progress.Append(player2Card.ToString() + "(" + this.player2Name + ")");
                progress.Append("\r\n");
            }
            //Actions for any player wins
            if (player1Card.CompareTo(player2Card)>0)
            {
                // player1 wins, put all cards on the table to player1
                progress.Append(this.player1Name+" Wins");
                progress.Append("\r\n");
                
                foreach (Card card in onTheTable)
                {
                    player1Cards.Enqueue(card);
                }
            }
            else
            {
                // player2 wins, put all cards on the table to player2
                progress.Append(this.player2Name + " Wins");
                progress.Append("\r\n");
                foreach (Card card in onTheTable)
                {
                    player2Cards.Enqueue(card);
                }
            }
            //Report to handler
            progress.Append(CurrentReport());
            GameReport(progress.ToString());
            return false;
        }
        /// <summary>
        /// Delegate to print progress report
        /// </summary>
        /// <param name="progressReport"></param>
        protected void GameReport(string progressReport)
        {
            GameProgressed?.Invoke(progressReport, EventArgs.Empty);
        }
        /// <summary>
        /// Current report- How much cards each players have
        /// </summary>
        /// <returns>Summary Report</returns>
        public string CurrentReport()
        {
            if(player1Cards.Count==0)
            {
                return this.player2Name + " Wins the game";
            }
            else if(player2Cards.Count==0)
            {
                return this.player1Name + " Wins the game";
            }
            else
            {
                return this.player1Name + ":" + player1Cards.Count + ", " + this.player2Name + ":" + player2Cards.Count + "\r\n";
            }
        }
    }
}
