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
        readonly IUnionGenerator unionGenerator;
        readonly IDecisionRuleGenerator decisionRuleGenerator;
        readonly IApproximationsGenerator objectFilter;

        internal VCDomLEM(
            IUnionGenerator unionGenerator, 
            IDecisionRuleGenerator decisionRuleGenerator, 
            IApproximationsGenerator objectFilter
        )
        {
            this.unionGenerator = unionGenerator;
            this.decisionRuleGenerator = decisionRuleGenerator;
            this.objectFilter = objectFilter;
        }

        public ISet<IDecisionRule> GenerateDecisionRules(IInformationTable informationTable)
        {
            var rules = new HashSet<IDecisionRule>();
            var unions = unionGenerator.GenerateUnions(informationTable);
            foreach(var union in unions)
            {
                var allowedObjects = objectFilter.GetApproximation(union, informationTable);
                var generatedRules = decisionRuleGenerator.GenerateRulesFrom(allowedObjects);
                rules.UnionWith(generatedRules);
            }
            return rules;
        }
    }
}
