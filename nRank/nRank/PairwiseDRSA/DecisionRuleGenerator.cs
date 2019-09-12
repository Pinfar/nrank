using nRank.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class DecisionRuleGenerator
    {
        private delegate RuleResult BestRuleGenerator(IDecisionRule currentRule, PairwiseComparisonTable informationTable, List<IDecisionRule> filteredRules, HashSet<InformationObjectPair> notCovered);

        BestRuleGenerator bestRuleGenerator;

        public DecisionRuleGenerator(bool executeInParallel = false)
        {
            bestRuleGenerator = executeInParallel ? (BestRuleGenerator)FindBestRuleParallel : FindBestRuleSequential;
        }

        public IEnumerable<IDecisionRule> GenerateRulesFrom(PApproximation approximation, float consistencyLevel)
        {
            var originalPCT = approximation.OriginalTable;
            var notCoveredYet = approximation.Approximation;
            var rules = new List<IDecisionRule>();
            while (notCoveredYet.Count != 0)
            {
                IDecisionRule currentRule = ImmutableDecisionRule.GetAlwaysTrueRule(approximation);
                var objectsCoveredByCurrentRule = notCoveredYet;
                while (!currentRule.SatisfiesConsistencyLevel(consistencyLevel) || !originalPCT.Filter(currentRule.AsFunc()).AsInformationObjectPairs().IsSubsetOf(approximation.PositiveRegion))
                {
                    var allPossibleRules = GetAllPossibleDecisionRules(objectsCoveredByCurrentRule, approximation);
                    var bestLocalRule = FindBestRule(currentRule, allPossibleRules, approximation.Approximation, originalPCT);

                    currentRule = currentRule.And(bestLocalRule);
                    objectsCoveredByCurrentRule = objectsCoveredByCurrentRule.Where(currentRule.AsListFilterFunc()).ToList();
                }

                currentRule = currentRule.CreateOptimizedRule(consistencyLevel, approximation.Approximation);
                rules.Add(currentRule);
                var filterFunc = currentRule.AsListFilterFunc();
                notCoveredYet = notCoveredYet.Where(x => !filterFunc(x)).ToList();
            }


            return rules;
        }

        private IEnumerable<IDecisionRule> GetAllPossibleDecisionRules(List<InformationObjectPair> objectsCoveredByCurrentRule, PApproximation approximation)
        {
            var attributes = objectsCoveredByCurrentRule.SelectMany(x => x.GetAttributes());


            return attributes.Select(
                x => new ImmutableDecisionRule(x, approximation)
            );
        }

        private IDecisionRule FindBestRule(IDecisionRule currentRule, IEnumerable<IDecisionRule> rules, List<InformationObjectPair> notCoveredYet, PairwiseComparisonTable informationTable)
        {
            var filteredRules = rules.Where(x => !currentRule.Contains(x)).ToList();
            var notCovered = new HashSet<InformationObjectPair>(notCoveredYet);
            RuleResult best = bestRuleGenerator(currentRule, informationTable, filteredRules, notCovered);

            return best.Rule;
        }

        private RuleResult FindBestRuleParallel(IDecisionRule currentRule, PairwiseComparisonTable informationTable, List<IDecisionRule> filteredRules, HashSet<InformationObjectPair> notCovered)
        {
            return filteredRules
                .AsParallel()
                .Select(x => EvaluateRule(currentRule.And(x), notCovered, informationTable))
                .Aggregate((x, y) => x > y ? x : y);
        }

        private RuleResult FindBestRuleSequential(IDecisionRule currentRule, PairwiseComparisonTable informationTable, List<IDecisionRule> filteredRules, HashSet<InformationObjectPair> notCovered)
        {
            return filteredRules
                .Select(x => EvaluateRule(currentRule.And(x), notCovered, informationTable))
                .Aggregate((x, y) => x > y ? x : y);
        }

        private RuleResult EvaluateRule(IDecisionRule rule, IEnumerable<InformationObjectPair> notCoveredYet, PairwiseComparisonTable informationTable)
        {
            var coveredByRule = informationTable.Filter(rule.AsFunc()).AsInformationObjectPairs();
            var common = coveredByRule.Intersect(notCoveredYet);
            double commonCount = common.Count();
            return new RuleResult(rule, rule.Accuracy, commonCount);
        }

        private struct RuleResult
        {
            public RuleResult(IDecisionRule rule, double accuracy, double commonCount) : this()
            {
                Rule = rule;
                Accuracy = accuracy;
                CommonCount = commonCount;
            }

            public IDecisionRule Rule { get; }
            public double Accuracy { get; }
            public double CommonCount { get; }

            public static bool operator >(RuleResult rule1, RuleResult rule2)
            {
                return rule1.Accuracy < rule2.Accuracy || rule1.Accuracy == rule2.Accuracy && rule1.CommonCount > rule2.CommonCount;
            }

            public static bool operator <(RuleResult rule1, RuleResult rule2)
            {
                throw new NotImplementedException();
            }
        }
    }
}
