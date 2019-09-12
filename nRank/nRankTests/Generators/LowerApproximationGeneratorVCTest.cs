using nRank.PairwiseDRSA;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRankTests.Generators
{
    [TestFixture]
    class LowerApproximationGeneratorVCTest
    {
        [Test]
        public void GenerateApproxTest()
        {
            var obj1 = CreateObject(1, 4);
            var obj2 = CreateObject(2, 8);
            var obj3 = CreateObject(3, 10);
            var obj4 = CreateObject(4, 11);
            var table = new InformationTable(new[] { obj1, obj2, obj3, obj4 });
            var generator = new LowerApproximationGeneratorVC(new PDominatingSetGenerator());
            var preferred = new[] { obj3.Pair(obj1) }.ToList();
            var notPreferred = new[] { obj3.Pair(obj2) }.ToList();
            var approx = generator.GetApproximation(preferred, notPreferred, table, 0.0f);
            approx.Approximation.ShouldBe(new[] { obj3.Pair(obj1) }, true);
            approx.PositiveRegion.ShouldBe(new[] { obj3.Pair(obj1), obj4.Pair(obj1) }, true);
        }

        [Test]
        public void GenerateApproxTestWithConsistency()
        {
            var obj1 = CreateObject(1, 4);
            var obj2 = CreateObject(2, 8);
            var obj3 = CreateObject(3, 10);
            var obj4 = CreateObject(4, 11);
            var table = new InformationTable(new[] { obj1, obj2, obj3, obj4 });
            var generator = new LowerApproximationGeneratorVC(new PDominatingSetGenerator());
            var preferred = new[] { obj3.Pair(obj1) }.ToList();
            var notPreferred = new[] { obj3.Pair(obj2), obj4.Pair(obj1) }.ToList();
            var approx = generator.GetApproximation(preferred, notPreferred, table, 0.5f);
            approx.Approximation.ShouldBe(new[] { obj3.Pair(obj1), obj4.Pair(obj1) }, true);
            approx.PositiveRegion.ShouldBe(new[] { obj3.Pair(obj1), obj4.Pair(obj1) }, true);
        }


        [Test]
        public void GenerateApproxNegativeTestWithConsistency()
        {
            var obj1 = CreateObject(1, 4);
            var obj2 = CreateObject(2, 8);
            var obj3 = CreateObject(3, 10);
            var obj4 = CreateObject(4, 11);
            var table = new InformationTable(new[] { obj1, obj2, obj3, obj4 });
            var generator = new LowerApproximationGeneratorVC(new PDominatedSetGenerator());
            var preferred = new[] { obj4.Pair(obj3), obj1.Pair(obj4) }.ToList();
            var notPreferred = new[] { obj1.Pair(obj3) }.ToList();
            var approx = generator.GetApproximation(notPreferred, preferred, table, 0.5f);
            approx.Approximation.ShouldBe(new[] { obj1.Pair(obj3), obj1.Pair(obj4) }, true);
            approx.PositiveRegion.ShouldBe(new[] { obj1.Pair(obj3), obj1.Pair(obj4) }, true);
        }

        [Test]
        public void GenerateApproxNegativeTest()
        {
            var obj1 = CreateObject(1, 4);
            var obj2 = CreateObject(2, 8);
            var obj3 = CreateObject(3, 10);
            var obj4 = CreateObject(4, 11);
            var table = new InformationTable(new[] { obj1, obj2, obj3, obj4 });
            var generator = new LowerApproximationGeneratorVC(new PDominatedSetGenerator());
            var preferred = new[] { obj4.Pair(obj3) }.ToList();
            var notPreferred = new[] { obj1.Pair(obj3) }.ToList();
            var approx = generator.GetApproximation(notPreferred, preferred, table, 0.0f);
            approx.Approximation.ShouldBe(new[] { obj1.Pair(obj3) }, true);
            approx.PositiveRegion.ShouldBe(new[] { obj1.Pair(obj3), obj1.Pair(obj4) }, true);
        }

        private InformationObject CreateObject(int id, float value)
        {
            return new InformationObject(id, id.ToString(), new List<IAttribute> { new NominalAttribute("Att1", new FloatValue(value, AttributeType.Gain)) });
        }
    }
}
