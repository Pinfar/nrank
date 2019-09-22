using nRank;
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
    class PairVCDomLEMTest
    {
        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public void CheckIfRuleGeneratingWorks(bool parallelizeApproximationProcessing, bool parallelizeRuleEvaluation)
        {
            var domLem = new PairVCDomLEM(parallelizeApproximationProcessing, parallelizeRuleEvaluation);
            var obj1 = CreateIO(1, 10);
            var obj2 = CreateIO(2, 8);
            var obj3 = CreateIO(3, 7);
            PairwiseComparisonTable table = new PairwiseComparisonTable();
            table.Add(obj1, PairwiseComparisonTable.RelationType.S, obj2);
            table.Add(obj2, PairwiseComparisonTable.RelationType.Sc, obj1);
            table.Add(obj3, PairwiseComparisonTable.RelationType.Sc, obj2);
            InformationTable originalTable = new InformationTable(new[] { obj1, obj2, obj3 });
            var rules = domLem.GenerateDecisionRules(table, 0.4f);
            rules.Count.ShouldBe(2);
        }


        private InformationObject CreateIO(int id, float value)
        {
            return new InformationObject(id, id.ToString(), new List<IAttribute> { new NominalAttribute("A", new FloatValue(value, AttributeType.Gain)) });
        }
    }
}
