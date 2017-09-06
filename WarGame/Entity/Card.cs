using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarGame.Entity
{
    enum Suit{
        Club, Diamond, Heart, Spade
    }
    enum Player
    {
        Invalid, Player1, Player2
    }
    class Card : IComparable<Card>
    {
        private Suit suit;
        private int faceValue; // 2..10 as value, 11 for J, 12 for Q, 13 for K, 14 for A
        private Player owner;

        public int FaceValue { get => faceValue; set => faceValue = value; }        
        internal Suit Suit { get => suit; set => suit = value; }
        internal Player Owner { get => owner; set => owner = value; }

        public Card(Suit suit, int faceValue, Player owner)
        {
            this.suit = suit;
            this.faceValue = faceValue;
            this.owner = owner;
        }

        public int CompareTo(Card other)
        {
            return this.FaceValue - other.FaceValue;
        }
        public override string ToString()
        {
            switch (this.suit)
            {
                case Suit.Club:
                    return "\u2663 " + this.FaceValue;
                case Suit.Diamond:
                    return "\u2666 " + this.FaceValue;
                case Suit.Spade:
                    return "\u2660 " + this.FaceValue;
                case Suit.Heart:
                    return "\u2665 " + this.FaceValue;
                default:
                    return string.Empty;
            }
            
        }
    }
}
