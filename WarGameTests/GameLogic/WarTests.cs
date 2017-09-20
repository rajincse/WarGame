using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarGame.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGame.Entity;

namespace WarGame.GameLogic.Tests
{
    [TestClass()]
    public class WarTests
    {
        [TestMethod()]
        public void DealCardsTest()
        {
            War game = new War("Player1", "Player2");
            game.DealCards();
            List<Card> deck = new List<Card>();
            // dealing decks
            for (Suit s = Suit.Club; s <= Suit.Spade; s++)
            {
                for (int faceval = 2; faceval < 15; faceval++)
                {
                    deck.Add(new Card(s, faceval));
                }
            }
            int halfSize = deck.Count / 2;
            Queue<Card> q1 = new Queue<Card>(deck.GetRange(0, halfSize));
            Queue<Card> q2 = new Queue<Card>(deck.GetRange(halfSize, halfSize));
            CollectionAssert.AreNotEqual(q1, game.Player1Cards, "Player1 Cards should be randomized");
            CollectionAssert.AreNotEqual(q2, game.Player2Cards, "Player2 Cards should be randomized");
        }

        
    }
}