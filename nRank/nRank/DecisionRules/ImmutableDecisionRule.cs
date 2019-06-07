using nRank.Extensions;
using nRank.VCDomLEMAbstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.DecisionRules
{
    class ImmutableDecisionRule : IDecisionRule
    {
        List<IConditionalPart> _conditionalParts = new List<IConditionalPart>();
        string _approximation;

        public ImmutableDecisionRule(string attribute, string operatorStr, float value, string approximation) : this(approximation)
        {
            _conditionalParts.Add(new ConditionalPart(attribute, operatorStr, value));
            _approximation = approximation;
        }

        private ImmutableDecisionRule(ImmutableDecisionRule decisionRule)
        {
            _conditionalParts.AddRange(decisionRule._conditionalParts);
            _approximation = decisionRule._approximation;
        }

        private ImmutableDecisionRule(string approximation)
        {
            _approximation = approximation;
        }

        public static IDecisionRule GetAlwaysTrueRule(string approximation)
        {
            var rule = new ImmutableDecisionRule(approximation);
            rule._conditionalParts.Add(new AlwaysTruePart());
            return rule;
        }

        public override string ToString()
        {
            var conditions = string.Join(" and ", _conditionalParts);
            return $"if {conditions} then x E Cl{_approximation}";
        }

        public IDecisionRule And(string attribute, string operatorStr, float value)
        {
            var decisionRule = new ImmutableDecisionRule(this);
            decisionRule._conditionalParts.Add(new ConditionalPart(attribute, operatorStr, value));
            return decisionRule;
        }

        public IDecisionRule And(IDecisionRule decisionRule)
        {
            var newDecisionRule = new ImmutableDecisionRule(this);
            newDecisionRule._conditionalParts.AddRange(((ImmutableDecisionRule)decisionRule)._conditionalParts);
            return newDecisionRule;
        }

        public Dictionary<string, bool> Satisfy(IInformationTable informationTable)
        {
            var identifiers = informationTable.GetAllObjectIdentifiers();
            var result = new Dictionary<string, bool>();
            foreach (var identifier in identifiers)
            {
                var attributes = informationTable.GetObjectAttributes(identifier);
                result[identifier] = _conditionalParts.All(x => x.IsTrueFor(attributes));
            }
            return result;
        }

        public bool IsEmpty()
        {
            return _conditionalParts.All(x => x.IsEmpty());
        }

        public bool IsCreatingSubsetOf(IInformationTable source, IInformationTable target)
        {
            var currentCoverage = Satisfy(source).Where(x => x.Value).Select(x => x.Key);
            return currentCoverage.IsSubsetOf(target.GetAllObjectIdentifiers());
        }

        public IDecisionRule CreateOptimizedRule(IInformationTable source, IInformationTable target)
        {
            var resultList = new List<IConditionalPart>();
            var rule = new ImmutableDecisionRule(_approximation);
            rule._conditionalParts.AddRange(_conditionalParts);
            foreach (var part in _conditionalParts)
            {
                rule._conditionalParts.Remove(part);
                if(!rule.IsCreatingSubsetOf(source, target))
                {
                    rule._conditionalParts.Add(part);
                    resultList.Add(part);
                }
            }
            rule._conditionalParts = resultList;
            return rule;
        }
    }
}
