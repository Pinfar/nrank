namespace nRank.VCDomLEMAbstractions
{
    internal interface IObjectFilter
    {
        IInformationTable GetAllowedObjects(IInformationTable approximation);
    }
}