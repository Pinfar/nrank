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
            while(notCoveredYet.GetAllObjectIdentifiers().Count()!=0)
            {
                var currentRule = ImmutableDecisionRule.GetAlwaysTrueRule(approximation);
                var objectsCoveredByCurrentRule = notCoveredYet;
                while(currentRule.IsEmpty() || !currentRule.SatisfiesConsistencyLevel(consistencyLevel) )
                {
                    var allPossibleRules = GetAllPossibleDecisionRules(objectsCoveredByCurrentRule, approximation);
                    var bestLocalRule = FindBestRule(currentRule, allPossibleRules, notCoveredYet, informationTable);

                    currentRule = currentRule.And(bestLocalRule);
                    objectsCoveredByCurrentRule = objectsCoveredByCurrentRule.Filter(currentRule);
                }

                currentRule = currentRule.CreateOptimizedRule(consistencyLevel);
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
                        .Select(y => new { AttributeName = x, AttributeValue = y } )
                );
            return attributes.SelectMany(
                x => approximation.AllowedOperators, 
                (x, y) => new ImmutableDecisionRule(x.AttributeName, y, x.AttributeValue, approximation)
            );
        }

        private IDecisionRule FindBestRule(IDecisionRule currentRule, IEnumerable<IDecisionRule> rules, IInformationTable notCoveredYet, IInformationTable informationTable)
        {
            var filteredRules = rules.Where(x => !currentRule.Contains(x)).ToList();
            var best = filteredRules.First();
            var notCovered = notCoveredYet.GetAllObjectIdentifiers();
            var bestScore = EvaluateRule(currentRule.And(best), notCovered, informationTable);
            foreach (var rule in filteredRules)
            {
                var ruleScore = EvaluateRule(currentRule.And(rule), notCovered, informationTable);
                if(ruleScore.Item1 > bestScore.Item1 || ruleScore.Item1 == bestScore.Item1 && ruleScore.Item2 > bestScore.Item2)
                {
                    best = rule;
                    bestScore = ruleScore;
                }
            }
            return best;
        }

        private Tuple<double, double> EvaluateRule(IDecisionRule rule, IEnumerable<string> notCoveredYet, IInformationTable informationTable)
        {
            var coveredByRule = informationTable.Filter(rule).GetAllObjectIdentifiers();
            var common = coveredByRule.Intersect(notCoveredYet);
            double commonCount = common.Count();
            double coveredByRuleCount = coveredByRule.Count();
            return Tuple.Create(commonCount / coveredByRuleCount, commonCount);
        }
    }
}
