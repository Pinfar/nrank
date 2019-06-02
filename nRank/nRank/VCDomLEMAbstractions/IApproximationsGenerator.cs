namespace nRank.VCDomLEMAbstractions
{
    internal interface IApproximationsGenerator
    {
        IInformationTable GetApproximation(IInformationTable union, IInformationTable originalTable);
    }
}