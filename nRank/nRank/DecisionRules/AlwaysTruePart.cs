using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.DecisionRules
{
    class AlwaysTruePart : IConditionalPart
    {
        public bool IsEmpty()
        {
            return true;
        }

        public bool IsTrueFor(Dictionary<string, float> attributes)
        {
            return true;
        }

        public override string ToString()
        {
            return $"(true)";
        }
    }
}
