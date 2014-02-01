using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace ArtificialIntelligence
{
    public class NeuralNetwork
    {
        
        private static Random random = new Random();
        
        List<Neuron> InputNeurons;
        List<Neuron> OutputNeurons;
        List<Neuron> network;
        List<Synapse> Connections;

        public NeuralNetwork()
        {
            InputNeurons = new List<Neuron>();
            network = new List<Neuron>();
            OutputNeurons = new List<Neuron>();
            Connections = new List<Synapse>();
        }

        public List<Neuron> Outputs
        {
            get { return OutputNeurons; }
            set { OutputNeurons = value; }
        }

        public List<Neuron> Inputs
        {
            get { return InputNeurons; }
            set { InputNeurons = value; }
        }

        public List<Neuron> Network
        {
            get {return network;}
            set {network = value;}
        }

        public List<double> GetOutputs()
        {
            List<double> outputs = new List<double>();
            foreach (Neuron n in OutputNeurons)
            {
                outputs.Add(n.TransmissionStrength);
            }
            return outputs;
        }

        public List<Synapse> Synapse
        {
            get { return Connections; }
            set { Connections = value; }
        }

        public void CreateInputs(IList<double> inputs)
        {
            foreach (double input in inputs)
                CreateInputNeuron(input);
        }       

        public void SetInputs(IList<double> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                InputNeurons[i].TransmissionStrength = values[i];
            }
        }

        public void CreateInputNeuron(double inputValue)
        {
            Neuron input = new Neuron
            {
                Depth = 1,
                TransmissionStrength = inputValue,                
                Threshold = 0.0,
                IsActivated = true,
                Transmission =  Neuron.TransmissionType.Linear
            };
            InputNeurons.Add(input);
            Network.Add(input);
        }

        public void ChangeThreshold(int inputIndex, double factorOfChange)
        {
            Neuron StartNode = InputNeurons[inputIndex];
            List<Synapse> SubNetwork = new List<Synapse>();
            foreach (Neuron n in StartNode.OutboundConnections)
            {
                SubNetwork.AddRange(Dive2(n));
            }

            List<Synapse> OrderedSubNetwork = SubNetwork.OrderBy(a => a.TimesActivated).ToList();
            double unitCHange = factorOfChange / (double)(OrderedSubNetwork.Count);

            double largest = OrderedSubNetwork[OrderedSubNetwork.Count - 1].TimesActivated;
            double sum = Connections.Sum(a => a.TimesActivated);
            double p = random.NextDouble();

            double value = 0;
            for (int j = 0; j < OrderedSubNetwork.Count; j++)
                for (int i = 0; i < Connections.Count; i++)
                {
                    value += (largest - (double)(OrderedSubNetwork[i].TimesActivated)) / sum;

                    if (value >= p)
                    {
                        Neuron A = OrderedSubNetwork[i].A;
                        Neuron B = OrderedSubNetwork[i].B;
                        B.Weightings[B.InBoundConnections.IndexOf(A)] += unitCHange;
                        break;
                    }
                }                     
        }

        public void CreateOutputNeuron(double outputs, Neuron.TransmissionType type)
        {
            Neuron output = new Neuron
            {
                Depth = -1,
                TransmissionStrength = 0.0,
                Threshold = 1.0,
                IsActivated = false,
                Transmission = type
            };
            OutputNeurons.Add(output);
            Network.Add(output);
        }

        public void CreateNeuron()
        {
           Neuron neuron = new Neuron 
           {
                Transmission = Neuron.TransmissionTypes[random.Next(Neuron.TransmissionTypes.Length)],
                Depth = -1,
           };
           Network.Add(neuron);          
        }

        public void PruneNeuron()
        {
            List<Synapse> OrderedSynapses = Connections.OrderBy(a => a.TimesActivated).ToList();
            double largest = OrderedSynapses[OrderedSynapses.Count - 1].TimesActivated;
            double sum = Connections.Sum(a => a.TimesActivated);
            double p = random.NextDouble();

            double value = 0;
            Neuron toPrune = null;
            for (int i = 0; i < Connections.Count; i++)
            {
                value += (largest - (double)(OrderedSynapses[i].TimesActivated)) / sum;

                if (value >= p)
                {
                    toPrune = OrderedSynapses[i].A;
                    break;
                }
            }
            if (toPrune == null || InputNeurons.Contains(toPrune) || OutputNeurons.Contains(toPrune))
                return;
            foreach (Neuron n in toPrune.OutboundConnections)
            {
                n.InBoundConnections.Remove(toPrune);              
            }

            foreach (Neuron n in toPrune.InBoundConnections)
            {
                n.InBoundConnections.Remove(toPrune);
            }

            Connections.RemoveAll(a => a.A == toPrune || a.B == toPrune);
        }

        public void PruneConnection()
        {
            List<Synapse> OrderedSynapses = Connections.OrderBy(a => a.TimesActivated).ToList();
            double largest = OrderedSynapses[OrderedSynapses.Count - 1].TimesActivated;            
            double sum = Connections.Sum(a => a.TimesActivated);
            double p = random.NextDouble();

            double value = 0;
            Synapse toPrune = null;
            for (int i = 0; i < Connections.Count; i++)
            {
                value += (sum > 0 ? (largest - (double)(OrderedSynapses[i].TimesActivated) + 1.0) / sum : 0);

                if (value >= p)
                {
                    toPrune = OrderedSynapses[i];
                    break;
                }
            }
            if (toPrune == null)
                return;
            
            toPrune.A.OutboundConnections.Remove(toPrune.B);
            toPrune.B.InBoundConnections.Remove(toPrune.A);
            Connections.Remove(toPrune);

        }

        private Neuron RouletteWheelSelection()
        {
            //Roulette Wheel Selection of Nodes
            double sum = 0; double max = Network.Max(a => a.Depth) + 1;
            double probability = random.NextDouble();
            foreach (Neuron n in Network)
            {
                if (n.Depth != -1)
                    sum += max - n.Depth;
                else sum += max;
            }

            double weight = 0;            
            foreach (Neuron n in Network)
            {
                weight += (max - (n.Depth == -1 ? 0.0 : (double)n.Depth)) / sum;

                if (weight >= probability)
                    return n;
            }

            return Network.Last();
        }

        public void CreateConnection()
        {
            Network = Network.OrderBy(a => a.Depth).ToList();                                   
            Neuron x = null;
            Neuron y = null;            
                  
            while (x == null || y == null || x == y || Outputs.Contains(x) || Inputs.Contains(y) || CheckForCycle(x,y) || x.OutboundConnections.Contains(y))
            {
                x = RouletteWheelSelection();
                y = RouletteWheelSelection();
                            
            }

            Synapse s = new Synapse(x, y);
            Connections.Add(s);
            x.OutboundConnections.Add(y);
            y.InBoundConnections.Add(x);            
            //Set Appropriate Weightings;           
            y.Weightings.Add(x.ID ,random.NextDouble());

            if (y.Depth == -1 || x.Depth > y.Depth)
            {
                y.Depth = x.Depth + 1;
            }
           

        }
        
        public bool CheckForCycle(Neuron u, Neuron v)
        {
            bool cycle = false;
            foreach (Neuron x in v.OutboundConnections)
            {
                if (!x.IsActivated)
                    cycle = cycle || DFS(u, x);             
            }

            ResetNetwork();
            return cycle;
        }
        
        public bool DFS(Neuron root, Neuron u)
        {
            if (u == root)
                return true;            
            u.IsActivated = true;
            foreach (Neuron x in u.OutboundConnections)
            {
                if (!x.IsActivated)
                    DFS(root, x);
            }
            return false;
        }

        public void SetDepths()
        {
            foreach (Neuron n in InputNeurons)
                SetDepths(n);
        }

        public void SetDepths(Neuron n)
        {
            foreach (Neuron m in n.OutboundConnections)
            {
                if (n.Depth >= m.Depth)
                    m.Depth = n.Depth + 1;
            }
            ResetNetwork();
        }

        public bool Dive(Neuron u)
        {            
            u.IsActivated = true;
            foreach (Neuron x in u.OutboundConnections)
            {
                Dive(x);
            }
            return false;
        }

        public List<Synapse> Dive2(Neuron n)
        {
            List<Synapse> connections = new List<Synapse>();

            foreach (Neuron x in n.OutboundConnections)
            {
                Synapse s = Connections.Find(a => a.A == n && a.B == x);
                connections.Add(s);
            }
            
            foreach (Neuron x in n.OutboundConnections)
            {
               connections.AddRange(Dive2(x).ToList());
            }
            return connections;
        }

        public void ResetNetwork()
        {
            foreach (Neuron n in Network)
                n.IsActivated = false;
        }        

        public void EvaluateNetwork(List<double> inputs)
        {
            if (inputs.Count != InputNeurons.Count)
                return;

            for (int i = 0; i < inputs.Count; i++)
            {
                InputNeurons[i].TransmissionStrength = inputs[i];
            }

            Network.Sort();

            foreach (Neuron n in Network)
            {
              n.CalculateSignalStrength();                          
            }

            foreach (Synapse synapse in Connections)
            {
                if (synapse.A.IsActivated)
                {
                    synapse.Activate();
                }
            }

            
        }

        public void ArbitraryTuneing(double percentage, int inputIndex)
        {
            Neuron StartNode = InputNeurons[inputIndex];
            List<Synapse> SubNetwork = new List<Synapse>();
            foreach (Neuron n in StartNode.OutboundConnections)
            {
                SubNetwork.AddRange(Dive2(n));                
            }
            
            List<Synapse> OrderedSubNetwork = SubNetwork.OrderBy(a => a.TimesActivated).ToList();
            double unitCHange = percentage / (double)(OrderedSubNetwork.Count);

            double largest = OrderedSubNetwork[OrderedSubNetwork.Count - 1].TimesActivated;
            double sum = Connections.Sum(a => a.TimesActivated);
            double p = random.NextDouble();

            double value = 0;         
            for (int j = 0 ; j < OrderedSubNetwork.Count; j++)
                for (int i = 0; i < Connections.Count; i++)
                {
                    value += (largest - (double)(OrderedSubNetwork[i].TimesActivated)) / sum;

                    if (value >= p)
                    {
                        Neuron A = OrderedSubNetwork[i].A;
                        Neuron B = OrderedSubNetwork[i].B;
                        B.Weightings[B.InBoundConnections.IndexOf(A)] += percentage; 
                        break;
                    }
                }                                       
        }
      
        public NeuralNetworkDTO ToDTO()
        {
            List<NeuronDTO> inputs = new List<NeuronDTO>();
            foreach (Neuron i in Inputs)
            {
                inputs.Add(i.ToDTO());
            }

            List<NeuronDTO> outputs = new List<NeuronDTO>();
            foreach (Neuron o in Outputs)
            {
                outputs.Add(o.ToDTO());
            }

            List<NeuronDTO> network = new List<NeuronDTO>();
            foreach (Neuron n in Network)
            {
                network.Add(n.ToDTO());
            }


            List<SynapseDTO> connections = new List<SynapseDTO>();
            foreach (Synapse n in Connections)
            {
                connections.Add(n.ToDTO());
            }
            NeuralNetworkDTO dto = new NeuralNetworkDTO
            {
                InputNeurons = inputs,
                OutputNeurons = outputs,
                Network = network,
                Connections = connections
            };

            return dto;
        }



    }
}
