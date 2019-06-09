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
    class DecisionRuleGenerator
    {
        private readonly List<string> AllowedOperators;

        public DecisionRuleGenerator() : this("<=")
        {
        }

        public DecisionRuleGenerator(string operatorStr) : this(new List<string> { operatorStr })
        {
        }

        public DecisionRuleGenerator(IEnumerable<string> operatorStrs)
        {
            AllowedOperators = operatorStrs.ToList();
        }

        public IEnumerable<IDecisionRule> GenerateRulesFrom(IInformationTable approximation, IInformationTable informationTable, string approximationSymbol)
        {
            var notCoveredYet = approximation;
            var rules = new List<IDecisionRule>();
            while(notCoveredYet.GetAllObjectIdentifiers().Count()!=0)
            {
                var currentRule = ImmutableDecisionRule.GetAlwaysTrueRule(approximationSymbol);
                var objectsCoveredByCurrentRule = notCoveredYet;
                while(currentRule.IsEmpty() || !currentRule.IsCreatingSubsetOf(informationTable, approximation) )
                {
                    var allPossibleRules = GetAllPossibleDecisionRules(objectsCoveredByCurrentRule, approximationSymbol);
                    var bestLocalRule = FindBestRule(currentRule, allPossibleRules, notCoveredYet, informationTable);

                    currentRule = currentRule.And(bestLocalRule);
                    objectsCoveredByCurrentRule = objectsCoveredByCurrentRule.Filter(currentRule);
                }

                currentRule = currentRule.CreateOptimizedRule(informationTable, approximation);
                rules.Add(currentRule);
                var pattern = currentRule.Satisfy(notCoveredYet).ToDictionary(x => x.Key, x => !x.Value);
                notCoveredYet = notCoveredYet.Filter(pattern);
            }


            return rules;
        }

        private IEnumerable<IDecisionRule> GetAllPossibleDecisionRules(IInformationTable objectsCoveredByCurrentRule, string approximation)
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
                x => AllowedOperators, 
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
