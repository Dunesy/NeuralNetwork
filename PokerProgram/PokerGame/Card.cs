using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerGame
{
    public class Card : IComparable
    {

        public static string[] TypeSet = {"2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A"};
        public static string[] SuitSet = { "S", "C", "H", "D" };

        private string type;
        private string suit;
        private string imagePath;

        public Card(string t, string s, string ip)
        {
            type = t;
            suit = s;
            imagePath = ip;
        }

        public Card()
        {
            type = "";
            suit = "";
            imagePath = "";
        }
        //Properties
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Suit
        {
            get { return suit; }
            set { suit = value; }
        }

        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }

        //Basic Functionality
        public override String ToString()
        {
            return Type + Suit;
        }
        public int CompareTo(Object obj)
        {
            Card card = obj as Card;
            int position = GetValue();
            int position2 = card.GetValue();
            return position.CompareTo(position2);
        }
        public int GetValue()
        {
            for (int i = 0; i < TypeSet.Length; i++)
            {
                if (Type.Equals(TypeSet[i]))
                    return i + 1;
            }
            return 0;
        }

    }

}