using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class AlwaysTruePart : IConditionalPart
    {
        public bool IsEmpty()
        {
            return true;
        }

        public bool IsTrueFor(InformationObjectPair attributes)
        {
            return true;
        }

        public string ToLatexString()
        {
            return ToString();
        }

        public override string ToString()
        {
            return $"(true)";
        }
    }
}
