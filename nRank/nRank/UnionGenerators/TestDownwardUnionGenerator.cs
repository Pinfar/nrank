using nRank.TestCommons;
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
    class TestDownwardUnionGenerator
    {
        [Test]
        public void TestGeneratingDownwardUnions()
        {
            var generator = new DownwardUnionGenerator();
            var informationTable = new InformationTableGenerator().GetInformationTable();

            var unions = generator.GenerateUnions(informationTable).ToList();

            unions.Count.ShouldBe(2);
            unions[0]
                .InformationTable
                .GetAllObjectIdentifiers()
                .ShouldBe(new[] {"3", "4", "7", "9", "14" }, true);

            unions[0].IsUpward.ShouldBeFalse();
            unions[0].Classes.ShouldBe(new[] { 1 }, true);

            unions[1]
                .InformationTable
                .GetAllObjectIdentifiers()
                .ShouldBe(new[] {"1","2", "3", "4","6", "7", "9", "10", "11","12","13", "14","15" }, true);

            unions[1].IsUpward.ShouldBeFalse();
            unions[1].Classes.ShouldBe(new[] { 1, 2 }, true);
        }
    }
}
