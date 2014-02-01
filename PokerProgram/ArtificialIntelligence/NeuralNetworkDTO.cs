using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialIntelligence
{
    public class NeuralNetworkDTO
    {
        private List<NeuronDTO> inputNeurons;
        private List<NeuronDTO> outputNeurons;
        private List<NeuronDTO> network;
        private List<SynapseDTO> connections;

        public List<NeuronDTO> InputNeurons
        {
            get { return inputNeurons;} 
            set  { inputNeurons = value;}
        }

        public List<NeuronDTO> OutputNeurons
        {
            get { return outputNeurons; }
            set { outputNeurons = value; }
        }

        public List<NeuronDTO> Network
        {
            get { return network; }
            set { network = value; }
        }

        public List<SynapseDTO> Connections
        {
            get { return connections; }
            set { connections = value; }
        }

    }
}
