using nRank.PairwiseDRSA;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRankTests.DataStructures
{
    [TestFixture]
    class AttributePairTests
    {
        [Test]
        public void OrdinalPreferenceRelationIsTrue()
        {
            var att1 = CreateAttA(3);
            var att2 = CreateAttA(2);
            var att3 = CreateAttA(1);
            var att4 = CreateAttA(0);
            new OrdinalAttributePair(att1, att4)
                .IsWeaklyPreferredTo(new OrdinalAttributePair(att2, att3))
                .ShouldBeTrue();
        }

        [Test]
        public void OrdinalPreferenceRelationIsFalse()
        {
            var att1 = CreateAttA(3);
            var att2 = CreateAttA(2);
            var att3 = CreateAttA(1);
            var att4 = CreateAttA(0);
            new OrdinalAttributePair(att1, att2)
                .IsWeaklyPreferredTo(new OrdinalAttributePair(att3, att4))
                .ShouldBeFalse();
        }

        private static OrdinalAttribute CreateAttA(int i)
        {
            List<string> preferenceOrder = new List<string> { "bad", "good", "better", "best" };
            return new OrdinalAttribute("A", preferenceOrder[i], preferenceOrder);
        }


        [Test]
        public void NominalPreferenceRelationIsTrue()
        {
            var att1 = CreateFloatAttA(1.5f);
            var att2 = CreateFloatAttA(2.5f);
            var att3 = CreateFloatAttA(3.5f);
            var att4 = CreateFloatAttA(4.5f);
            new NominalAttributePair(att1.Label, att1.DifferenceWith(att4))
                .IsWeaklyPreferredTo(new NominalAttributePair(att2.Label, att2.DifferenceWith(att3)))
                .ShouldBeTrue();
        }

        [Test]
        public void NominalPreferenceRelationIsFalse()
        {
            var att1 = CreateFloatAttA(1.5f);
            var att2 = CreateFloatAttA(2.5f);
            var att3 = CreateFloatAttA(3.5f);
            var att4 = CreateFloatAttA(4.5f);
            new NominalAttributePair(att1.Label, att1.DifferenceWith(att2))
                .IsWeaklyPreferredTo(new NominalAttributePair(att2.Label, att2.DifferenceWith(att4)))
                .ShouldBeFalse();
        }

        private NominalAttribute CreateFloatAttA(float value)
        {
            return new NominalAttribute("A", new FloatValue(value, AttributeType.Cost));
        }
    }
}
