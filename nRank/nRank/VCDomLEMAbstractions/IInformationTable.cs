using System.Collections.Generic;

namespace nRank.VCDomLEMAbstractions
{
    public interface IInformationTable
    {
        string DecisionAttributeName { get; }

        void AddObject(string identifier, Dictionary<string, float> attributes, int decisionAttributeValue);
        Dictionary<string, int> GetDecisionAttribute();
        IEnumerable<float> GetAttribute(string name);
        IInformationTable Filter(Dictionary<string, bool> pattern);
        IEnumerable<int> GetDecicionClassesWorstFirst();
        bool Outranks(string identifier1, string identifier2);
    }
}