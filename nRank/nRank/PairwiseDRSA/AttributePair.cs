using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    public class AttributePair
    {
        public AttributePair(IAttribute first, IAttribute second)
        {
            if (first.Label != second.Label) throw new InvalidOperationException("Can not create attribute pair from different attributes!");
            First = first;
            Second = second;
            if(first is OrdinalAttribute firstOrdinal && second is OrdinalAttribute secondOrdinal)
            {
                _isWeaklyPreferredFunc = GetIsWeaklyPreferredFuncOrdinal(firstOrdinal, secondOrdinal);
            }
            else if (first is NominalAttribute firstNominal && second is NominalAttribute secondNominal)
            {
                _isWeaklyPreferredFunc = GetIsWeaklyPreferredFuncNominal(firstNominal.DifferenceWith(secondNominal));
            }
            else
            {
                throw new NotImplementedException("This attribute type is not supported");
            }
        }

        public IAttribute First { get; }
        public IAttribute Second { get; }
        public string Label => First.Label;

        private Func<AttributePair, bool> _isWeaklyPreferredFunc;

        public override string ToString()
        {
            return $"{First.Label}: {First.StringValue} , {Second.StringValue}";
        }

        public bool IsWeaklyPreferredTo(AttributePair other)
        {
            return _isWeaklyPreferredFunc(other);
        }

        private Func<AttributePair, bool> GetIsWeaklyPreferredFuncOrdinal(OrdinalAttribute first, OrdinalAttribute second)
        {
            return x =>
            {
                var xFirst = (OrdinalAttribute)x.First;
                var xSecond = (OrdinalAttribute)x.Second;
                return first.IsWeaklyPreferedTo(xFirst) && xSecond.IsWeaklyPreferedTo(second);
            };
        }



        private Func<AttributePair, bool> GetIsWeaklyPreferredFuncNominal(IPreferable preferable)
        {
            return x =>
            {
                var xFirst = (NominalAttribute)x.First;
                var xSecond = (NominalAttribute)x.Second;
                var xPreferable = xFirst.DifferenceWith(xSecond);
                return preferable.IsWeaklyPreferedTo(xPreferable);
            };
        }
    }
}
