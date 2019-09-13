using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    interface IConditionalPart
    {
        bool IsTrueFor(InformationObjectPair pair);
        string ToString();
        bool IsEmpty();
        string ToLatexString();
    }
}
