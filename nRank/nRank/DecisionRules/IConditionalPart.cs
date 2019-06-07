using System.Collections.Generic;

namespace nRank.DecisionRules
{
    interface IConditionalPart
    {
        bool IsTrueFor(Dictionary<string, float> attributes);
        string ToString();
        bool IsEmpty();
    }
}