using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ArtificialIntelligence
{
    public class NeuronDTO
    {
        private int id;
        private int depth;
        private List<int> inputs;
        private List<int> outputs;
        private List<Items> weightings;
        private Neuron.TransmissionType transmissionType;
        private Neuron.Functionality functionality;
        private double threshold;

        public NeuronDTO()
        {
            id = 0;
            inputs = new List<int>();
            outputs = new List<int>();
            weightings = new List<Items>();
            threshold = 1.0;
        }

        public NeuronDTO(int aId, List<int> anInputs, List<int> anOutputs, List<Items> aWeightings, Neuron.TransmissionType aTransmissionTypes, Neuron.Functionality aFunctionality, double aThreshold)
        {
            id = aId;
            inputs = anInputs;
            outputs = anOutputs;
            weightings = aWeightings;
            transmissionType = aTransmissionTypes;
            functionality = aFunctionality;
            threshold = aThreshold;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public List<int> Inputs
        {
            get { return inputs; }
            set { inputs = value; }
        }

        public List<int> Outputs
        {
            get { return outputs; }
            set { outputs = value; }
        }

        public List<Items> Weightings
        {
            get { return weightings; }
            set { weightings = value; }
        }

        public Neuron.TransmissionType TransmissionType
        {
            get { return transmissionType; }
            set { transmissionType = value; }
        }

        public Neuron.Functionality Functionality
        {
            get { return functionality; }
            set { functionality = value; }
        }

        public double Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }

        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }
    }

    public class Items
    {
        [XmlAttribute]
        public int key;

        [XmlAttribute]
        public double value;
    }
}
