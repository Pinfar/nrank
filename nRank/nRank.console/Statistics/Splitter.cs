using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.console.Statistics
{
    class Splitter
    {
        private Random random = new Random();

        public SplitInformationTable Split(IInformationTable informationTable, float ratio)
        {
            var trainingPattern = informationTable
                .GetAllObjectIdentifiers()
                .ToDictionary(x => x, x => random.NextDouble() < ratio);
            var trainingSet = informationTable.Filter(trainingPattern);
            var testingPattern = trainingPattern.ToDictionary(x => x.Key, x => !x.Value);
            var testingSet = informationTable.Filter(testingPattern);
            return new SplitInformationTable(trainingSet, testingSet);
        }
    }
}
