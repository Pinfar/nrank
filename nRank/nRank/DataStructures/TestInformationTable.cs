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
            var table = new InformationTable();
            var attributeNames = new[] { "attribute1", "attribute2" };
            var attributesValues = new[] { new[] { 1f, 2f, 3f, 4f }, new[] { 2f, 3f, 4f, 8f } };
            foreach (var i in Enumerable.Range(0, 2))
            {
                table.AddAttribute(attributeNames[i], attributesValues[i]);
            }

            foreach (var i in Enumerable.Range(0, 2))
            {
                table.GetAttribute(attributeNames[i]).ShouldBe(attributesValues[i], 0.001);
            }
        }

        [Test]
        public void TestGetSetDecisionAttribute()
        {
            var table = new InformationTable();
            var attributeName = "decisionAttribute";
            var decisionAttributeValues = new[] { 5, 3, 26, 1 };
            table.AddDecisionAttribute(attributeName, decisionAttributeValues);
            table.GetDecisionAttribute().ShouldBe(decisionAttributeValues);
            table.DecisionAttributeName.ShouldBe(attributeName);
        }

        [Test]
        public void TestSetMultipleDecisionAttribute()
        {
            var table = new InformationTable();
            var attributeName = "decisionAttribute";
            var decisionAttributeValues = new[] { 5, 3, 26, 1 };
            table.AddDecisionAttribute(attributeName, decisionAttributeValues);
            var secondDecisionAttributeName = "secondDecisionAttribute";
            var secondDecisionAttributeValues = new[] { 5, 3, 26, 1 };
            Should.Throw<InvalidOperationException>(() =>
                table.AddDecisionAttribute(secondDecisionAttributeName, secondDecisionAttributeValues)
            );
        }

        [Test]
        public void TestTableFiltering()
        {
            var table = new InformationTable();
            var attributeNames = new[] { "attribute1", "attribute2" };
            var attributesValues = new[] { new[] { 1f, 2f, 3f, 4f }, new[] { 2f, 3f, 4f, 8f } };
            foreach (var i in Enumerable.Range(0, 2))
            {
                table.AddAttribute(attributeNames[i], attributesValues[i]);
            }

            var decisionAttributeName = "decisionAttribute";
            var decisionAttributeValues = new[] { 5, 3, 26, 1 };
            table.AddDecisionAttribute(decisionAttributeName, decisionAttributeValues);

            var filterPattern = new[] { true, false, true, false };
            var filteredAttributesValues = new[] { new[] { 1f, 3f }, new[] { 2f, 4f } };
            var filteredDecisionAttributeValues = new[] { 5, 26 };
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
            var table = new InformationTable();
            var attributeName = "decisionAttribute";
            var decisionAttributeValues = new[] { 2, 1, 2, 4, 3, 2, 1 };
            table.AddDecisionAttribute(attributeName, decisionAttributeValues, false);
            table.GetDecicionClassesWorstFirst().ShouldBe(new[] { 1, 2, 3, 4 });
        }

        [Test]
        public void TestGetDecisionAttributeOrderedClassesAsCost()
        {
            var table = new InformationTable();
            var attributeName = "decisionAttribute";
            var decisionAttributeValues = new[] { 2, 1, 2, 4, 3, 2, 1 };
            table.AddDecisionAttribute(attributeName, decisionAttributeValues, true);
            table.GetDecicionClassesWorstFirst().ShouldBe(new[] { 4, 3, 2, 1 });
        }
    }
}
