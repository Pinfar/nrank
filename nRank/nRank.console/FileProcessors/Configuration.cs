using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.console.FileProcessors
{
    public class Configuration
    {
        public string LearningDataFile { get; set; }
        public float Consistency { get; set; }
        public List<Relation> Pairs { get; set; }
        public string PCTDataFile { get; set; }
    }

    public class Relation
    {
        public int First { get; set; }
        public int Second { get; set; }
        public PairwiseDRSA.PairwiseComparisonTable.RelationType Symbol { get; set; }
    }
}
