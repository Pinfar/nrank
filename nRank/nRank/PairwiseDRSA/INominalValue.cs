namespace nRank.PairwiseDRSA
{
    public interface INominalValue
    {
        IPreferable DifferenceWith(INominalValue value);
    }
}