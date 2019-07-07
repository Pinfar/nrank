using nRank.Extensions;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.DataStructures
{
    [TestFixture]
    class TestInformationTable
    {
        [Test]
        public void TestGetSetMultipleAttributes()
        {
            var attributeNames = new[] { "attribute1", "attribute2" };
            var attributesValues = new[] { new[] { 1f, 2f, 3f, 4f }, new[] { 2f, 3f, 4f, 8f } };
            var isAttributeCost = attributeNames.ToDictionary(x => x, x => false);
            var table = new InformationTable(isAttributeCost, "noDecisionAttribute", false);
            foreach (var i in Enumerable.Range(0, 4))
            {
                var attributes = Enumerable.Range(0, 2).ToDictionary(x => attributeNames[x], x => attributesValues[x][i]);
                table.AddObject(i.ToString(), attributes, 0);
            }

            foreach (var i in Enumerable.Range(0, 2))
            {
                table.GetAttribute(attributeNames[i]).ShouldBe(attributesValues[i], 0.001);
            }
        }

        [Test]
        public void TestGetSetDecisionAttribute()
        {
            var attributeName = "decisionAttribute";
            var decisionAttributeValues = new[] { 5, 3, 26, 1 }.ToDictionary();
            var table = new InformationTable(new Dictionary<string, bool>(), attributeName, false);
            foreach(var objectData in decisionAttributeValues)
            {
                table.AddObject(objectData.Key, new Dictionary<string, float>(), objectData.Value);
            }
            table.GetDecisionAttribute().ShouldBe(decisionAttributeValues);
            table.DecisionAttributeName.ShouldBe(attributeName);
        }

        [Test]
        public void TestSetMultipleObjectWithTheSameKey()
        {
            var table = new InformationTable(new Dictionary<string, bool>(), "decisionAttribute", false);
            table.AddObject("1", new Dictionary<string, float>(), 2);
            Should.Throw<InvalidOperationException>(() =>
               table.AddObject("1", new Dictionary<string, float>(), 2)
            );
        }

        [Test]
        public void TestTableFiltering()
        {
            var attributeNames = new[] { "attribute1", "attribute2" };
            var attributesValues = new[] { new[] { 1f, 2f, 3f, 4f }, new[] { 2f, 3f, 4f, 8f } };
            var isAttributeCost = attributeNames.ToDictionary(x => x, x => false);
            var decisionAttributeName = "decisionAttribute";
            var decisionAttributeValues = new[] { 5, 3, 26, 1 };
            var table = new InformationTable(isAttributeCost, decisionAttributeName, false);
            foreach (var i in Enumerable.Range(0, 4))
            {
                var attributes = Enumerable.Range(0, 2).ToDictionary(x => attributeNames[x], x => attributesValues[x][i]);
                table.AddObject(i.ToString(), attributes, decisionAttributeValues[i]);
            }


            var filterPattern = new[] { true, false, true, false }.ToDictionary();
            var filteredAttributesValues = new[] { new[] { 1f, 3f }, new[] { 2f, 4f } };
            var filteredDecisionAttributeValues = new Dictionary<string, int>
            { {"0",5 }, {"2", 26 } };
            var filteredTable = table.Filter(filterPattern);

            foreach (var i in Enumerable.Range(0, 2))
            {
                filteredTable.GetAttribute(attributeNames[i]).ShouldBe(filteredAttributesValues[i], 0.001);
            }
            filteredTable.GetDecisionAttribute().ShouldBe(filteredDecisionAttributeValues);
            filteredTable.DecisionAttributeName.ShouldBe(decisionAttributeName);
        }

        [Test]
        public void TestGetDecisionAttributeOrderedClassesAsGain()
        {

            var attributeName = "decisionAttribute";
            var table = new InformationTable(new Dictionary<string, bool>(), attributeName, false);
            var decisionAttributeValues = new[] { 2, 1, 2, 4, 3, 2, 1 }.ToDictionary();
            foreach (var objectData in decisionAttributeValues)
            {
                table.AddObject(objectData.Key, new Dictionary<string, float>(), objectData.Value);
            }
            table.GetDecicionClassesWorstFirst().ShouldBe(new[] { 1, 2, 3, 4 });
        }

        [Test]
        public void TestGetDecisionAttributeOrderedClassesAsCost()
        {
            var attributeName = "decisionAttribute";
            var table = new InformationTable(new Dictionary<string, bool>(), attributeName, true);
            var decisionAttributeValues = new[] { 2, 1, 2, 4, 3, 2, 1 }.ToDictionary();
            foreach (var objectData in decisionAttributeValues)
            {
                table.AddObject(objectData.Key, new Dictionary<string, float>(), objectData.Value);
            }
            table.GetDecicionClassesWorstFirst().ShouldBe(new[] { 4, 3, 2, 1 });
        }

        [Test]
        public void TestOutrankingRelationOnCost()
        {
            var attributeName = "decisionAttribute";
            var table = new InformationTable(new Dictionary<string, bool> { {"att1", true } }, attributeName, true);
            var attributeValues = new[] { 1,2 }.ToDictionary();
            foreach (var attribute in attributeValues)
            {
                table.AddObject(attribute.Key, new Dictionary<string, float> { { "att1", attribute.Value } }, 1);
            }
            table.Outranks("0", "1").ShouldBeTrue();
            table.Outranks("1", "0").ShouldBeFalse();
        }

        [Test]
        public void TestOutrankingRelationOnGain()
        {
            var attributeName = "decisionAttribute";
            var table = new InformationTable(new Dictionary<string, bool> { { "att1", false } }, attributeName, true);
            var attributeValues = new[] { 1, 2 }.ToDictionary();
            foreach (var attribute in attributeValues)
            {
                table.AddObject(attribute.Key, new Dictionary<string, float> { { "att1", attribute.Value } }, 1);
            }
            table.Outranks("0", "1").ShouldBeFalse();
            table.Outranks("1", "0").ShouldBeTrue();
        }

        [Test]
        public void TestOutrankingRelationOnUncomparable()
        {
            var attributeName = "decisionAttribute";
            var table = new InformationTable(new Dictionary<string, bool> { { "att1", true }, { "att2", true } }, attributeName, true);
            var objects = new[]
            {
                new { Key = "0", Attributes = new Dictionary<string, float> { { "att1", 1 }, { "att2", 2 } } },
                new { Key = "1", Attributes = new Dictionary<string, float> { { "att1", 2 }, { "att2", 1 } } },
            };
            foreach (var obj in objects)
            {
                table.AddObject(obj.Key, obj.Attributes, 1);
            }
            table.Outranks("0", "1").ShouldBeFalse();
            table.Outranks("1", "0").ShouldBeFalse();
        }

        [Test]
        public void TestNegaion()
        {
            var attributeName = "decisionAttribute";
            var table = new InformationTable(new Dictionary<string, bool> { { "att1", true }, { "att2", true } }, attributeName, true);
            var objects = new[]
            {
                new { Key = "0", Attributes = new Dictionary<string, float> { { "att1", 1 }, { "att2", 2 } } },
                new { Key = "1", Attributes = new Dictionary<string, float> { { "att1", 2 }, { "att2", 1 } } },
            };
            foreach (var obj in objects)
            {
                table.AddObject(obj.Key, obj.Attributes, 1);
            }
            var derivatedTable = new InformationTable(new Dictionary<string, bool> { { "att1", true }, { "att2", true } }, attributeName, true);
            derivatedTable.AddObject(objects[0].Key, objects[0].Attributes, 1);

            var negatedTable = derivatedTable.Negation(table);

            negatedTable.GetAllObjectIdentifiers().ShouldBe(new[] { "1" });
        }

        [Test]
        public void TestCount()
        {
            var attributeName = "decisionAttribute";
            var table = new InformationTable(new Dictionary<string, bool> { { "att1", true }, { "att2", true } }, attributeName, true);
            var objects = new[]
            {
                new { Key = "0", Attributes = new Dictionary<string, float> { { "att1", 1 }, { "att2", 2 } } },
                new { Key = "1", Attributes = new Dictionary<string, float> { { "att1", 2 }, { "att2", 1 } } },
            };
            foreach (var obj in objects)
            {
                table.AddObject(obj.Key, obj.Attributes, 1);
            }

            table.Count().ShouldBe(2);
        }
    }
}
