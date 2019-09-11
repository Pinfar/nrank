namespace nRank.PairwiseDRSA
{
    internal interface INominalValue
    {
        IPreferable DifferenceWith(INominalValue value);
    }
}