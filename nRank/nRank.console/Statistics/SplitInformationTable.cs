using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.console.Statistics
{
    struct SplitInformationTable
    {
        public SplitInformationTable(IInformationTable trainingTable, IInformationTable testingTable) : this()
        {
            TrainingTable = trainingTable;
            TestingTable = testingTable;
        }

        public IInformationTable TrainingTable { get; }
        public IInformationTable TestingTable { get; }
    }
}
