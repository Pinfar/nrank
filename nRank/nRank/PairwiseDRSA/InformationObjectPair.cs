﻿using System;
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

        public InformationObject First { get; }
        public InformationObject Second { get; }

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



        public override bool Equals(object obj)
        {
            var pair = obj as InformationObjectPair;
            return pair != null &&
                   EqualityComparer<InformationObject>.Default.Equals(First, pair.First) &&
                   EqualityComparer<InformationObject>.Default.Equals(Second, pair.Second);
        }

        public override int GetHashCode()
        {
            var hashCode = 43270662;
            hashCode = hashCode * -1521134295 + EqualityComparer<InformationObject>.Default.GetHashCode(First);
            hashCode = hashCode * -1521134295 + EqualityComparer<InformationObject>.Default.GetHashCode(Second);
            return hashCode;
        }

        public override string ToString()
        {
            return $"{First.IntIdentifier} : {Second.IntIdentifier}";
        }
    }
}
