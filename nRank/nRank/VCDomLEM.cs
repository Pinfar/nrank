using nRank.ApproximationsGenerators;
using nRank.ApproximationsGeneratorsVC;
using nRank.DecisionRulesGenerator;
using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank
{
    public class VCDomLEM
    {
        readonly IDecisionRuleGenerator decisionRuleGenerator;
        readonly IAllApproximationsGenerator approximationsGenerator;

        public VCDomLEM()
        {
            decisionRuleGenerator = new DecisionRuleGenerator();
            approximationsGenerator = new ApproximationsGeneratorVC();
        }

        public List<IDecisionRule> GenerateDecisionRules(IInformationTable informationTable, float consistencyLevel)
        {
            var rules = new List<IDecisionRule>();
            var approximations = approximationsGenerator.GetApproximations(informationTable, consistencyLevel);
            foreach(var approximation in approximations)
            {
                var generatedRules = decisionRuleGenerator.GenerateRulesFrom(approximation, consistencyLevel);
                rules.AddRange(generatedRules);
            }
            return rules;
        }
    }
}
