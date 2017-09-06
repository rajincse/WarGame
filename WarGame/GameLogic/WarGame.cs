using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGame.Entity;


namespace WarGame.GameLogic
{
    
    class War
    {
        private string player1Name;
        private string player2Name;
        private Queue<Card> player1Cards;
        private Queue<Card> player2Cards;
        public event EventHandler GameProgressed;
        public War(string player1Name, string player2Name)
        {
            this.player1Name = player1Name;
            this.player2Name = player2Name;
        }

        public void DealCards()
        {
            List<Card> deck = new List<Card>();
            // dealing decks
            for(Suit s=Suit.Club;s<= Suit.Spade;s++)
            {
                for(int faceval = 2; faceval < 15; faceval++)
                {
                    deck.Add(new Card(s, faceval, Player.Invalid));
                }
            }
            Random random = new Random();
            int halfSize = deck.Count / 2;
            deck =deck.OrderBy<Card, int>(card => random.Next()).ToList<Card>();
            player1Cards = new Queue<Card>(deck.GetRange(0, halfSize));
            player2Cards = new Queue<Card>(deck.GetRange(halfSize, halfSize));
        }
        public bool Play()
        {
            StringBuilder progress = new StringBuilder();
            if (player1Cards.Count==0 || player2Cards.Count==0)
            {
                progress.Append(CurrentReport());
                GameReport(progress.ToString());
                return true;
            }
            
            Queue<Card> onTheTable = new Queue<Card>();
            Card player1Card = player1Cards.Dequeue();
            Card player2Card = player2Cards.Dequeue();
            onTheTable.Enqueue(player1Card);
            onTheTable.Enqueue(player2Card);
            //progress
            progress.Append(player1Card.ToString()+"("+this.player1Name+")");
            progress.Append(" ");
            progress.Append(player2Card.ToString() + "(" + this.player2Name + ")");
            progress.Append("\r\n");
            while (player1Card.CompareTo(player2Card) ==0)
            {
                //War!                
                progress.Append("WAR!!!\r\n");
                if (player1Cards.Count == 0 || player2Cards.Count == 0)
                {
                    progress.Append(CurrentReport());
                    GameReport(progress.ToString());
                    return true;
                }
                //facedown
                progress.Append("Facedown");
                progress.Append("\r\n");
                progress.Append("_ _");
                progress.Append("\r\n");
                onTheTable.Enqueue(player1Cards.Dequeue());
                onTheTable.Enqueue(player2Cards.Dequeue());
                //faceup
                if (player1Cards.Count == 0 || player2Cards.Count == 0)
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

            if (player1Card.CompareTo(player2Card)>0)
            {
                // player1 wins
                progress.Append(this.player1Name+" Wins");
                progress.Append("\r\n");
                
                foreach (Card card in onTheTable)
                {
                    player1Cards.Enqueue(card);
                }
            }
            else
            {
                //player 2 wins
                progress.Append(this.player2Name + " Wins");
                progress.Append("\r\n");
                foreach (Card card in onTheTable)
                {
                    player2Cards.Enqueue(card);
                }
            }
            progress.Append(CurrentReport());
            GameReport(progress.ToString());
            return false;
        }
        protected void GameReport(string progressReport)
        {
            GameProgressed?.Invoke(progressReport, EventArgs.Empty);
        }
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
