namespace nRank.VCDomLEMAbstractions
{
    internal interface IApproximationsGenerator
    {
        IInformationTable GetApproximation(IUnion union, IInformationTable originalTable);
    }
}