using nRank.PairwiseDRSA;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Relation = nRank.PairwiseDRSA.PairwiseComparisonTable.RelationType;

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
            var generator = new LowerApproximationGeneratorVC(Relation.S);
            var pct = new PairwiseComparisonTable();
            pct.Add(obj3, Relation.S, obj1);
            pct.Add(obj3, Relation.Sc, obj2);
            var approx = generator.GetApproximation(pct, 0.0f);
            approx.Approximation.ShouldBe(new[] { obj3.Pair(obj1) }, true);
            approx.PositiveRegion.ShouldBe(new[] { obj3.Pair(obj1) }, true);
        }

        [Test]
        public void GenerateApproxTestWithConsistency()
        {
            var obj1 = CreateObject(1, 4);
            var obj2 = CreateObject(2, 8);
            var obj3 = CreateObject(3, 10);
            var obj4 = CreateObject(4, 11);
            var table = new InformationTable(new[] { obj1, obj2, obj3, obj4 });
            var generator = new LowerApproximationGeneratorVC(Relation.S);
            var pct = new PairwiseComparisonTable();
            pct.Add(obj3, Relation.S, obj1);
            pct.Add(obj3, Relation.Sc, obj2);
            pct.Add(obj4, Relation.Sc, obj1);
            var approx = generator.GetApproximation(pct, 0.5f);
            approx.Approximation.ShouldBe(new[] { obj3.Pair(obj1) }, true);
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
            var generator = new LowerApproximationGeneratorVC(Relation.Sc);
            var pct = new PairwiseComparisonTable();
            pct.Add(obj4, Relation.S, obj3);
            pct.Add(obj1, Relation.S, obj4);
            pct.Add(obj1, Relation.Sc, obj3);
            var approx = generator.GetApproximation(pct, 0.5f);
            approx.Approximation.ShouldBe(new[] { obj1.Pair(obj3) }, true);
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
            var generator = new LowerApproximationGeneratorVC(Relation.Sc);
            var pct = new PairwiseComparisonTable();
            pct.Add(obj4, Relation.S, obj3);
            pct.Add(obj1, Relation.Sc, obj3);
            var approx = generator.GetApproximation(pct, 0.0f);
            approx.Approximation.ShouldBe(new[] { obj1.Pair(obj3) }, true);
            approx.PositiveRegion.ShouldBe(new[] { obj1.Pair(obj3) }, true);
        }

        private InformationObject CreateObject(int id, float value)
        {
            return new InformationObject(id, id.ToString(), new List<IAttribute> { new NominalAttribute("Att1", new FloatValue(value, AttributeType.Gain)) });
        }
    }
}
