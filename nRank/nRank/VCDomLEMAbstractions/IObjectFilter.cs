namespace nRank.VCDomLEMAbstractions
{
    internal interface IObjectFilter
    {
        IUnion GetAllowedObjects(IUnion approximation);
    }
}