using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.VCDomLEMAbstractions
{
    interface IAllApproximationsGenerator
    {
        IEnumerable<IApproximation> GetApproximations(IInformationTable originalTable);
    }
}
