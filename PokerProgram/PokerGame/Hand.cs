using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerGame
{
    public class Hand
    {
        public List<Card> hand;

        public Hand()
        {
            hand = new List<Card>();            
        }

        public void AddCard(Card c)
        {
            hand.Add(c);         
        }

        public void AddCards(List<Card> cards)
        {
            foreach ( Card c in cards)
            {
                AddCard(c);
            }
        }
        public double CalculateOdds()
        {
            return 0.0;   
        }

        public override string ToString()
        {
            string response = "";            
            foreach (Card c in hand)
            {
                response += c.ToString() + " ";
            }
            return response;
        }

        public int HandValue()
        {
            int score = 0;

            if (IsRoyalFlush(hand))
                score += 1000;
            else if (IsStraightFlush(hand))
                score += 900;
            else if (IsFourOfAKind(hand))
                score += 800;
            else if (IsFullHouse(hand))
                score += 700;
            else if (IsFlush(hand))
                score += 600;
            else if (IsStraight(hand))
                score += 500;
            else if (IsThreeOfAKind(hand))
                score += 400;
            else if (IsTwoPair(hand))
                score += 300;
            else if (IsPair(hand))
                score += 200;

            foreach (Card c in hand)
            {                
                score += c.GetValue();
            }

            return score;
        }       

        private static bool IsStraight(List<Card> hand)
        {
            if (hand[0].Type.Equals("2") && hand[1].Type.Equals("3") && hand[2].Type.Equals("4") && hand[3].Type.Equals("5") && hand[4].Type.Equals("A"))
                return true;            
            int seriescounter = 0;
            hand.Sort();
            for (int i = 1; i < hand.Count; i++)
            {
                if (hand[i].GetValue() - hand[i - 1].GetValue() == 1)
                {
                    seriescounter++;
                }                
            }            
            if (seriescounter == 4)
                return true;

            return false;
        }

        private static bool IsPair(List<Card> hand)
        {            
            for (int i = 1; i < hand.Count; i++)
            {
                if (hand[i].GetValue() - hand[i - 1].GetValue() == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsTwoPair(List<Card> hand)
        {                        
            int counter = 0;
            for (int i = 1; i < hand.Count; i++)
            {
                if (hand[i].GetValue() - hand[i - 1].GetValue() == 0)
                {
                    counter++;
                    i++;
                }                
            }
            return counter == 2;
        }

        public static bool IsThreeOfAKind(List<Card> hand)
        {
            int counter = 0;
            for (int i = 1; i < hand.Count; i++)
            {
                if (counter == 2)
                    return true;
                if ( hand[i].GetValue() - hand[i - 1].GetValue() == 0 )
                {
                    counter++;
                }
                else if (hand[i].GetValue() - hand[i - 1].GetValue() != 0)
                {
                    counter = 0;
                }
                if (counter == 2)
                    return true; 
            }
            return false;
        }

        public static bool IsFourOfAKind(List<Card> hand)
        {
            int counter = 0;
            for (int i = 1; i < hand.Count; i++)
            {
                if (counter == 3)
                   return true;            
                if (hand[i].GetValue() - hand[i - 1].GetValue() == 0)
                {
                    counter++;
                }
                else if (hand[i].GetValue() - hand[i - 1].GetValue() != 0)
                {
                    counter = 0;
                }
                if (counter == 3)
                    return true;        
            }        
            return false;
        }

        public static bool IsFullHouse(List<Card> hand)
        {
            int pair = 0;
            int three = 0;
            int counter = 0;
            for (int i = 1; i < hand.Count; i++)
            {
                if (hand[i].GetValue() - hand[i - 1].GetValue() == 0)
                {
                    counter++;
                }
                if (hand[i].GetValue() - hand[i - 1].GetValue() != 0)
                {
                    if (counter == 2)
                        three = 1;
                    if (counter == 1)
                        pair = 1;
                    counter = 0;                    
                }                
            }
            return ((pair == 1 && counter == 2) || (counter == 1 && three == 1));
        }

        public static bool IsFlush(List<Card> hand)
        {
            for (int i = 1; i < hand.Count; i++)
            {
                if (hand[i].Suit != hand[i - 1].Suit)
                    return false;                                   
            }
            return true;
        }

        public static bool IsStraightFlush(List<Card> hand)
        {
            return IsStraight(hand) && IsFlush(hand);
        }

        public static bool IsRoyalFlush(List<Card> hand)
        {
            return IsStraight(hand) && IsFlush(hand) && hand[3].Type == "K" && hand[4].Type == "A";
        }
    
    }
}
