using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;


namespace ArtificialIntelligence
{
    public class Neuron : IComparable<Neuron>
    {
        public enum Functionality { Excitatory, Inhibitive }
        public enum TransmissionType { Linear, ThresholdLinear, Sigmoid, BinaryThreshold }
        public static Functionality[] Functions = { Functionality.Excitatory, Functionality.Inhibitive };
        public static TransmissionType[] TransmissionTypes = { TransmissionType.Linear, TransmissionType.ThresholdLinear, TransmissionType.Sigmoid, TransmissionType.BinaryThreshold };
        private static int NEURONCOUNT = 0;
        private static Random rand = new Random(DateTime.Now.Millisecond);

        private int depth;
        private int id;
        private double transmissionStrength, threshold;
        private bool isActivated;
        private Dictionary<int, double> weightings;
        private List<Neuron> InboundConnectedNeurons, OutboundConnectedNeurons;
        private TransmissionType transmission;
        private Functionality neuronType;

        public Neuron()
        {
            InboundConnectedNeurons = new List<Neuron>();
            OutboundConnectedNeurons = new List<Neuron>();
            weightings = new Dictionary<int, double>();
            id = NEURONCOUNT++;
            Threshold = 1.0;
            transmission = TransmissionType.BinaryThreshold;
            neuronType = Functionality.Excitatory;
            isActivated = false;
        }

        #region Properties

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public TransmissionType Transmission
        {
            get { return transmission; }
            set { transmission = value; }
        }

        public Functionality NeuronType
        {
            get { return neuronType; }
            set { neuronType = value; }
        }

        public Dictionary<int, double> Weightings
        {
            get { return weightings; }
            set { weightings = value; }
        }

        public List<Neuron> InBoundConnections
        {
            get { return InboundConnectedNeurons; }
            set { InboundConnectedNeurons = value; }

        }

        public List<Neuron> OutboundConnections
        {
            get { return OutboundConnectedNeurons; }
            set { OutboundConnectedNeurons = value; }
        }

        public double Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }

        public double TransmissionStrength
        {
            get { return transmissionStrength; }
            set { transmissionStrength = value; }
        }

        public bool IsActivated
        {
            get { return isActivated; }
            set { isActivated = value; }
        }

        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        #endregion
        public double CalculateSignalStrength()
        {
            double sum = 0;

            for (int i = 0; i < InboundConnectedNeurons.Count; i++)
            {
                if (InboundConnectedNeurons[i].NeuronType == Functionality.Excitatory)
                {
                    sum += InboundConnectedNeurons[i].TransmissionStrength * weightings[i];
                }
                else
                {
                    sum -= InboundConnectedNeurons[i].TransmissionStrength * weightings[i];
                }
            }
            if (transmission == TransmissionType.Linear)
                return sum;
            else if (transmission == TransmissionType.BinaryThreshold)
                return (sum > Threshold ? 1.0 : 0.0);
            else if (transmission == TransmissionType.ThresholdLinear)
                return (sum - Threshold < 0 ? 0 : sum - Threshold);
            else
                return (sum < -5 ? 0 : sum > 5 ? 1.0 : (1 / (1 + Math.Exp(sum))));

        }

        public void Transmit()
        {
            if (neuronType == Functionality.Excitatory)
                TransmissionStrength = CalculateSignalStrength();
            else
                TransmissionStrength = -CalculateSignalStrength();
        }

        public void Connect(Neuron n)
        {
            if (!OutboundConnectedNeurons.Contains(n))
                OutboundConnectedNeurons.Add(n);
            else
                n.weightings[n.InboundConnectedNeurons.IndexOf(this)] /= 2.0;
        }

        public void Prune(Neuron n)
        {
            OutboundConnectedNeurons.Remove(n);
            InboundConnectedNeurons.Remove(n);
        }

        public int CompareTo(Neuron other)
        {
            return this.depth.CompareTo(other.Depth);
        }

        public List<int> Inputs()
        {
            List<int> Inputs = new List<int>();

            foreach (Neuron n in InboundConnectedNeurons)
            {
                Inputs.Add(n.id);
            }
            return Inputs;
        }

        public List<int> Outputs()
        {
            List<int> Outputs = new List<int>();

            foreach (Neuron n in OutboundConnectedNeurons)
            {
                Outputs.Add(n.id);
            }
            return Outputs;
        }

        public NeuronDTO ToDTO()
        {

            List<Items> weightings = new List<Items>();
            foreach (KeyValuePair<int, double> w in this.weightings)
            {
                weightings.Add(new Items { key = w.Key, value = w.Key });
            }

            NeuronDTO dto = new NeuronDTO
            {
                ID = id,
                Inputs = Inputs(),
                Outputs = Outputs(),
                Functionality = neuronType,
                TransmissionType = transmission,
                Threshold = threshold,
                Depth = depth
            };

            return dto;
        }
    }
}

