namespace nRank.VCDomLEMAbstractions
{
    internal interface IApproximationsGenerator
    {
        IApproximation GetApproximation(IUnion union, IInformationTable originalTable);
    }
}