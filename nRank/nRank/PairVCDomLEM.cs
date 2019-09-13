using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nRank.Extensions;
using nRank.PairwiseDRSA;

namespace nRank
{
    class PairVCDomLEM
    {
        readonly DecisionRuleGenerator decisionRuleGenerator;
        readonly IEnumerable<LowerApproximationGeneratorVC> approximationsGenerators;
        readonly bool parallelizeApproximationProcessing;

        public PairVCDomLEM(bool parallelizeApproximationProcessing = true, bool parallelizeRuleEvaluation = false)
        {
            this.parallelizeApproximationProcessing = parallelizeApproximationProcessing;
            decisionRuleGenerator = new DecisionRuleGenerator(parallelizeRuleEvaluation);
            approximationsGenerators = new[]
            {
                new LowerApproximationGeneratorVC(PairwiseComparisonTable.RelationType.S),
                new LowerApproximationGeneratorVC(PairwiseComparisonTable.RelationType.Sc)
            };
        }

        public List<IDecisionRule> GenerateDecisionRules(InformationTable informationTable, PairwiseComparisonTable pairwiseComparisonTable, float consistencyLevel)
        {
            var rules = new List<IDecisionRule>();
            if (parallelizeApproximationProcessing)
            {
                rules = approximationsGenerators
                    .AsParallel()
                    .Select( x => x.GetApproximation(pairwiseComparisonTable, informationTable, consistencyLevel))
                    .SelectMany(x => decisionRuleGenerator.GenerateRulesFrom(x, consistencyLevel))
                    .ToList();
            }
            else
            {
                rules = approximationsGenerators
                    .Select(x => x.GetApproximation(pairwiseComparisonTable, informationTable, consistencyLevel))
                    .SelectMany(x => decisionRuleGenerator.GenerateRulesFrom(x, consistencyLevel))
                    .ToList();
            }
            return GetMinimalRules(rules, pairwiseComparisonTable).ToList();
        }

        private IEnumerable<IDecisionRule> GetMinimalRules(IEnumerable<IDecisionRule> generatedRules, PairwiseComparisonTable pairwiseComparisonTable)
        {
            var minimalRules = generatedRules.ToList();
            foreach (var rule in generatedRules)
            {
                var covered = pairwiseComparisonTable.Filter(rule.AsFunc()).AsInformationObjectPairs();
                if (minimalRules.Any(x => x != rule && covered.IsSubsetOf(pairwiseComparisonTable.Filter(x.AsFunc()).AsInformationObjectPairs())))
                {
                    minimalRules.Remove(rule);
                }
            }
            return minimalRules;
        }
    }
}
