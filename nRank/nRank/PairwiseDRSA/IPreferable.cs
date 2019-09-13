namespace nRank.PairwiseDRSA
{
    public interface IPreferable
    {
        bool IsWeaklyPreferedTo(IPreferable other);
    }
}