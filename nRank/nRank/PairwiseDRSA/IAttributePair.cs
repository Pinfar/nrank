namespace nRank.PairwiseDRSA
{
    public interface IAttributePair
    {
        string Label { get; }

        bool IsWeaklyPreferredTo(IAttributePair other);
        string ToString(PairwiseComparisonTable.RelationType relation);
    }
}