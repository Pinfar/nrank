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
                    var bestLocalRule = new ImmutableDecisionRule(null, null, 0, null);


                    currentRule = currentRule.And(bestLocalRule);
                    objectsCoveredByCurrentRule = objectsCoveredByCurrentRule.Filter(currentRule);
                }


                rules.Add(currentRule);
                notCoveredYet = notCoveredYet.Filter(currentRule);
            }


            return rules;
        }
    }
}
