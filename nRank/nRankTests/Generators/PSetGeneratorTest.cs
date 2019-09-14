using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nRank.PairwiseDRSA;
using NUnit.Framework;
using Shouldly;

namespace nRankTests.Generators
{
    [TestFixture]
    class PSetGeneratorTest
    {
        [Test]
        public void TestGenerateDominatingSet()
        {
            var obj1 = CreateObject(1,4);
            var obj2 = CreateObject(2,8);
            var obj3 = CreateObject(3,10);
            //var table = new InformationTable(new[] { obj1, obj2, obj3 });
            var generator = new PDominatingSetGenerator();
            var table = new PairwiseComparisonTable();
            table.Add(obj2, PairwiseComparisonTable.RelationType.S, obj1);
            table.Add(obj3, PairwiseComparisonTable.RelationType.S, obj2);
            table.Add(obj3, PairwiseComparisonTable.RelationType.S, obj1);
            var pdset = generator.Generate(table, obj2.Pair(obj1));
            pdset.ShouldBe(new[] { obj2.Pair(obj1), obj3.Pair(obj1) }, true);
        }

        [Test]
        public void TestGenerateDominatedSet()
        {
            var obj1 = CreateObject(1, 4);
            var obj2 = CreateObject(2, 8);
            var obj3 = CreateObject(3, 10);
            var generator = new PDominatedSetGenerator();
            var table = new PairwiseComparisonTable();
            table.Add(obj2, PairwiseComparisonTable.RelationType.S, obj1);
            table.Add(obj3, PairwiseComparisonTable.RelationType.S, obj2);
            table.Add(obj3, PairwiseComparisonTable.RelationType.S, obj1);
            var pdset = generator.Generate(table, obj2.Pair(obj1));

            pdset.ShouldBe(new[] { obj2.Pair(obj1), obj3.Pair(obj2)  }, true);
        }

        private InformationObject CreateObject(int id, float value)
        {
            return new InformationObject(id, id.ToString(), new List<IAttribute> { new NominalAttribute("Att1", new FloatValue(value, AttributeType.Gain)) });
        }
    }
}
