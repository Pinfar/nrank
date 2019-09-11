namespace nRank.PairwiseDRSA
{
    internal interface IPreferable
    {
        bool IsWeaklyPreferedTo(IPreferable other);
    }
}