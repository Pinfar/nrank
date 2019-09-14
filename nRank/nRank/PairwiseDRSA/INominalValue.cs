namespace nRank.PairwiseDRSA
{
    public interface INominalValue
    {
        AttributeType Type { get; }

        IPreferable DifferenceWith(INominalValue value);
    }
}