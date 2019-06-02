using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.DataStructures
{
    class InformationObject
    {
        public string Identifier { get; set; }
        public int DecisionAttributeValue { get; set; }
        public string DecisionAttributeName { get; set; }
        public Dictionary<string, float> Attributes { get; set; }
    }
}
