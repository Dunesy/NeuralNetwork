using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using System.IO;
using System.Xml.Serialization;
namespace ArtificialIntelligence
{
    public class ArtificialIntelligenceController
    {
        private static ArtificialIntelligenceController instance;
        
        private Random rand = new Random();
        
        public static ArtificialIntelligenceController Instance()
        {
            if (instance == null)
                instance = new ArtificialIntelligenceController();
                return instance;
        }

        /// <summary>
        /// Generation of Ai Structures
        /// </summary>
        public NeuralNetwork CreateAIStructure(int inputs, int Outputs, List<Neuron.TransmissionType> OutputTypes)
        {
            NeuralNetwork ai = new NeuralNetwork();
            bool OutputsConnected = false;
            bool InputsConnected = false;

            for (int i = 0; i < inputs; i++)
                ai.CreateInputNeuron(1.0);

            for (int i = 0; i < Outputs; i++)
                ai.CreateOutputNeuron(1.0, OutputTypes[i]);
            //The Loop that Builds!
            while (!OutputsConnected || !InputsConnected)
            {
                double p = rand.NextDouble();
                if (p < 0.8)
                    ai.CreateConnection();
                else
                    ai.CreateNeuron();
                bool x = true, y = true;
                foreach (Neuron n in ai.Outputs)
                    x = x && (n.InBoundConnections.Count > 0);
                foreach (Neuron n in ai.Inputs)
                    y = y && (n.OutboundConnections.Count > 0);

                OutputsConnected = (x ? true : false);
                InputsConnected = (y ? true : false);

            }

            return ai;
        }

        public NeuralNetwork Deserialize(String neuralNetworkDTO)
        {
            
            
            return null;
        }

        //OUTPUT
        public void Serialize(Stream stream, NeuralNetwork n)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NeuralNetworkDTO));
            serializer.Serialize(stream, n.ToDTO());
        }

        public NeuralNetworkDTO Deserialize(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NeuralNetworkDTO));
            NeuralNetworkDTO n = (NeuralNetworkDTO)serializer.Deserialize(stream);
            return n;
        }

        public NeuralNetwork ConvertToNeuralNetwork(Stream stream)
        {
            NeuralNetworkDTO dto = Deserialize(stream);
            List<Neuron> network = new List<Neuron>();

            foreach (NeuronDTO n in dto.Network)
            {
                Dictionary<int, double> weightings = new Dictionary<int, double>();
                foreach (Items i in n.Weightings)
                {
                    weightings.Add(i.key, i.value);
                }
                network.Add(new Neuron
                {
                    ID = n.ID,
                    Depth = n.Depth,
                    NeuronType = n.Functionality,
                    Transmission = n.TransmissionType,
                    Threshold = n.Threshold  ,
                    Weightings = weightings
                });
            }

            foreach (Neuron n in network)
            {
                NeuronDTO m = dto.Network.Find(a => a.ID == n.ID);
                foreach (int index in m.Inputs)
                {
                    n.InBoundConnections.Add(network.Find(a => a.ID == index));
                }
            }

            foreach (Neuron n in network)
            {
                NeuronDTO m = dto.Network.Find(a => a.ID == n.ID);
                foreach (int index in m.Inputs)
                {
                    n.InBoundConnections.Add(network.Find(a => a.ID == index));
                }
                foreach (int index in m.Outputs)
                {
                    n.OutboundConnections.Add(network.Find(a => a.ID == index));
                }
            }

            List<Neuron> inputs = new List<Neuron>();
            foreach (NeuronDTO n in dto.InputNeurons)
            {
                inputs.Add(network.Find(a=>a.ID == n.ID));
            }

            List<Neuron> outputs = new List<Neuron>();
            foreach (NeuronDTO n in dto.OutputNeurons)
            {
                outputs.Add(network.Find(a => a.ID == n.ID));
            }

            List<Synapse> connections = new List<Synapse>();
            foreach (SynapseDTO n in dto.Connections)
            {
                connections.Add(new Synapse(network.Find(a=>a.ID == n.B), network.Find(a=>a.ID == n.B)));
            }

            return new NeuralNetwork
            {
                Inputs = inputs,
                Outputs = outputs,
                Network = network,
                Synapse = connections
            };

        }
    }
}
