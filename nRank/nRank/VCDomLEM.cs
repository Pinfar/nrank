using nRank.ApproximationsGenerators;
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
            approximationsGenerator = new ApproximationsGenerator();
        }

        public List<IDecisionRule> GenerateDecisionRules(IInformationTable informationTable)
        {
            var rules = new List<IDecisionRule>();
            var approximations = approximationsGenerator.GetApproximations(informationTable);
            foreach(var approximation in approximations)
            {
                var generatedRules = decisionRuleGenerator.GenerateRulesFrom(approximation);
                rules.AddRange(generatedRules);
            }
            return rules;
        }
    }
}
