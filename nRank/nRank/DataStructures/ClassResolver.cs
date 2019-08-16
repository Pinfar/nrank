using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.DataStructures
{
    class ClassResolver
    {
        readonly Dictionary<int, float> _internalTable;
        readonly float _worstPossibleValue;

        public ClassResolver(IDecisionRule rule)
        {
            _worstPossibleValue = 1 - rule.Accuracy;
            _internalTable = rule.Classes.ToDictionary(x => x, x => rule.Accuracy);
        }

        private ClassResolver(Dictionary<int, float> internalTable, float worstPossibleValue)
        {
            _internalTable = internalTable;
            _worstPossibleValue = worstPossibleValue;
        }

        public int GetMostPossibleClass()
        {
            return _internalTable.OrderBy(x => x.Value).First().Key;
        }

        public static ClassResolver operator +(ClassResolver first, ClassResolver second)
        {
            var worstPossibleValue = first._worstPossibleValue + second._worstPossibleValue;
            var allKeys = first._internalTable.Keys.Union(second._internalTable.Keys);
            var internalTable = allKeys.ToDictionary(x => x, x => first.GetValueOfDefault(x) + second.GetValueOfDefault(x));
            return new ClassResolver(internalTable, worstPossibleValue);
        }

        private float GetValueOfDefault(int key)
        {
            if (_internalTable.TryGetValue(key, out float firstValue))
            {
                return firstValue;
            }
            return _worstPossibleValue;
        }

    }
}
