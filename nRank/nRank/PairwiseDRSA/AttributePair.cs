using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class AttributePair
    {
        public AttributePair(IAttribute first, IAttribute second)
        {
            if (first.Label != second.Label) throw new InvalidOperationException("Can not create attribute pair from different attributes!");
            First = first;
            Second = second;
        }

        public IAttribute First { get; }
        public IAttribute Second { get; }

        public override string ToString()
        {
            return $"{First.Label}: {First.StringValue} , {Second.StringValue}";
        }
    }
}
