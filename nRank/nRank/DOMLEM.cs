using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank
{
    public class DOMLEM
    {
        readonly IRoughSetGenerator roughSetGenerator;
        readonly IDecisionRuleGenerator decisionRuleGenerator;

        internal DOMLEM(IRoughSetGenerator roughSetGenerator, IDecisionRuleGenerator decisionRuleGenerator)
        {
            this.roughSetGenerator = roughSetGenerator;
            this.decisionRuleGenerator = decisionRuleGenerator;
        }

        public ISet<IDecisionRule> GenerateDecisionRules(IInformationTable informationTable)
        {
            var rules = new HashSet<IDecisionRule>();
            var approximations = roughSetGenerator.GenerateLowerApproximations(informationTable);
            foreach(var approximation in approximations)
            {
                var generatedRules = decisionRuleGenerator.GenerateMinimalRulesFrom(approximation);
                rules.UnionWith(generatedRules);
            }

            return rules;
        }
    }
}
