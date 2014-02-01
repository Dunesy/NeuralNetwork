using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerGame
{
    public class Game
    {
        
        private List<Card> tableCards;
        private Deck pokerDeck;
        private List<Player> players;
        public enum State { Dealout, Run, TurnOver, River } 
        public int CurrentState = 0;

        private State currentState;
        private double pot;
        private double smallBlind, largeBlind;
        
        public Game()
        {
            players = new List<Player>();
            tableCards = new List<Card>();
            pokerDeck = new Deck();
            pokerDeck.CreateDeck();
            pot = 0.0;
            smallBlind = 0.0;
            largeBlind = 0.0;            
        }

        public List<Player> Players
        {
            get { return players; }
        }

        public void NewRound()
        {
            tableCards.Clear();
            pokerDeck.CreateDeck();
            pokerDeck.ShuffleDeck();                    
        }

        public void RaiseBlinds(double smallIncrement, double bigIncrement)
        {
            smallBlind += smallIncrement;
            bigIncrement += bigIncrement;
        }

        public void PrintPlayerHands()
        {
            foreach (Player p in players)
            {
                Console.WriteLine(p.ToString());
            }
        }

        public void PrintTableCards()
        {
            String TableCards = "Table Cards: {";
            foreach (Card c in tableCards)
            {
                TableCards += c.ToString() + " ";
            }
            Console.WriteLine(TableCards + "}");
        }

        public void AddPlayer(Player p)
        {
            players.Add(p);
        }

        public void AddToPot( double amount )
        {
            pot += amount;
        }

        public void DealOut()
        {
            foreach (Player currentPlayer in players)
            {
                currentPlayer.CardSet.Add(pokerDeck.DealOut());
                currentPlayer.CardSet.Add(pokerDeck.DealOut());
            }
        }

        public void TurnOver()
        {
            tableCards.Add(pokerDeck.DealOut());
            tableCards.Add(pokerDeck.DealOut());
            tableCards.Add(pokerDeck.DealOut()); 
        }

        public void RiverRun()
        {
            tableCards.Add(pokerDeck.DealOut());
        }

        public void CompareHands()
        {
            CompareHands(players);
        }

        public void CompareHands(List<Player> CompetingPlayers)
        {
            List<Player> WinningPlayers = new List<Player>();
            Hand bestHand = null;
            foreach (Player p in CompetingPlayers)
            {
                p.CardSet.AddRange(tableCards);
                if (bestHand == null || bestHand.HandValue() < (p.BestHand().HandValue()))
                {
                    if (bestHand != null) 
                        Console.WriteLine(p.BestHand().ToString() + " Is Better Than " + bestHand.ToString());
                    bestHand = p.BestHand();
                    WinningPlayers.Clear();
                    WinningPlayers.Add(p);
                    
                }
                else if (bestHand.HandValue() == p.BestHand().HandValue())
                {
                    Console.WriteLine(p.BestHand().ToString() + " Is Equal To " + bestHand.ToString());
                    WinningPlayers.Add(p);
                }
                else
                {
                    Console.WriteLine(p.BestHand().ToString() + " Is Less Than " + bestHand.ToString());
                }
            }
        }

        public void PerformRound()
        {
            DealOut();
            foreach (Player currentPlayer in players)
            {               
                //Grab Current Information
                //currentPlayer.Decision(State.Dealout,);
            }
            TurnOver();
            foreach (Player currentPlayer in players)
            {
                //   currentPlayer.Decision(State.Run);
            }
            RiverRun();
            foreach (Player currentPlayer in players)
            {
                //   currentPlayer.Decision(State.TurnOver);
            }
            RiverRun();
            foreach (Player currentPlayer in players)
            {
                //   currentPlayer.Decision(State.River);
            }

        }

        public bool IsGameOver()
        {
            bool isGameOVer = true;
            int count = 0;
            foreach (Player p in Players)            
                if (p.Money <= 0)                
                    count++;                                          
            return (count > 1);
        }

        public void SaveGameRound()
        {
            using (var context = new PokerDataContext())
            {
                Round round = new Round
                {                    
                    Hand1 = players[0].CardSet.ToString(),
                    Hand2 = players[1].CardSet.ToString(),
                    Hand3 = players[2].CardSet.ToString(),
                    Hand4 = players[3].CardSet.ToString(),
                    Hand5 = players[4].CardSet.ToString(),
                    Hand6 = players[5].CardSet.ToString(),
                    Hand7 = players[6].CardSet.ToString(),
                    Hand8 = players[7].CardSet.ToString(),
                    Hand9 = players[8].CardSet.ToString(),
                    TurnOver = tableCards.GetRange(0, 3).ToString(),
                    Run = tableCards[3].ToString(),
                    River = tableCards[4].ToString()
                };

                context.Rounds.Attach(round);               
            }

            
        }


    }
}
