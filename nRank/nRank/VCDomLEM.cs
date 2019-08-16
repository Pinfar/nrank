using nRank.ApproximationsGeneratorsVC;
using nRank.DataStructures;
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

        public IModel GenerateDecisionRules(IInformationTable informationTable, float consistencyLevel)
        {
            var rules = new List<IDecisionRule>();
            var approximations = approximationsGenerator.GetApproximations(informationTable, consistencyLevel);
            rules = approximations.SelectMany(approximation => GenerateRules(informationTable, consistencyLevel, approximation)).ToList();
            //foreach(var approximation in approximations)
            //{
            //    IEnumerable<IDecisionRule> minimalRules = GenerateRules(informationTable, consistencyLevel, approximation);
            //    rules.AddRange(minimalRules);
            //}
            return new TrainedModel(rules);
        }

        private IEnumerable<IDecisionRule> GenerateRules(IInformationTable informationTable, float consistencyLevel, IApproximation approximation)
        {
            var consistencyModifier = ((float)approximation.Union.InformationTable.Negation(informationTable).Count()) / approximation.ApproximatedInformationTable.Negation(informationTable).Count();
            var generatedRules = decisionRuleGenerator.GenerateRulesFrom(approximation, consistencyModifier * consistencyLevel);
            var minimalRules = GetMinimalRules(generatedRules);
            return minimalRules;
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
