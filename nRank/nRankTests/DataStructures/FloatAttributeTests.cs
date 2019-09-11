using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using nRank.PairwiseDRSA;
using NSubstitute;

namespace nRankTests.DataStructures
{
    [TestFixture]
    class FloatAttributeTests
    {
        [TestCase(1.1f, 0, AttributeType.Gain, true)]
        [TestCase(0, 1.1f, AttributeType.Gain, false)]
        [TestCase(1.1f, 1.1f, AttributeType.Gain, true)]
        [TestCase(1.1f, 0, AttributeType.Cost, false)]
        [TestCase(0, 1.1f, AttributeType.Cost, true)]
        [TestCase(1.1f, 1.1f, AttributeType.Cost, true)]
        public void TestRegularCases(float value1, float value2, AttributeType type, bool expectedResult)
        {
            new FloatPreferable(value1, type)
                            .IsWeaklyPreferedTo(new FloatPreferable(value2, type))
                            .ShouldBe(expectedResult);
        }

        [Test]
        public void TestWrongTypeException()
        {
            var strangeAtt = Substitute.For<IPreferable>();
            Should.Throw<InvalidOperationException>(() => new FloatPreferable(3.4f, AttributeType.Gain).IsWeaklyPreferedTo(strangeAtt));
        }

        [Test]
        public void TestIvalidComparisonException()
        {
            var strangeAtt = new FloatPreferable(5.4f, AttributeType.Cost);
            Should.Throw<InvalidOperationException>(() => new FloatPreferable(2.3f, AttributeType.Gain).IsWeaklyPreferedTo(strangeAtt));
        }

        [TestCase(4.5f, 1.3f, 3.2f, AttributeType.Cost)]
        [TestCase(8.2f, 16.4f, -8.2f, AttributeType.Gain)]
        public void TestFloatValueDifference(float value1, float value2, float result, AttributeType type)
        {
            var val1 = new FloatValue(value1, type);
            var val2 = new FloatValue(value2, type);
            var res = val1.DifferenceWith(val2);
            var resGoodType = res.ShouldBeAssignableTo<FloatPreferable>();
            resGoodType.Value.ShouldBe(result);
            resGoodType.Type.ShouldBe(type);
        }

        [Test]
        public void TestFloatValWrongTypeException()
        {
            var strangeVal = Substitute.For<INominalValue>();
            Should.Throw<InvalidOperationException>(() => new FloatValue(4.1f, AttributeType.Cost).DifferenceWith(strangeVal));
        }

        [Test]
        public void TestFloatValIvalidComparisonException()
        {
            var strangeVal = new FloatValue(2.4f, AttributeType.Gain);
            Should.Throw<InvalidOperationException>(() => new FloatValue(0.1f, AttributeType.Cost).DifferenceWith(strangeVal));
        }
    }
}
