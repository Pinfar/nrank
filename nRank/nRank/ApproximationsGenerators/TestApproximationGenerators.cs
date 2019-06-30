using nRank.DataStructures;
using nRank.TestCommons;
using nRank.UnionGenerators;
using nRank.VCDomLEMAbstractions;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.ApproximationsGenerators
{
    [TestFixture]
    class TestApproximationGenerators
    {
        [Test]
        public void TestLAOUGenerator()
        {
            var lAOUGenerator = new LowerApproximationOfUpwardUnionGenerator();
            var UUGenerator = new UpwardUnionGenerator();
            var table = GetInformationTable();
            var upwardUnions = UUGenerator.GenerateUnions(table).ToList();
            upwardUnions.Count.ShouldBe(2);

            var approximation0 = lAOUGenerator
                .GetApproximation(upwardUnions[0], table);
            ShouldHave(approximation0, new[] { "1", "2", "5", "8", "10", "11", "12", "13", "15", "16", "17" }, table, ">=");

            var approximation1 = lAOUGenerator
                .GetApproximation(upwardUnions[1], table);
            ShouldHave(approximation1, new[] { "5", "16", "17" }, table, ">=");
        }

        [Test]
        public void TestUAOUGenerator()
        {
            var uAOUGenerator = new UpperApproximationOfUpwardUnionGenerator();
            var UUGenerator = new UpwardUnionGenerator();
            var table = GetInformationTable();
            var upwardUnions = UUGenerator.GenerateUnions(table).ToList();
            upwardUnions.Count.ShouldBe(2);


            var approximation0 = uAOUGenerator
                .GetApproximation(upwardUnions[0], table);
            ShouldHave(approximation0, new[] { "1", "2", "5", "6", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17" }, table, ">=");

            var approximation1 = uAOUGenerator
                .GetApproximation(upwardUnions[1], table);
            ShouldHave(approximation1, new[] { "5", "8", "11", "16", "17" }, table, ">=");
        }

        [Test]
        public void TestLAODGenerator()
        {
            var lAOUGenerator = new LowerApproximationOfDownwardUnionGenerator();
            var UUGenerator = new DownwardUnionGenerator();
            var table = GetInformationTable();
            var upwardUnions = UUGenerator.GenerateUnions(table).ToList();
            upwardUnions.Count.ShouldBe(2);

            var approximation0 = lAOUGenerator
                .GetApproximation(upwardUnions[0], table);
            ShouldHave(approximation0, new[] { "3", "4", "7" }, table, "<=");

            var approximation1 = lAOUGenerator
                .GetApproximation(upwardUnions[1], table);
            ShouldHave(approximation1, new[] { "1", "2", "3", "4", "6", "7", "9", "10", "12", "13", "14", "15" }, table, "<=");
        }

        [Test]
        public void TestUAODGenerator()
        {
            var lAOUGenerator = new UpperApproximationOfDownwardUnionGenerator();
            var UUGenerator = new DownwardUnionGenerator();
            var table = GetInformationTable();
            var upwardUnions = UUGenerator.GenerateUnions(table).ToList();
            upwardUnions.Count.ShouldBe(2);

            var approximation0 = lAOUGenerator
                .GetApproximation(upwardUnions[0], table);
            ShouldHave(approximation0, new[] { "3", "4", "6", "7", "9", "14" }, table, "<=");

            var approximation1 = lAOUGenerator
                .GetApproximation(upwardUnions[1], table);
            ShouldHave(approximation1, new[] { "1", "2", "3", "4", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" }, table, "<=");

        }

        [Test]
        public void TestBoundaryGenerator()
        {
            var uAOUGenerator = new UpperApproximationOfDownwardUnionGenerator();
            var lAOUGenerator = new LowerApproximationOfDownwardUnionGenerator();
            var boundaryGenerator = new BoundaryApproximationGenerator(lAOUGenerator, uAOUGenerator);
            var UUGenerator = new DownwardUnionGenerator();
            var table = GetInformationTable();
            var upwardUnions = UUGenerator.GenerateUnions(table).ToList();
            upwardUnions.Count.ShouldBe(2);

            var approximation0 = boundaryGenerator
                .GetApproximation(upwardUnions[0], table);
            ShouldHave(approximation0, new[] { "6", "9", "14" }, table, new [] { ">=", "<=" });

            var approximation1 = boundaryGenerator
                .GetApproximation(upwardUnions[1], table);
            ShouldHave(approximation1, new[] { "8", "11" }, table, new[] { ">=", "<=" });
        }

        private IInformationTable GetInformationTable()
        {
            var generator = new InformationTableGenerator();
            return generator.GetInformationTable();
        }
        private void ShouldHave(IApproximation approximation, IEnumerable<string> identifiers, IInformationTable originalTable, string allowedOperator)
        {
            ShouldHave(approximation, identifiers, originalTable, new[] { allowedOperator });
        }

        private void ShouldHave(IApproximation approximation, IEnumerable<string> identifiers, IInformationTable originalTable, IEnumerable<string> allowedOperators)
        {
            approximation
                .ApproximatedInformationTable
                .GetAllObjectIdentifiers()
                .ShouldBe(identifiers, true);
            approximation.OriginalInformationTable.ShouldBe(originalTable);
            approximation.AllowedOperators.ShouldBe(allowedOperators);
        }
    }
}
