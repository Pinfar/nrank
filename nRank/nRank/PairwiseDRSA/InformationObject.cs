using System;
using System.Collections.Generic;
using System.Linq;

namespace nRank.PairwiseDRSA
{
    public class InformationObject
    {
        public InformationObject(int intIdentifier, string descriptionIdentifier, List<IAttribute> attributes)
        {
            IntIdentifier = intIdentifier;
            DescriptionIdentifier = descriptionIdentifier;
            Attributes = attributes;
        }

        public int IntIdentifier { get; }
        public string DescriptionIdentifier { get; }
        public List<IAttribute> Attributes { get; }

        private List<NominalAttribute> _lazyNominalAttributes;
        private List<NominalAttribute> GetNominalAttributes() => Attributes.Where(x => x is NominalAttribute).Select(x => (NominalAttribute)x).ToList();
        public List<NominalAttribute> NominalAttributes => _lazyNominalAttributes ?? (_lazyNominalAttributes = GetNominalAttributes());

        private List<OrdinalAttribute> _lazyOrdinalAttributes;
        private List<OrdinalAttribute> GetOrdinalAttributes() => Attributes.Where(x => x is OrdinalAttribute).Select(x => (OrdinalAttribute)x).ToList();
        public List<OrdinalAttribute> OrdinalAttributes => _lazyOrdinalAttributes ?? (_lazyOrdinalAttributes = GetOrdinalAttributes());

        public InformationObjectPair Pair(InformationObject second)
        {
            return new InformationObjectPair(this, second);
        }

        public override bool Equals(object obj)
        {
            var @object = obj as InformationObject;
            return @object != null &&
                   IntIdentifier == @object.IntIdentifier;
        }

        public override int GetHashCode()
        {
            return 988103285 + IntIdentifier.GetHashCode();
        }
    }
}