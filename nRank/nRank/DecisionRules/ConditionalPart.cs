using System;
using System.Collections.Generic;

namespace nRank.DecisionRules
{
    class ConditionalPart : IConditionalPart
    {
        public ConditionalPart(string att, string oper, float value)
        {
            _att = att;
            _oper = oper;
            _value = value;
        }

        private string _att;
        private string _oper;
        private float _value;

        public override string ToString()
        {
            return $"(f({_att}, x) {_oper} {_value})";
        }

        public bool IsTrueFor(Dictionary<string, float> attributes)
        {
            var attValue = attributes[_att];
            switch (_oper)
            {
                case ">=": return attValue >= _value;
                case "<=": return attValue <= _value;
                default: throw new InvalidOperationException($"Operator '{_oper}' is not supported");
            }
        }

        public bool IsEmpty()
        {
            return false;
        }
    }
}
