using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nRank.PairwiseDRSA;
using Shouldly;

namespace nRankTests.DataStructures
{
    [TestFixture]
    public class PairwiseComparisonTableTests
    {
        [Test]
        public void DisplayEmptyTableTest()
        {
            new PairwiseComparisonTable()
                .ToDisplayableTable()
                .ShouldBe(new[]
                {
                    "(PCT is Empty)"
                });
        }

        [Test]
        public void DisplayTableTest()
        {
            var pct = new PairwiseComparisonTable();
            List<IAttributePair> attributes = new List<IAttributePair>
            {
                new NominalAttributePair("Att1", new IntPreferable(2, AttributeType.Gain)),
                new NominalAttributePair("Att2", new IntPreferable(4, AttributeType.Gain))
            };
            pct.Add(attributes, 1, 2, PairwiseComparisonTable.RelationType.S);

            pct.ToDisplayableTable()
                .ShouldBe(new[]
                {
                    "ID, Pair, Att1, Att2, Relation",
                    "1, {1, 2}, 2, 4, S"
                });
        }
    }
}
