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
    class IntAttributeTests
    {
        [TestCase(1, 0, AttributeType.Gain, true)]
        [TestCase(0, 1, AttributeType.Gain, false)]
        [TestCase(1, 1, AttributeType.Gain, true)]
        [TestCase(1, 0, AttributeType.Cost, false)]
        [TestCase(0, 1, AttributeType.Cost, true)]
        [TestCase(1, 1, AttributeType.Cost, true)]
        public void TestRegularCases(int value1, int value2, AttributeType type, bool expectedResult)
        {
            new IntPreferable(value1, type)
                            .IsWeaklyPreferedTo(new IntPreferable(value2, type))
                            .ShouldBe(expectedResult);
        }

        [Test]
        public void TestWrongTypeException()
        {
            var strangeAtt = Substitute.For<IPreferable>();
            Should.Throw<InvalidOperationException>(() => new IntPreferable(1, AttributeType.Gain).IsWeaklyPreferedTo(strangeAtt));
        }

        [Test]
        public void TestIvalidComparisonException()
        {
            var strangeAtt = new IntPreferable(1, AttributeType.Cost);
            Should.Throw<InvalidOperationException>(() => new IntPreferable(1, AttributeType.Gain).IsWeaklyPreferedTo(strangeAtt));
        }

        [TestCase(4, 1, 3, AttributeType.Cost)]
        [TestCase(8, 16, -8, AttributeType.Gain)]
        public void TestIntValueDifference(int value1, int value2, int result, AttributeType type)
        {
            var val1 = new IntValue(value1, type);
            var val2 = new IntValue(value2, type);
            var res = val1.DifferenceWith(val2);
            var resGoodType = res.ShouldBeAssignableTo<IntPreferable>();
            resGoodType.Value.ShouldBe(result);
            resGoodType.Type.ShouldBe(type);
        }

        [Test]
        public void TestIntValWrongTypeException()
        {
            var strangeVal = Substitute.For<INominalValue>();
            Should.Throw<InvalidOperationException>(() => new IntValue(1, AttributeType.Cost).DifferenceWith(strangeVal));
        }

        [Test]
        public void TestIntValIvalidComparisonException()
        {
            var strangeVal = new IntValue(2, AttributeType.Gain);
            Should.Throw<InvalidOperationException>(() => new IntValue(1, AttributeType.Cost).DifferenceWith(strangeVal));
        }
    }
}
