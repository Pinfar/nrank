using System.Collections.Generic;

namespace nRank.VCDomLEMAbstractions
{
    internal interface IUnionGenerator
    {
        IEnumerable<IInformationTable> GenerateUnions(IInformationTable informationTable);
    }
}