using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nRank.Extensions;
using nRank.PairwiseDRSA;

namespace nRank
{
    public class PairVCDomLEM
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

        public List<IDecisionRule> GenerateDecisionRules(PairwiseComparisonTable pairwiseComparisonTable, float consistencyLevel)
        {
            var rules = new List<IDecisionRule>();
            if (parallelizeApproximationProcessing)
            {
                rules = approximationsGenerators
                    .AsParallel()
                    .Select( x => x.GetApproximation(pairwiseComparisonTable, consistencyLevel))
                    .SelectMany(x => decisionRuleGenerator.GenerateRulesFrom(x, consistencyLevel))
                    .ToList();
            }
            else
            {
                rules = approximationsGenerators
                    .Select(x => x.GetApproximation(pairwiseComparisonTable, consistencyLevel))
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

        public List<string> GetDebugData(PairwiseComparisonTable pct, float consistencyLevel)
        {
            var result = new List<string>();
            result.Add("[P-dominating sets]");
            var pdominatingGen = new PDominatingSetGenerator();
            var dominatingLines = pct
                .AsInformationObjectPairs()
                .Select(x => $"{x.Id} : {string.Join(" ", pdominatingGen.Generate(pct, x).Select(y => y.Id))} ");
            result.AddRange(dominatingLines);

            result.Add("");
            result.Add("[P-dominated sets]");
            var pdominatedGen = new PDominatedSetGenerator();
            var dominatedLines = pct
                .AsInformationObjectPairs()
                .Select(x => $"{x.Id} : {string.Join(" ", pdominatedGen.Generate(pct, x).Select(y => y.Id))} ");
            result.AddRange(dominatedLines);

            result.Add("");
            result.Add("[Sc-lower approximation]");
            var scApproxGenerator = new LowerApproximationGeneratorVC(PairwiseComparisonTable.RelationType.Sc);
            var scapprox = scApproxGenerator.GetApproximation(pct, consistencyLevel);
            result.Add(string.Join(" ", scapprox.Approximation.Select(x => x.Id).OrderBy(x => int.Parse(x))));

            result.Add("");
            result.Add("[S-lower approximation]");
            var sApproxGenerator = new LowerApproximationGeneratorVC(PairwiseComparisonTable.RelationType.S);
            var sapprox = sApproxGenerator.GetApproximation(pct, consistencyLevel);
            result.Add(string.Join(" ", sapprox.Approximation.Select(x => x.Id).OrderBy(x => int.Parse(x))));


            return result;

        }
    }
}
