using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.VCDomLEMAbstractions
{
    interface IUnion
    {
        IInformationTable InformationTable { get; }
        ISet<int> Classes { get; }
        bool IsUpward { get; }
    }
}
