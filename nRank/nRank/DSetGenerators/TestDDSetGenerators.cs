using nRank.DataStructures;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.DSetGenerators
{
    [TestFixture]
    class TestDDSetGenerators
    {
        [Test]
        public void TestDominatingSetGenerator()
        {
            var attributeName = "decisionAttribute";
            var table = new InformationTable(new Dictionary<string, bool> { { "att1", true } }, attributeName, true);
            var objects = new[]
            {
                new { Key = "0", Attributes = new Dictionary<string, float> { { "att1", 1 } } },
                new { Key = "1", Attributes = new Dictionary<string, float> { { "att1", 2 } } },
                new { Key = "2", Attributes = new Dictionary<string, float> { { "att1", 1 } } },
                new { Key = "3", Attributes = new Dictionary<string, float> { { "att1", 3 } } },
            };
            foreach (var obj in objects)
            {
                table.AddObject(obj.Key, obj.Attributes, 1);
            }
            var generator = new DDominatingSetGenerator();

            generator.Generate(table, "2").GetAllObjectIdentifiers().ShouldBe(new[] { "0", "2" }, true);
            generator.Generate(table, "3").GetAllObjectIdentifiers().ShouldBe(new[] { "0","1","3", "2" }, true);
        }

        [Test]
        public void TestDominatedSetGenerator()
        {
            var attributeName = "decisionAttribute";
            var table = new InformationTable(new Dictionary<string, bool> { { "att1", true } }, attributeName, true);
            var objects = new[]
            {
                new { Key = "0", Attributes = new Dictionary<string, float> { { "att1", 1 } } },
                new { Key = "1", Attributes = new Dictionary<string, float> { { "att1", 2 } } },
                new { Key = "2", Attributes = new Dictionary<string, float> { { "att1", 1 } } },
                new { Key = "3", Attributes = new Dictionary<string, float> { { "att1", 3 } } },
            };
            foreach (var obj in objects)
            {
                table.AddObject(obj.Key, obj.Attributes, 1);
            }
            var generator = new DDominatedSetGenerator();

            generator.Generate(table, "2").GetAllObjectIdentifiers().ShouldBe(new[] { "0", "1", "3", "2" }, true);
            generator.Generate(table, "3").GetAllObjectIdentifiers().ShouldBe(new[] { "3" }, true);
        }

    }
}
