using nRank.DecisionRules;
using nRank.Extensions;
using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.DecisionRulesGenerator
{
    class DecisionRuleGenerator : IDecisionRuleGenerator
    {
        public IEnumerable<IDecisionRule> GenerateRulesFrom(IApproximation approximation, float consistencyLevel)
        {
            var informationTable = approximation.OriginalInformationTable;
            var notCoveredYet = approximation.ApproximatedInformationTable;
            var rules = new List<IDecisionRule>();
            while (notCoveredYet.GetAllObjectIdentifiers().Count() != 0)
            {
                var currentRule = ImmutableDecisionRule.GetAlwaysTrueRule(approximation);
                var objectsCoveredByCurrentRule = notCoveredYet;
                while (!currentRule.SatisfiesConsistencyLevel(consistencyLevel) || !informationTable.Filter(currentRule).GetAllObjectIdentifiers().IsSubsetOf(approximation.PositiveRegion))
                {
                    var allPossibleRules = GetAllPossibleDecisionRules(objectsCoveredByCurrentRule, approximation);
                    var bestLocalRule = FindBestRule(currentRule, allPossibleRules, approximation.ApproximatedInformationTable, informationTable);

                    currentRule = currentRule.And(bestLocalRule);
                    objectsCoveredByCurrentRule = objectsCoveredByCurrentRule.Filter(currentRule);
                }

                currentRule = currentRule.CreateOptimizedRule(consistencyLevel, approximation.ApproximatedInformationTable.GetAllObjectIdentifiers());
                rules.Add(currentRule);
                var pattern = currentRule.Satisfy(notCoveredYet).ToDictionary(x => x.Key, x => !x.Value);
                notCoveredYet = notCoveredYet.Filter(pattern);
            }


            return rules;
        }

        private IEnumerable<IDecisionRule> GetAllPossibleDecisionRules(IInformationTable objectsCoveredByCurrentRule, IApproximation approximation)
        {
            var attributes = objectsCoveredByCurrentRule
                .GetAllAttributes()
                .SelectMany(x =>
                    objectsCoveredByCurrentRule
                        .GetAttribute(x)
                        .Distinct()
                        .Select(y => new
                        {
                            AttributeName = x,
                            AttributeValue = y,
                            AttributeOperator = objectsCoveredByCurrentRule.IsAttributeCost(x) ? approximation.AllowedCostOperator : approximation.AllowedGainOperator
                        })
                );


            return attributes.Select(
                x => new ImmutableDecisionRule(x.AttributeName, x.AttributeOperator, x.AttributeValue, approximation)
            );
        }

        private IDecisionRule FindBestRule(IDecisionRule currentRule, IEnumerable<IDecisionRule> rules, IInformationTable notCoveredYet, IInformationTable informationTable)
        {
            var filteredRules = rules.Where(x => !currentRule.Contains(x)).ToList();
            var notCovered = new HashSet<string>(notCoveredYet.GetAllObjectIdentifiers());

            var best = filteredRules
                .Select(x => EvaluateRule(currentRule.And(x), notCovered, informationTable))
                .Aggregate((x, y) => x > y ? x : y);

            return best.Rule;
        }

        private RuleResult EvaluateRule(IDecisionRule rule, IEnumerable<string> notCoveredYet, IInformationTable informationTable)
        {
            var coveredByRule = rule.GetSatisfiedObjectsIdentifiers(informationTable).ToList();
            //var coveredByRule = informationTable.Filter(rule).GetAllObjectIdentifiers();
            var common = coveredByRule.Intersect(notCoveredYet);
            double commonCount = common.Count();
            double coveredByRuleCount = coveredByRule.Count;
            //return Tuple.Create(commonCount / coveredByRuleCount, commonCount);
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
