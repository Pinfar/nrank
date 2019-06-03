using nRank.VCDomLEMAbstractions;

namespace nRank.DSetGenerators
{
    interface IDDSetGenerator
    {
        IInformationTable Generate(IInformationTable informationTable, string objectIdentifier);
    }
}