using nRank.ApproximationsGenerators;
using nRank.TestCommons;
using nRank.UnionGenerators;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.ApproximationsGeneratorsVC
{
    [TestFixture]
    class TestApproximationGeneratorsVC
    {
        [Test]
        public void TestLAODUGVC()
        {
            var generator = new LowerApproximationOfDownwardUnionGeneratorVC();
            var generator1 = new LowerApproximationOfDownwardUnionGenerator();
            var DUGenerator = new DownwardUnionGenerator();
            var table = new InformationTableGenerator().GetInformationTable();
            var unions = DUGenerator.GenerateUnions(table).ToList();
            foreach (var union in unions)
            {
                var approximation1 = generator1.GetApproximation(union, table).ApproximatedInformationTable.GetAllObjectIdentifiers();
                var approximation2 = generator.GetApproximation(union, table, 1.0f).ApproximatedInformationTable.GetAllObjectIdentifiers();
                approximation2.ShouldBe(approximation1);
            }
        }

        [Test]
        public void TestLAOUUGVC()
        {
            var generator = new LowerApproximationOfUpwardUnionGeneratorVC();
            var generator1 = new LowerApproximationOfUpwardUnionGenerator();
            var DUGenerator = new DownwardUnionGenerator();
            var table = new InformationTableGenerator().GetInformationTable();
            var unions = DUGenerator.GenerateUnions(table).ToList();
            foreach (var union in unions)
            {
                var approximation1 = generator1.GetApproximation(union, table).ApproximatedInformationTable.GetAllObjectIdentifiers();
                var approximation2 = generator.GetApproximation(union, table, 1.0f).ApproximatedInformationTable.GetAllObjectIdentifiers();
                approximation2.ShouldBe(approximation1);
            }
        }

        [Test]
        public void TestUAODUGVC()
        {
            var generator = new UpperApproximationOfDownwardUnionGeneratorVC();
            var generator1 = new UpperApproximationOfDownwardUnionGenerator();
            var DUGenerator = new DownwardUnionGenerator();
            var UUGenerator = new UpwardUnionGenerator();
            var table = new InformationTableGenerator().GetInformationTable();
            var unions = DUGenerator.GenerateUnions(table).ToList();
            var dict = UUGenerator.GenerateUnionsAsDict(table);
            foreach (var union in unions)
            {
                var approximation1 = generator1.GetApproximation(union, table).ApproximatedInformationTable.GetAllObjectIdentifiers();
                var approximation2 = generator.GetApproximation(union, table, 1.0f, dict).ApproximatedInformationTable.GetAllObjectIdentifiers();
                approximation2.ShouldBe(approximation1);
            }
        }

        [Test]
        public void TestUAOUUGVC()
        {
            var generator = new UpperApproximationOfUpwardUnionGeneratorVC();
            var generator1 = new UpperApproximationOfUpwardUnionGenerator();
            var DUGenerator = new DownwardUnionGenerator();
            var UUGenerator = new UpwardUnionGenerator();
            var table = new InformationTableGenerator().GetInformationTable();
            var unions = UUGenerator.GenerateUnions(table).ToList();
            var dict = DUGenerator.GenerateUnionsAsDict(table);
            foreach (var union in unions)
            {
                var approximation1 = generator1.GetApproximation(union, table).ApproximatedInformationTable.GetAllObjectIdentifiers();
                var approximation2 = generator.GetApproximation(union, table, 1.0f, dict).ApproximatedInformationTable.GetAllObjectIdentifiers();
                approximation2.ShouldBe(approximation1);
            }
        }
    }
}
