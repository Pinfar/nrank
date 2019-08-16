using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.VCDomLEMAbstractions
{
    public interface IModel
    {
        IList<IDecisionRule> Rules { get; }

        IList<int> Predict(IList<string> identifiers, IInformationTable table);
    }
}
