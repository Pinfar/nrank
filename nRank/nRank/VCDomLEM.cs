using nRank.ApproximationsGenerators;
using nRank.ApproximationsGeneratorsVC;
using nRank.DecisionRulesGenerator;
using nRank.Extensions;
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
                var minimalRules = GetMinimalRules(generatedRules);
                rules.AddRange(minimalRules);
            }
            return rules;
        }

        private IEnumerable<IDecisionRule> GetMinimalRules(IEnumerable<IDecisionRule> generatedRules)
        {
            var minimalRules = generatedRules.ToList();
            foreach(var rule in generatedRules)
            {
                var covered = rule.GetCoveredItems().ToList();
                if(minimalRules.Any( x => x != rule && covered.IsSubsetOf(x.GetCoveredItems()) ))
                {
                    minimalRules.Remove(rule);
                }
            }
            return minimalRules;
        }
    }
}
