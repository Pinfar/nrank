using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class ImmutableDecisionRule : IDecisionRule
    {
        List<IConditionalPart> _conditionalParts = new List<IConditionalPart>();
        PApproximation _approximation;

        public float Accuracy => CalculateAccuracyEpsilon();
        

        public ImmutableDecisionRule(AttributePair x, PApproximation approximation) : this(approximation)
        {
            _conditionalParts.Add(new ConditionalPart(x, approximation.Relation));
        }

        private ImmutableDecisionRule(ImmutableDecisionRule decisionRule)
        {
            _conditionalParts.AddRange(decisionRule._conditionalParts);
            _approximation = decisionRule._approximation;
        }

        private ImmutableDecisionRule(PApproximation approximation)
        {
            _approximation = approximation;
        }

        public static IDecisionRule GetAlwaysTrueRule(PApproximation approximation)
        {
            var rule = new ImmutableDecisionRule(approximation);
            rule._conditionalParts.Add(new AlwaysTruePart());
            return rule;
        }

        public override string ToString()
        {
            var conditions = string.Join(" and ", _conditionalParts);
            return $"if {conditions} then x {_approximation.Relation.ToString("G")} y";
        }

        public IDecisionRule And(IDecisionRule decisionRule)
        {
            var newDecisionRule = new ImmutableDecisionRule(this);
            newDecisionRule._conditionalParts.AddRange(((ImmutableDecisionRule)decisionRule)._conditionalParts);
            return newDecisionRule;
        }

        public bool IsEmpty()
        {
            return _conditionalParts.All(x => x.IsEmpty());
        }

        public bool SatisfiesConsistencyLevel(float consistencyLevel)
        {
            return Accuracy <= consistencyLevel;
        }

        public IDecisionRule CreateOptimizedRule(float consistencyLevel, List<InformationObjectPair> notCoveredYet)
        {
            var resultList = new List<IConditionalPart>();
            var rule = new ImmutableDecisionRule(_approximation);
            rule._conditionalParts.AddRange(_conditionalParts);

            var coveredByRule = _approximation.OriginalTable.Filter(rule.AsFunc()).AsInformationObjectPairs();
            var common = coveredByRule.Intersect(notCoveredYet);
            double commonCount = common.Count();

            foreach (var part in _conditionalParts)
            {
                rule._conditionalParts.Remove(part);
                var coveredByRulePartial = _approximation.OriginalTable.Filter(rule.AsFunc()).AsInformationObjectPairs();
                var commonPartial = coveredByRule.Intersect(notCoveredYet);
                double commonCountPartial = common.Count();
                if (!rule.SatisfiesConsistencyLevel(consistencyLevel) || commonCountPartial < commonCount)
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
            var selfRules = new HashSet<string>(_conditionalParts.Select(x => x.ToString()));
            return immutableRule._conditionalParts.All(x => selfRules.Contains(x.ToString()));
        }
        

        private float CalculateAccuracyEpsilon()
        {
            var currentCoverage = _approximation.OriginalTable.Filter(AsFunc()).AsInformationObjectPairs();
            var negSet = _approximation.NegativeApproximation;
            float commonPart = currentCoverage.Intersect(negSet).Count();
            float negSetCount = negSet.Count;
            return commonPart / negSetCount;

        }

        public string ToLatexString()
        {
            var conditions = string.Join(" and ", _conditionalParts.Select(x => x.ToLatexString()));
            return $"if {conditions} then  \\(x \\in {GetLatexSymbol()} \\)";
        }

        private string GetLatexSymbol()
        {
            return _approximation.Relation.ToString("G");
        }

        public Func<PairwiseComparisonTable.PairwiseComparisonTableEntry, bool> AsFunc()
        {
            return x => AsListFilterFunc()(x.ObjectPair);
        }

        public Func<InformationObjectPair, bool> AsListFilterFunc()
        {
            return x => _conditionalParts.All(y => y.IsTrueFor(x));
        }
    }
}
