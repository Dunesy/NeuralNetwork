using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokerGame;
namespace Utlity
{
    class HandGenerator
    {
        static void Main(string[] args)
        {
            Deck deck = new Deck();
            deck.CreateDeck();
            int counter = 0;
            Card c1 = null, c2 = null, c3 = null, c4 = null, c5 = null;                       
            for (int i = 0; i < deck.deck.Count; i++)
            {
                c1 = deck.deck[i];
                for (int j = i + 1; j < deck.deck.Count; j++)
                {
                    c2 = deck.deck[j];
                    for (int k = j + 1; k < deck.deck.Count; k++)
                    {
                        c3 = deck.deck[k];
                         for (int l = k + 1; l < deck.deck.Count; l++)
                         {                             
                             using (var context = new HandDataContext())                            
                             {                            
                                c4 = deck.deck[l];
                                for (int m = l + 1; m < deck.deck.Count; m++)
                                {
                                    c5 = deck.deck[m];                                                                
                                    PokerGame.Hand h = new PokerGame.Hand();
                                    h.AddCard(c1);
                                    h.AddCard(c2);
                                    h.AddCard(c3);
                                    h.AddCard(c4);
                                    h.AddCard(c5);                                
                                    int score = h.HandValue();
                                    counter++;                                                               
                                    Hand toInsert = new Hand
                                    {                                       
                                        card1 = c1.Type + c1.Suit,
                                        card2 = c2.Type + c2.Suit,
                                        card3 = c3.Type + c3.Suit,
                                        card4 = c4.Type + c4.Suit,
                                        card5 = c5.Type + c5.Suit,
                                        score = score
                                    };
                                    Console.WriteLine(counter);
                                    context.Hands.InsertOnSubmit(toInsert);                                                                                                                      
                                }
                                 context.SubmitChanges();
                              }
                          }
                       }
                    }
                }
            
            
            /*
            PokerGame.Hand h = new PokerGame.Hand();
            h.AddCard(new Card("5", "S", ""));
            h.AddCard(new Card("A", "S", ""));
            h.AddCard(new Card("A", "S", ""));
            h.AddCard(new Card("A", "S", ""));
            h.AddCard(new Card("A", "H", ""));
            Console.WriteLine(PokerGame.Hand.IsFourOfAKind(h.hand));
            */

             }             
        }
    }

