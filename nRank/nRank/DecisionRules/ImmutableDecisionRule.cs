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
        IApproximation _approximation;

        public float Accuracy => CalculateAccuracy();

        public ImmutableDecisionRule(string attribute, string operatorStr, float value, IApproximation approximation) : this(approximation)
        {
            _conditionalParts.Add(new ConditionalPart(attribute, operatorStr, value));
        }

        private ImmutableDecisionRule(ImmutableDecisionRule decisionRule)
        {
            _conditionalParts.AddRange(decisionRule._conditionalParts);
            _approximation = decisionRule._approximation;
        }

        private ImmutableDecisionRule(IApproximation approximation)
        {
            _approximation = approximation;
        }

        public static IDecisionRule GetAlwaysTrueRule(IApproximation approximation)
        {
            var rule = new ImmutableDecisionRule(approximation);
            rule._conditionalParts.Add(new AlwaysTruePart());
            return rule;
        }

        public override string ToString()
        {
            var conditions = string.Join(" and ", _conditionalParts);
            return $"if {conditions} then x E {_approximation.Symbol}";
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
                result[identifier] = _conditionalParts.Count > 0 && _conditionalParts.All(x => x.IsTrueFor(attributes));
            }
            return result;
        }

        public bool IsEmpty()
        {
            return _conditionalParts.All(x => x.IsEmpty());
        }

        public bool SatisfiesConsistencyLevel(float consistencyLevel)
        {
            return Accuracy >= consistencyLevel;
        }

        public IDecisionRule CreateOptimizedRule(float consistencyLevel)
        {
            var resultList = new List<IConditionalPart>();
            var rule = new ImmutableDecisionRule(_approximation);
            rule._conditionalParts.AddRange(_conditionalParts);
            foreach (var part in _conditionalParts)
            {
                rule._conditionalParts.Remove(part);
                if(!rule.SatisfiesConsistencyLevel(consistencyLevel))
                {
                    rule._conditionalParts.Add(part);
                    resultList.Add(part);
                }
            }
            rule._conditionalParts = resultList;
            return rule;
        }

        public bool Contains(IDecisionRule rule)
        {
            if (!(rule is ImmutableDecisionRule)) return false;
            var immutableRule = (ImmutableDecisionRule)rule;
            var selfRules = _conditionalParts.Select(x => x.ToString()).ToList();
            return immutableRule._conditionalParts.All(x => selfRules.Contains(x.ToString()));
        }

        public IEnumerable<string> GetCoveredItems()
        {
            return _approximation.OriginalInformationTable.Filter(this).GetAllObjectIdentifiers();
        }

        private float CalculateAccuracy()
        {
            var currentCoverage = Satisfy(_approximation.OriginalInformationTable).Where(x => x.Value).Select(x => x.Key).ToList();
            var union = _approximation
                .OriginalInformationTable
                .GetDecisionAttribute()
                .Where(x => _approximation.Classes.Contains(x.Value))
                .Select(x => x.Key);
            float commonPart = currentCoverage.Intersect(union).Count();
            float dsetCount = currentCoverage.Count();
            if (dsetCount > 0)
            {
                return commonPart / dsetCount;
            }
            else
            {
                return 0;
            }
        }
    }
}
