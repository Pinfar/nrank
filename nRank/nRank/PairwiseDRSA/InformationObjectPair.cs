using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    public class InformationObjectPair
    {
        public InformationObjectPair(InformationObject first, InformationObject second)
        {
            FirstIdentifier = first.IntIdentifier;
            SecondIdentifier = second.IntIdentifier;
            _ordinals = first.OrdinalAttributes.Zip(second.OrdinalAttributes, (x, y) => new OrdinalAttributePair(x, y)).ToList();
            _nominals = first.NominalAttributes.Zip(second.NominalAttributes, (x, y) => new NominalAttributePair(x.Label, x.DifferenceWith(y))).ToList();
            _allAttributes = _ordinals.Cast<IAttributePair>().Concat(_nominals).ToList();
        }

        public int FirstIdentifier { get; }
        public int SecondIdentifier { get; }

        private List<OrdinalAttributePair> _ordinals;
        private List<NominalAttributePair> _nominals;
        private List<IAttributePair> _allAttributes;

        public string Id { get; set; }

        public bool Dominates(InformationObjectPair other)
        {
            return _allAttributes.Zip(other._allAttributes, (x, y) => x.IsWeaklyPreferredTo(y)).All(x => x);
        }

        public List<IAttributePair> GetAttributes()
        {
            return _allAttributes;
        }

        public IAttributePair GetAttribute(string label)
        {
            return _allAttributes.Single(x => x.Label == label);
        }


        public override string ToString()
        {
            return $"{FirstIdentifier} : {SecondIdentifier}";
        }

        public override bool Equals(object obj)
        {
            var pair = obj as InformationObjectPair;
            return pair != null &&
                   FirstIdentifier == pair.FirstIdentifier &&
                   SecondIdentifier == pair.SecondIdentifier;
        }

        public override int GetHashCode()
        {
            var hashCode = 343520272;
            hashCode = hashCode * -1521134295 + FirstIdentifier.GetHashCode();
            hashCode = hashCode * -1521134295 + SecondIdentifier.GetHashCode();
            return hashCode;
        }
    }
}
