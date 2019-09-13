using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    public class PairwiseComparisonTable
    {
        public PairwiseComparisonTable()
        {
        }

        protected PairwiseComparisonTable(List<PairwiseComparisonTableEntry> entries)
        {
            Entries = entries;
        }

        public List<PairwiseComparisonTableEntry> Entries { get; } = new List<PairwiseComparisonTableEntry>();

        public void Add(InformationObject obj1, RelationType relation, InformationObject obj2)
        {
            Entries.Add(new PairwiseComparisonTableEntry(obj1.Pair(obj2), relation));
        }

        public PairwiseComparisonTable Filter(Func<PairwiseComparisonTableEntry, bool> predicate)
        {
            return new PairwiseComparisonTable(Entries.Where(predicate).ToList());
        }

        public List<InformationObjectPair> AsInformationObjectPairs()
        {
            return Entries.Select(x => x.ObjectPair).ToList();
        }

        public class PairwiseComparisonTableEntry
        {
            public PairwiseComparisonTableEntry(InformationObjectPair objectPair, RelationType relation)
            {
                ObjectPair = objectPair;
                Relation = relation;
            }

            public InformationObjectPair ObjectPair { get; }
            public RelationType Relation { get; }

            public override bool Equals(object obj)
            {
                var entry = obj as PairwiseComparisonTableEntry;
                return entry != null &&
                       EqualityComparer<InformationObjectPair>.Default.Equals(ObjectPair, entry.ObjectPair) &&
                       Relation == entry.Relation;
            }

            public override int GetHashCode()
            {
                var hashCode = -1883596917;
                hashCode = hashCode * -1521134295 + EqualityComparer<InformationObjectPair>.Default.GetHashCode(ObjectPair);
                hashCode = hashCode * -1521134295 + Relation.GetHashCode();
                return hashCode;
            }

            public override string ToString()
            {
                return $"{ObjectPair.First.IntIdentifier} {Relation.ToString("g")} {ObjectPair.Second.IntIdentifier}";
            }
        }

        public enum RelationType
        {
            S,
            Sc
        }
    }
}
