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

        public InformationObject First { get; }
        public InformationObject Second { get; }

        public bool Dominates(InformationObjectPair other)
        {
            return DominatesOrdinal(other) && DominatesNominal(other);
        }

        public List<AttributePair> GetAttributes()
        {
            var list = new List<AttributePair>();
            var ordinals = First.OrdinalAttributes.Zip(Second.OrdinalAttributes, (x, y) => new AttributePair(x, y));
            list.AddRange(ordinals);
            var nominals = First.NominalAttributes.Zip(Second.NominalAttributes, (x, y) => new AttributePair(x, y));
            list.AddRange(nominals);
            return list;
        }

        public AttributePair GetAttribute(string label)
        {
            var first = First.OrdinalAttributes.SingleOrDefault(x => x.Label == label);
            if(first != null)
            {
                var second = Second.OrdinalAttributes.Single(x => x.Label == label);
                return new AttributePair(first, second);
            }

            var firstNom = First.NominalAttributes.Single(x => x.Label == label);
            var secondNom = Second.NominalAttributes.Single(x => x.Label == label);
            return new AttributePair(firstNom, secondNom);
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
