using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PokerGame;
using ArtificialIntelligence;
using System.IO;
namespace PokerProgram
{
    static class Program
    {
       /// <summary>
       /// The main entry point for the application.
       /// </summary>
       
        public static NeuralNetwork FindBestNetwork()
        {
            //Perform Database LookUp Of Each Player
            return null;
        }

        public static void PlayPokerGame(List<NeuralNetwork> intelligences)
        {
            //Play Poker Gmae
            Game game = new Game();
            game.AddPlayer(new Player(10000, "Brendan Smith"));
            game.AddPlayer(new Player(10000, "Flip Vegas"));
            game.AddPlayer(new Player(10000, "Boy Budda"));
            game.AddPlayer(new Player(10000, "Hisoka"));
            game.AddPlayer(new Player(10000, "Shaefox"));
            game.AddPlayer(new Player(10000, "Grungy"));
            game.AddPlayer(new Player(10000, "Donald Smith"));
            game.AddPlayer(new Player(10000, "Dave"));

            for (int i = 0; i < intelligences.Count; i++)
            {
                game.Players[i].Intelligence = intelligences[i];
            }

            while (!game.IsGameOver())
            {
                game.NewRound();
                game.PerformRound();               
            }

        }

        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            
            Game testRound = new Game();
            Player p1 = new Player(100.0, "Brendan Smith");
            Player p2 = new Player(100.0, "Jericho James");
            Player p3 = new Player(100.0, "James Bond");
            Player p4 = new Player(100.0, "Miguel");

            testRound.AddPlayer(p1);
            testRound.AddPlayer(p2);
            testRound.AddPlayer(p3);
            testRound.AddPlayer(p4);

            testRound.NewRound();
            testRound.DealOut();
            testRound.TurnOver();
            testRound.PrintTableCards();
            testRound.RiverRun();
            testRound.PrintTableCards();
            testRound.RiverRun();
            testRound.PrintTableCards();                      
            testRound.PrintPlayerHands();
            testRound.CompareHands();
            

            /* Hand h = new Hand();
            h.AddCard(new Card("2", "H","",));
            h.AddCard(new Card("3", "H", ""));
            h.AddCard(new Card("4", "H", ""));
            h.AddCard(new Card("A", "H", ""));
            Console.WriteLine(h.HandValue());
            */
            List<Neuron.TransmissionType> outputTypes = new List<Neuron.TransmissionType>();
            outputTypes.Add(Neuron.TransmissionType.BinaryThreshold);
            outputTypes.Add(Neuron.TransmissionType.BinaryThreshold);
            outputTypes.Add(Neuron.TransmissionType.BinaryThreshold);
            outputTypes.Add(Neuron.TransmissionType.BinaryThreshold);

            ArtificialIntelligenceController aiControl = ArtificialIntelligenceController.Instance();
            NeuralNetwork network = aiControl.CreateAIStructure(7, 1, outputTypes);
            FileStream test = new FileStream("MyNeuralNetwork.xml", FileMode.Create);
            
            aiControl.Serialize(test, network);

            FileStream f = new FileStream("test.xml", FileMode.Open);
           
            NeuralNetwork newNetwork = aiControl.ConvertToNeuralNetwork(f);

        }
    }
}
