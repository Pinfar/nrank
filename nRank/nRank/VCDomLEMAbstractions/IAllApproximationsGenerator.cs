using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.VCDomLEMAbstractions
{
    interface IAllApproximationsGenerator
    {
        ParallelQuery<IApproximation> GetApproximations(IInformationTable originalTable, float consistencyLevel);
    }
}
