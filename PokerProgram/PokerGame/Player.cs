using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtificialIntelligence;
namespace PokerGame
{
    public class Player
    {
        private double money;
        public List<Card> CardSet;
        private string PlayerName;
        private NeuralNetwork intelligence;

        public Player(double startingMoney, string pname)
        {
            CardSet = new List<Card>();
            money = startingMoney;
            PlayerName = pname;           
        }

        public NeuralNetwork Intelligence
        {
            get { return intelligence; }
            set { intelligence = value; }
        }

        public double Money 
        {
            get { return money;}
            set { money = value; }
        }

        public Hand BestHand()
        {
            if (CardSet.Count != 7) 
                return null;
            CardSet.Sort();
            //Create Many Combination Sets of the Cards
            List<Hand> PotentialHandCombos = new List<Hand>();
            for (int i = 0; i < 21; i++)
            {
                int x = 0;
                int y = 5;

                Hand h = new Hand();
                foreach (Card c in CardSet)
                {
                  h.AddCard(c);
                }

                h.hand.RemoveAt(x);
                h.hand.RemoveAt(y);
                y--;
                if (y == i)
                {
                    x++;
                    y = 5;
                }
                PotentialHandCombos.Add(h);
            }
            int bestscore = 0;
            Hand bestHand = null;
            foreach (Hand h in PotentialHandCombos)
            {
                if (h.HandValue() > bestscore)
                    bestHand = h;
            }
            return bestHand;
        }

        public override string ToString()
        {
            string s = "{";
            foreach (Card c in CardSet)
            {
                s += c.ToString() + " ";
            }
            return s + "}";
        }

        public double Decision(List<double> parameters)
        {
            intelligence.EvaluateNetwork(parameters);
            IList<double> outputs = intelligence.GetOutputs();

            if (outputs[0] < 0)
            {
                //Player Has Folded
            }
            else if (outputs[0] <= 1.0)
            {
                //Player Has Called
            }
            else if (outputs[0] > 1.0)
            {
                //Player Has Raised
            }


            return outputs[0];
            // Todo : Need to validate decision process
        }
    }  
}
