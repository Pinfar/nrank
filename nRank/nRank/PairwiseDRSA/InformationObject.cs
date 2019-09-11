using System;
using System.Collections.Generic;
using System.Linq;

namespace nRank.PairwiseDRSA
{
    class InformationObject
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
    }
}