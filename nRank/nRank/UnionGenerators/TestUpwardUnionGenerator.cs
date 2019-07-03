using nRank.DataStructures;
using nRank.Extensions;
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
            var isAttributeCost = attributeValues.Select(x => false).ToDictionary();
            var decisionAttributeName = "Decision";
            var decisionAttributeValues = new[] { 1, 2, 3, 1, 2, 3 };

            var informationTable = new InformationTable(isAttributeCost, decisionAttributeName, true);
            foreach(var i in Enumerable.Range(0, 6))
            {
                informationTable.AddObject(i.ToString(), CreateSingleAttributeDict(attributeName, attributeValues[i]), decisionAttributeValues[i]);
            }

            var generator = new UpwardUnionGenerator();
            var unions = generator.GenerateUnions(informationTable).ToList();

            unions.Count.ShouldBe(2);

            unions[0].InformationTable.GetAttribute(attributeName).ShouldBe(new[] { 1f, 2f, 4f, 5f });
            var expectedDecisionAttributeDict1 = new Dictionary<string, int>
            {
                { "0",1 },{"1", 2 },{"3", 1 },{"4", 2 }
            };
            unions[0].InformationTable.GetDecisionAttribute().ShouldBe(expectedDecisionAttributeDict1);
            unions[0].IsUpward.ShouldBeTrue();
            unions[0].Classes.ShouldBe(new[] { 1, 2 }, true);
            unions[0].Symbol.ShouldBe("Cl2>=");
            unions[0].OppositeSymbol.ShouldBe("Cl3<=");

            unions[1].InformationTable.GetAttribute(attributeName).ShouldBe(new[] { 1f, 4f });
            var expectedDecisionAttributeDict2 = new Dictionary<string, int>
            {
                { "0",1 },{"3", 1 }
            };
            unions[1].InformationTable.GetDecisionAttribute().ShouldBe(expectedDecisionAttributeDict2);
            unions[1].IsUpward.ShouldBeTrue();
            unions[1].Classes.ShouldBe(new[] { 1 }, true);
            unions[1].Symbol.ShouldBe("Cl1>=");
            unions[1].OppositeSymbol.ShouldBe("Cl2<=");
        }

        [Test]
        public void TestGenerateUnionsWithGainAttribute()
        {
            var attributeName = "Attribute1";
            var attributeValues = new[] { 1f, 2f, 3f, 4f, 5f, 6f };
            var isAttributeCost = attributeValues.Select(x => false).ToDictionary();
            var decisionAttributeName = "Decision";
            var decisionAttributeValues = new[] { 1, 2, 3, 1, 2, 3 };

            var informationTable = new InformationTable(isAttributeCost, decisionAttributeName, false);
            foreach (var i in Enumerable.Range(0, 6))
            {
                informationTable.AddObject(i.ToString(), CreateSingleAttributeDict(attributeName, attributeValues[i]), decisionAttributeValues[i]);
            }

            var generator = new UpwardUnionGenerator();
            var unions = generator.GenerateUnions(informationTable).ToList();

            unions.Count.ShouldBe(2);

            unions[0].InformationTable.GetAttribute(attributeName).ShouldBe(new[] { 2f, 3f, 5f, 6f });
            var expectedDecisionAttributeDict1 = new Dictionary<string, int>
            {
                { "1",2 },{"2", 3 },{"4", 2 },{"5", 3 }
            };
            unions[0].InformationTable.GetDecisionAttribute().ShouldBe(expectedDecisionAttributeDict1);
            unions[0].IsUpward.ShouldBeTrue();
            unions[0].Classes.ShouldBe(new[] { 2, 3 }, true);
            unions[0].Symbol.ShouldBe("Cl2>=");
            unions[0].OppositeSymbol.ShouldBe("Cl1<=");

            unions[1].InformationTable.GetAttribute(attributeName).ShouldBe(new[] { 3f, 6f });
            var expectedDecisionAttributeDict2 = new Dictionary<string, int>
            {
                { "2",3 },{"5", 3 }
            };
            unions[1].InformationTable.GetDecisionAttribute().ShouldBe(expectedDecisionAttributeDict2);
            unions[1].IsUpward.ShouldBeTrue();
            unions[1].Classes.ShouldBe(new[] { 3 }, true);
            unions[1].Symbol.ShouldBe("Cl3>=");
            unions[1].OppositeSymbol.ShouldBe("Cl2<=");
        }

        [Test]
        public void TestGenerateUnionsAsDictWithCostAttribute()
        {
            var attributeName = "Attribute1";
            var attributeValues = new[] { 1f, 2f, 3f, 4f, 5f, 6f };
            var isAttributeCost = attributeValues.Select(x => false).ToDictionary();
            var decisionAttributeName = "Decision";
            var decisionAttributeValues = new[] { 1, 2, 3, 1, 2, 3 };

            var informationTable = new InformationTable(isAttributeCost, decisionAttributeName, true);
            foreach (var i in Enumerable.Range(0, 6))
            {
                informationTable.AddObject(i.ToString(), CreateSingleAttributeDict(attributeName, attributeValues[i]), decisionAttributeValues[i]);
            }

            var generator = new UpwardUnionGenerator();
            var unions = generator.GenerateUnionsAsDict(informationTable);

            unions.Count.ShouldBe(2);
            unions.ShouldContainKey("Cl2>=");
            unions["Cl2>="].InformationTable.GetAttribute(attributeName).ShouldBe(new[] { 1f, 2f, 4f, 5f });
            var expectedDecisionAttributeDict1 = new Dictionary<string, int>
            {
                { "0",1 },{"1", 2 },{"3", 1 },{"4", 2 }
            };
            unions["Cl2>="].InformationTable.GetDecisionAttribute().ShouldBe(expectedDecisionAttributeDict1);
            unions["Cl2>="].IsUpward.ShouldBeTrue();
            unions["Cl2>="].Classes.ShouldBe(new[] { 1, 2 }, true);
            unions["Cl2>="].Symbol.ShouldBe("Cl2>=");
            unions["Cl2>="].OppositeSymbol.ShouldBe("Cl3<=");

            unions.ShouldContainKey("Cl1>=");
            unions["Cl1>="].InformationTable.GetAttribute(attributeName).ShouldBe(new[] { 1f, 4f });
            var expectedDecisionAttributeDict2 = new Dictionary<string, int>
            {
                { "0",1 },{"3", 1 }
            };
            unions["Cl1>="].InformationTable.GetDecisionAttribute().ShouldBe(expectedDecisionAttributeDict2);
            unions["Cl1>="].IsUpward.ShouldBeTrue();
            unions["Cl1>="].Classes.ShouldBe(new[] { 1 }, true);
            unions["Cl1>="].Symbol.ShouldBe("Cl1>=");
            unions["Cl1>="].OppositeSymbol.ShouldBe("Cl2<=");
        }

        [Test]
        public void TestGenerateUnionsAsDictWithGainAttribute()
        {
            var attributeName = "Attribute1";
            var attributeValues = new[] { 1f, 2f, 3f, 4f, 5f, 6f };
            var isAttributeCost = attributeValues.Select(x => false).ToDictionary();
            var decisionAttributeName = "Decision";
            var decisionAttributeValues = new[] { 1, 2, 3, 1, 2, 3 };

            var informationTable = new InformationTable(isAttributeCost, decisionAttributeName, false);
            foreach (var i in Enumerable.Range(0, 6))
            {
                informationTable.AddObject(i.ToString(), CreateSingleAttributeDict(attributeName, attributeValues[i]), decisionAttributeValues[i]);
            }

            var generator = new UpwardUnionGenerator();
            var unions = generator.GenerateUnionsAsDict(informationTable);

            unions.Count.ShouldBe(2);
            unions.ShouldContainKey("Cl2>=");
            unions["Cl2>="].InformationTable.GetAttribute(attributeName).ShouldBe(new[] { 2f, 3f, 5f, 6f });
            var expectedDecisionAttributeDict1 = new Dictionary<string, int>
            {
                { "1",2 },{"2", 3 },{"4", 2 },{"5", 3 }
            };
            unions["Cl2>="].InformationTable.GetDecisionAttribute().ShouldBe(expectedDecisionAttributeDict1);
            unions["Cl2>="].IsUpward.ShouldBeTrue();
            unions["Cl2>="].Classes.ShouldBe(new[] { 2, 3 }, true);
            unions["Cl2>="].Symbol.ShouldBe("Cl2>=");
            unions["Cl2>="].OppositeSymbol.ShouldBe("Cl1<=");

            unions.ShouldContainKey("Cl3>=");
            unions["Cl3>="].InformationTable.GetAttribute(attributeName).ShouldBe(new[] { 3f, 6f });
            var expectedDecisionAttributeDict2 = new Dictionary<string, int>
            {
                { "2",3 },{"5", 3 }
            };
            unions["Cl3>="].InformationTable.GetDecisionAttribute().ShouldBe(expectedDecisionAttributeDict2);
            unions["Cl3>="].IsUpward.ShouldBeTrue();
            unions["Cl3>="].Classes.ShouldBe(new[] { 3 }, true);
            unions["Cl3>="].Symbol.ShouldBe("Cl3>=");
            unions["Cl3>="].OppositeSymbol.ShouldBe("Cl2<=");
        }

        private Dictionary<string, float> CreateSingleAttributeDict(string key, float value)
        {
            return new Dictionary<string, float>
            {
                {key, value }
            };
        }
    }
}
