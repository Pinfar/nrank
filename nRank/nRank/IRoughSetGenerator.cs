using System.Collections.Generic;

namespace nRank
{
    internal interface IRoughSetGenerator
    {
        IEnumerable<ILowerApproximation> GenerateLowerApproximations(IInformationTable informationTable);
    }
}