using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.VCDomLEMAbstractions
{
    interface IApproximation
    {
        IInformationTable ApproximatedInformationTable { get; }
        IInformationTable OriginalInformationTable { get; }
        ISet<int> Classes { get; }
        string AllowedGainOperator { get; }
        string AllowedCostOperator { get; }
        string Symbol { get; }
        IUnion Union { get; }
        IList<string> PositiveRegion { get; }

        IInformationTable GetNegatedApproximatedInformationTable();
    }
}
