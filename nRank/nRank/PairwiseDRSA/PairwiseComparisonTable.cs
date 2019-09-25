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
            _counter = 1;
        }

        protected PairwiseComparisonTable(List<PairwiseComparisonTableEntry> entries, int counterState)
        {
            Entries = entries;
            _counter = counterState;
        }

        public List<PairwiseComparisonTableEntry> Entries { get; } = new List<PairwiseComparisonTableEntry>();
        private int _counter;

        public void Add(InformationObject obj1, RelationType relation, InformationObject obj2)
        {
            Entries.Add(new PairwiseComparisonTableEntry(obj1.Pair(obj2), relation, _counter.ToString()));
            _counter++;
        }

        public void Add(List<IAttributePair> attributes, int firstIdentifier, int secondIdentifier, RelationType relation)
        {
            var objectPair = new InformationObjectPair(attributes, firstIdentifier, secondIdentifier);
            var entry = new PairwiseComparisonTableEntry(objectPair, relation, _counter.ToString());
            Entries.Add(entry);
            _counter++;
        }

        public PairwiseComparisonTable Filter(Func<PairwiseComparisonTableEntry, bool> predicate)
        {
            return new PairwiseComparisonTable(Entries.Where(predicate).ToList(), _counter);
        }

        public List<InformationObjectPair> AsInformationObjectPairs()
        {
            return Entries.Select(x => x.ObjectPair).ToList();
        }

        public List<string> ToLabelList()
        {
            return Entries.Select(x => $"{{{ x.ToString() }}}").ToList();
        }

        public List<string> ToDisplayableTable()
        {
            if (Entries.Count == 0) return new List<string> { "(PCT is Empty)" };
            var entry = Entries.First();
            var labels = entry.ObjectPair.GetAttributes().Select(x => x.Label);
            var header = $"ID, Pair, {string.Join(", ", labels)}, Relation";
            var values = Entries.Select(x => x.ToRowString());
            var resultList = new List<string>(Entries.Count + 1)
            {
                header
            };
            resultList.AddRange(values);
            return resultList;
        }

        public class PairwiseComparisonTableEntry
        {
            public PairwiseComparisonTableEntry(InformationObjectPair objectPair, RelationType relation, string id)
            {
                ObjectPair = objectPair;
                Relation = relation;
                Id = id;
                ObjectPair.Id = id;
            }

            public InformationObjectPair ObjectPair { get; }
            public RelationType Relation { get; }
            public string Id { get; }

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
                return $"{ObjectPair.FirstIdentifier} {Relation.ToString("g")} {ObjectPair.SecondIdentifier}";
            }

            public string ToRowString()
            {
                var attValues = ObjectPair.GetAttributes().Select(x => x.GetValueAsString());
                return $"{Id}, {{{ObjectPair.FirstIdentifier}, {ObjectPair.SecondIdentifier}}}, {string.Join(", ", attValues)}, {Relation.ToString("g")}";
            }
        }

        public enum RelationType
        {
            S,
            Sc
        }
    }
}
