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
    class DecisionRuleGeneratorTests
    {
        [Test]
        public void TestPositiveRulesGeneration()
        {
            var appGenerator = new LowerApproximationGeneratorVC(PairwiseComparisonTable.RelationType.Sc);
            var obj1 = CreateIO(1, 10);
            var obj2 = CreateIO(2, 8);
            PairwiseComparisonTable table = new PairwiseComparisonTable();
            table.Add(obj1, PairwiseComparisonTable.RelationType.S, obj2);
            table.Add(obj2, PairwiseComparisonTable.RelationType.Sc, obj1);
            InformationTable originalTable = new InformationTable(new[] { obj1, obj2 });
            var approximation = appGenerator.GetApproximation(table, originalTable, 0.0f);
            var rulesGenerator = new DecisionRuleGenerator();
            var rules = rulesGenerator.GenerateRulesFrom(approximation, 0.0f).ToList();
            rules.Count.ShouldBe(1);
        }

        private InformationObject CreateIO(int id,float value)
        {
            return new InformationObject(id, id.ToString(), new List<IAttribute> { new NominalAttribute("A", new FloatValue(value, AttributeType.Gain)) });
        }
    }
}
