using System.Collections.Generic;

namespace nRank.VCDomLEMAbstractions
{
    internal interface IUnionGenerator
    {
        IEnumerable<IUnion> GenerateUnions(IInformationTable informationTable);
    }
}