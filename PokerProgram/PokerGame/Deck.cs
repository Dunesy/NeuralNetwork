using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerGame
{
    public class Deck
    {
        public static Random rand = new Random();
        public List<Card> deck;
        public static int SHUFFLE_COUNT = 1000;

        public Deck()
        {
            deck = new List<Card>();
        }

        //Creates A Deck - This is the Default, Should be Modifed for the File Path
        public void CreateDeck()
        {
            deck.Clear();   
            foreach (string type in Card.TypeSet)
            {
                foreach (string suit in Card.SuitSet)
                {
                    deck.Add(new Card(type, suit, " "));
                }
            }
            Console.WriteLine("Deck Created");
        }

        public void FlushDeck()
        {
            deck.Clear();
            Console.WriteLine("Deck Flushed");
        }

        public void ShuffleDeck()
        {
            for (int i = 0; i < SHUFFLE_COUNT; i++)
            {
                int position1 = rand.Next(deck.Count);               
                int position2 = rand.Next(deck.Count);
                Card placeholder = deck[position1];
                deck[position1] = deck[position2];
                deck[position2] = placeholder;
            }
            Console.WriteLine("Deck Shuffled");
        }

        public Card DealOut()
        {
            if (deck.Count > 0)
            {
                Card card = deck[0];
                deck.RemoveAt(0);
                Console.WriteLine("Card Dealt Out " + card.ToString());
                return card;
            }
            else
            {
                Console.WriteLine("Unable to Deal Out");
                return null;                
            }
        }

        public override string ToString()
        {
            string response = "";
            foreach (Card c in deck)
            {
                response += c.ToString() + " ";
            }
            return response;
        }       
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
    }
}
