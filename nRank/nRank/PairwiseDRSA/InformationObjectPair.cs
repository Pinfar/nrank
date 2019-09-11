using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class InformationObjectPair
    {
        public InformationObjectPair(InformationObject first, InformationObject second)
        {
            First = first;
            Second = second;
        }

        InformationObject First { get; }
        InformationObject Second { get; }

        public bool Dominates(InformationObjectPair other)
        {
            return DominatesOrdinal(other) && DominatesNominal(other);
        }

        private bool DominatesNominal(InformationObjectPair other)
        {
            var first = First.NominalAttributes.Zip(Second.NominalAttributes, (x, y) => x.DifferenceWith(y));
            var second = other.First.NominalAttributes.Zip(other.Second.NominalAttributes, (x, y) => x.DifferenceWith(y));
            return first.Zip(second, (x, y) => x.IsWeaklyPreferedTo(y)).All(x => x);
        }

        private bool DominatesOrdinal(InformationObjectPair other)
        {
            var first = First.OrdinalAttributes.Zip(other.First.OrdinalAttributes, (x,y) => x.IsWeaklyPreferedTo(y)).All(x => x);
            var second = Second.OrdinalAttributes.Zip(other.Second.OrdinalAttributes, (x, y) => y.IsWeaklyPreferedTo(x)).All(x => x);
            return first && second;
        }
    }
}
