using nRank.DataStructures;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.UnionGenerators
{
    [TestFixture]
    class TestUpwardUnionGenerator
    {
        [Test]
        public void TestGenerateUnionsWithCostAttribute()
        {
            var attributeName = "Attribute1";
            var attributeValues = new[] { 1f, 2f, 3f, 4f, 5f, 6f };
            var decisionAttributeName = "Decision";
            var decisionAttributeValues = new[] { 1, 2, 3, 1, 2, 3 };

            var informationTable = new InformationTable();
            informationTable.AddAttribute(attributeName, attributeValues);
            informationTable.AddDecisionAttribute(decisionAttributeName, decisionAttributeValues, true);

            var generator = new UpwardUnionGenerator();
            var unions = generator.GenerateUnions(informationTable).ToList();

            unions.Count.ShouldBe(2);

            unions[0].GetAttribute(attributeName).ShouldBe(new[] { 1f, 2f, 4f, 5f });
            unions[0].GetDecisionAttribute().ShouldBe(new[] { 1, 2, 1, 2 });

            unions[1].GetAttribute(attributeName).ShouldBe(new[] { 1f, 4f });
            unions[1].GetDecisionAttribute().ShouldBe(new[] { 1, 1 });
        }

        [Test]
        public void TestGenerateUnionsWithGainAttribute()
        {
            var attributeName = "Attribute1";
            var attributeValues = new[] { 1f, 2f, 3f, 4f, 5f, 6f };
            var decisionAttributeName = "Decision";
            var decisionAttributeValues = new[] { 1, 2, 3, 1, 2, 3 };

            var informationTable = new InformationTable();
            informationTable.AddAttribute(attributeName, attributeValues);
            informationTable.AddDecisionAttribute(decisionAttributeName, decisionAttributeValues, false);

            var generator = new UpwardUnionGenerator();
            var unions = generator.GenerateUnions(informationTable).ToList();

            unions.Count.ShouldBe(2);

            unions[0].GetAttribute(attributeName).ShouldBe(new[] { 2f, 3f, 5f, 6f });
            unions[0].GetDecisionAttribute().ShouldBe(new[] { 2, 3, 2, 3 });

            unions[1].GetAttribute(attributeName).ShouldBe(new[] { 3f, 6f });
            unions[1].GetDecisionAttribute().ShouldBe(new[] { 3, 3 });
        }
    }
}
