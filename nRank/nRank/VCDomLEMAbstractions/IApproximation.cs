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
        ISet<string> AllowedOperators { get; }
        string Symbol { get; }
    }
}
