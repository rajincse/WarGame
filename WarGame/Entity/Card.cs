using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarGame.Entity
{
    public enum Suit{
        Club, Diamond, Heart, Spade
    }
    public class Card : IComparable<Card>
    {
        private Suit suit;
        private int faceValue; // 2..10 as value, 11 for J, 12 for Q, 13 for K, 14 for A
        

        public int FaceValue { get => faceValue; set => faceValue = value; }        
        internal Suit Suit { get => suit; set => suit = value; }
        

        public Card(Suit suit, int faceValue)
        {
            this.suit = suit;
            this.faceValue = faceValue;
        }

        public int CompareTo(Card other)
        {
            return this.FaceValue - other.FaceValue;
        }
        /// <summary>
        /// For printing
        /// </summary>
        /// <returns>Symbol of Suit and Facevalue</returns>
        public override string ToString()
        {
            string faceValueName = string.Empty;
            switch(FaceValue)
            {
                case 11:
                    faceValueName = "J";
                    break;
                case 12:
                    faceValueName = "Q";
                    break;
                case 13:
                    faceValueName = "K";
                    break;
                case 14:
                    faceValueName = "A";
                    break;
                default:
                    faceValueName = "" + FaceValue;
                    break;
            }
            switch (this.suit)
            {
                case Suit.Club:
                    return "\u2663 " + faceValueName;
                case Suit.Diamond:
                    return "\u2666 " + faceValueName;
                case Suit.Spade:
                    return "\u2660 " + faceValueName;
                case Suit.Heart:
                    return "\u2665 " + faceValueName;
                default:
                    return string.Empty;
            }
            
        }
        /// <summary>
        ///  Check whether the suit and facevalue matches
        /// </summary>
        public override bool Equals(object obj)
        {
            if( obj is Card)
            {
                Card other = (Card)obj;
                return this.suit == other.suit && this.faceValue == other.faceValue;
            }
            else
            {

            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
