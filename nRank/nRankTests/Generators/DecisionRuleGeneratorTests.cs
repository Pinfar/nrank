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
        public void TestRulesGenerationFromObjects()
        {
            TestPositiveRulesGeneration(CreatePCT);
        }

        [Test]
        public void TestRulesGenerationFromAtts()
        {
            TestPositiveRulesGeneration(CreatePCTAsAttList);
        }


        private void TestPositiveRulesGeneration(Func<InformationObject, InformationObject, PairwiseComparisonTable> createPCT)
        {
            var appGenerator = new LowerApproximationGeneratorVC(PairwiseComparisonTable.RelationType.Sc);
            var obj1 = CreateIO(1, 10);
            var obj2 = CreateIO(2, 8);
            PairwiseComparisonTable table = createPCT(obj1, obj2);
            var approximation = appGenerator.GetApproximation(table, 0.0f);
            var rulesGenerator = new DecisionRuleGenerator();
            var rules = rulesGenerator.GenerateRulesFrom(approximation, 0.0f).ToList();
            rules.Count.ShouldBe(1);
        }

        private static PairwiseComparisonTable CreatePCT(InformationObject obj1, InformationObject obj2)
        {
            PairwiseComparisonTable table = new PairwiseComparisonTable();
            table.Add(obj1, PairwiseComparisonTable.RelationType.S, obj2);
            table.Add(obj2, PairwiseComparisonTable.RelationType.Sc, obj1);
            return table;
        }

        private static PairwiseComparisonTable CreatePCTAsAttList(InformationObject obj1, InformationObject obj2)
        {
            PairwiseComparisonTable table = new PairwiseComparisonTable();
            var atts1 = obj1.NominalAttributes.Zip(obj2.NominalAttributes, (x, y) => new NominalAttributePair(x.Label,x.DifferenceWith(y))).ToList<IAttributePair>();
            table.Add(atts1, obj1.IntIdentifier, obj2.IntIdentifier, PairwiseComparisonTable.RelationType.S);
            var atts2 = obj2.NominalAttributes.Zip(obj1.NominalAttributes, (x, y) => new NominalAttributePair(x.Label, x.DifferenceWith(y))).ToList<IAttributePair>();
            table.Add(atts2, obj2.IntIdentifier, obj1.IntIdentifier, PairwiseComparisonTable.RelationType.Sc);
            return table;
        }

        private InformationObject CreateIO(int id,float value)
        {
            return new InformationObject(id, id.ToString(), new List<IAttribute> { new NominalAttribute("A", new FloatValue(value, AttributeType.Gain)) });
        }
    }
}
