using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialIntelligence
{
    public class SynapseDTO
    {
        private int a, b;
        private int numberOfTimesActivated;

        public int A
        {
            get { return a; }
            set { a = value; }
        }

        public int B
        {
            get { return b; }
            set { b = value; }
        }

        public int NumberOfTimesActivated
        {
            get { return numberOfTimesActivated; }
            set { numberOfTimesActivated = value; }
        }
    }
}
