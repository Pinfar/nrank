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
            unions[0].Symbol.ShouldBe("Cl1<=");
            unions[0].OppositeSymbol.ShouldBe("Cl2>=");

            unions[1]
                .InformationTable
                .GetAllObjectIdentifiers()
                .ShouldBe(new[] {"1","2", "3", "4","6", "7", "9", "10", "11","12","13", "14","15" }, true);

            unions[1].IsUpward.ShouldBeFalse();
            unions[1].Classes.ShouldBe(new[] { 1, 2 }, true);
            unions[1].Symbol.ShouldBe("Cl2<=");
            unions[1].OppositeSymbol.ShouldBe("Cl3>=");
        }


        [Test]
        public void TestGeneratingDownwardUnionsAsDict()
        {
            var generator = new DownwardUnionGenerator();
            var informationTable = new InformationTableGenerator().GetInformationTable();

            var unions = generator.GenerateUnionsAsDict(informationTable);

            unions.Count.ShouldBe(2);
            unions.ShouldContainKey("Cl1<=");
            unions["Cl1<="]
                .InformationTable
                .GetAllObjectIdentifiers()
                .ShouldBe(new[] { "3", "4", "7", "9", "14" }, true);

            unions["Cl1<="].IsUpward.ShouldBeFalse();
            unions["Cl1<="].Classes.ShouldBe(new[] { 1 }, true);
            unions["Cl1<="].Symbol.ShouldBe("Cl1<=");
            unions["Cl1<="].OppositeSymbol.ShouldBe("Cl2>=");

            unions.ShouldContainKey("Cl2<=");
            unions["Cl2<="]
                .InformationTable
                .GetAllObjectIdentifiers()
                .ShouldBe(new[] { "1", "2", "3", "4", "6", "7", "9", "10", "11", "12", "13", "14", "15" }, true);

            unions["Cl2<="].IsUpward.ShouldBeFalse();
            unions["Cl2<="].Classes.ShouldBe(new[] { 1, 2 }, true);
            unions["Cl2<="].Symbol.ShouldBe("Cl2<=");
            unions["Cl2<="].OppositeSymbol.ShouldBe("Cl3>=");
        }
    }
}
