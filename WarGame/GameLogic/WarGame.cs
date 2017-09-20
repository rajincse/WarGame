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
    /// Enum for Game Statuses
    /// </summary>
    public enum GameStatus
    {
        Default, IsOver,Player1Win, Player2Win,FaceDownCards, War
    }
    /// <summary>
    /// The main class for conducting the game
    /// </summary>
    public class War
    {
        private string player1Name;
        private string player2Name;
        private Queue<Card> player1Cards;
        private Queue<Card> player2Cards;
        private Queue<Card> onTheTableCards;// container for cards on the table

        public Queue<Card> Player1Cards { get => player1Cards; }
        public Queue<Card> Player2Cards { get => player2Cards ; }

        public event EventHandler GameProgressed;// delegate for printing
        public War(string player1Name, string player2Name)
        {
            this.player1Name = player1Name;
            this.player2Name = player2Name;
            this.onTheTableCards = new Queue<Card>();
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
        /// <returns>Returns the game status ( E.g. isOver, Who won recently or War)</returns>
        public GameStatus Play()
        {
            GameStatus status = GameStatus.Default;
            if (IsPlayable()) // Checks wheather both players have atleast one card
            {
                status = CheckCard(); //Check status of the next cards 
                ReportCardStatus(); //Print card status

                PutCardsOnTheTable(); // Put a single card to the table
                ReportProgress(status);//Print result in the current step

                
                switch (status)
                {
                    case GameStatus.Player1Win:
                        Reward(player1Cards); // Put all cards on the table to Player1
                        break;
                    case GameStatus.Player2Win:
                        Reward(player2Cards);// Put all cards on the table to Player2
                        break;
                    case GameStatus.War:
                        HandleWar();
                        break;
                }
            }
            else
            {
                status = GameStatus.IsOver;
            }
            if(status != GameStatus.War) // For war the Play method is already called recursively
            {
                ReportProgress();
            }
            
            return status;
        }
        /// <summary>
        /// Handle war by putting a card facedown and play again
        /// </summary>
        private void HandleWar()
        {
            //faceup cards
            if(IsPlayable()) //check wheather putting facedown is possible
            {  
                PutCardsOnTheTable();
                ReportProgress(GameStatus.FaceDownCards); // Special print for facedown cards
            }
            //Play Again!
            Play();
            
        }
        /// <summary>
        /// Put a single card from each player on the table
        /// </summary>
        private void PutCardsOnTheTable()
        {
            this.onTheTableCards.Enqueue(Player1Cards.Dequeue());
            this.onTheTableCards.Enqueue(Player2Cards.Dequeue());
        }

        /// <summary>
        /// Give all the cards to the winner 
        /// </summary>
        /// <param name="winner">The winner Card queue</param>
        private void Reward(Queue<Card> winner)
        {
            while(this.onTheTableCards.Count !=0)
            {
                Card card = onTheTableCards.Dequeue();
                winner.Enqueue(card);
            }
        }
        /// <summary>
        /// Checks the next card from each player and determines the result in the current step
        /// </summary>
        /// <returns>Game status (e.g. War or any player win)</returns>
        public GameStatus CheckCard()
        {
            //Play a single card
            Card player1Card = player1Cards.Peek();
            Card player2Card = player2Cards.Peek();
            GameStatus status = GameStatus.Default;
            if(player1Card.CompareTo(player2Card)> 0) // Player1 card is bigger
            {
                status = GameStatus.Player1Win;
            }
            else if (player1Card.CompareTo(player2Card) < 0)// Player2 card is bigger
            {
                status = GameStatus.Player2Win;
            }
            else // War. Both cards have same value. 
            {
                status = GameStatus.War;
            }

            return status;
        }
        /// <summary>
        /// Checks whether one of the player is out of cards
        /// </summary>
        /// <returns> True if both of the players have atleast one card, otherwise False</returns>
        public bool IsPlayable()
        {
            return player1Cards.Count != 0 && player2Cards.Count != 0;
        }
        /// <summary>
        /// Invokes print to the handler based on the current card situation (e.g. Either any player win or War)
        /// </summary>
        private void ReportCardStatus()
        {
            StringBuilder progress = new StringBuilder(); // string container for progress message
            Card player1Card = player1Cards.Peek();
            Card player2Card = player2Cards.Peek();
            progress.Append(player1Card.ToString() + "(" + this.player1Name + ")");
            progress.Append(" ");
            progress.Append(player2Card.ToString() + "(" + this.player2Name + ")");
            progress.Append("\r\n");
            GameReport(progress.ToString());
        }
        /// <summary>
        /// Invokes print the current report to the handler
        /// </summary>
        private void ReportProgress()
        {
            GameReport(CurrentReport());
        }
        /// <summary>
        /// Invokes print based on the current game situation 
        /// </summary>
        /// <param name="status"></param>
        private void ReportProgress(GameStatus status)
        {
            StringBuilder progress = new StringBuilder(); // string container for progress message
            switch (status)
            {
                case GameStatus.Player1Win:
                    progress.Append(this.player1Name + " Wins");
                    progress.Append("\r\n");
                    break;
                case GameStatus.Player2Win:
                    progress.Append(this.player2Name + " Wins");
                    progress.Append("\r\n");
                    break;
                case GameStatus.War:
                    progress.Append("WAR!!!\r\n");
                    break;
                case GameStatus.FaceDownCards:
                    progress.Append("Facedown");
                    progress.Append("\r\n");
                    progress.Append("_ _");
                    progress.Append("\r\n");
                    break;
            }
            GameReport(progress.ToString());

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
