using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialIntelligence
{
    public class Synapse
    {
        private Neuron a,b;
        private int numberOfTimesActivated;

        public Synapse()
        {
            a = null;
            b = null;
            numberOfTimesActivated = 0;
        }

        public Synapse(Neuron x, Neuron y)
        {
            a = x;
            b = y;
            numberOfTimesActivated = 1;
        }

        public Neuron A
        {
            get { return a; }
        }

        public Neuron B
        {
            get { return b; }
        }

        public int TimesActivated { get { return numberOfTimesActivated; } }

        public void Activate() { numberOfTimesActivated++;}

        public SynapseDTO ToDTO()
        {
            SynapseDTO dto = new SynapseDTO
            {
                A = this.A.ID,
                B = this.B.ID,
                NumberOfTimesActivated = this.numberOfTimesActivated
            };

            return dto;
        }

    }
}
