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
            lAOUGenerator
                .GetApproximation(upwardUnions[0], table)
                .GetAllObjectIdentifiers()
                .ShouldBe(new[] { "1", "2", "5", "8", "10", "11", "12", "13", "15", "16", "17" }, true);
            lAOUGenerator
                .GetApproximation(upwardUnions[1], table)
                .GetAllObjectIdentifiers()
                .ShouldBe(new[] { "5", "16", "17" }, true);
        }

        [Test]
        public void TestUAOUGenerator()
        {
            var uAOUGenerator = new UpperApproximationOfUpwardUnionGenerator();
            var UUGenerator = new UpwardUnionGenerator();
            var table = GetInformationTable();
            var upwardUnions = UUGenerator.GenerateUnions(table).ToList();
            upwardUnions.Count.ShouldBe(2);
            uAOUGenerator
                .GetApproximation(upwardUnions[0], table)
                .GetAllObjectIdentifiers()
                .ShouldBe(new[] { "1", "2", "5", "6", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17" }, true);
            uAOUGenerator
                .GetApproximation(upwardUnions[1], table)
                .GetAllObjectIdentifiers()
                .ShouldBe(new[] { "5", "8", "11", "16", "17" }, true);
        }

        [Test]
        public void TestLAODGenerator()
        {
            var lAOUGenerator = new LowerApproximationOfDownwardUnionGenerator();
            var UUGenerator = new DownwardUnionGenerator();
            var table = GetInformationTable();
            var upwardUnions = UUGenerator.GenerateUnions(table).ToList();
            upwardUnions.Count.ShouldBe(2);
            lAOUGenerator
                .GetApproximation(upwardUnions[0], table)
                .GetAllObjectIdentifiers()
                .ShouldBe(new[] { "3", "4", "7" }, true);
            lAOUGenerator
                .GetApproximation(upwardUnions[1], table)
                .GetAllObjectIdentifiers()
                .ShouldBe(new[] { "1", "2", "3", "4", "6", "7", "9", "10", "12", "13", "14", "15" }, true);
        }

        [Test]
        public void TestUAODGenerator()
        {
            var lAOUGenerator = new UpperApproximationOfDownwardUnionGenerator();
            var UUGenerator = new DownwardUnionGenerator();
            var table = GetInformationTable();
            var upwardUnions = UUGenerator.GenerateUnions(table).ToList();
            upwardUnions.Count.ShouldBe(2);
            lAOUGenerator
                .GetApproximation(upwardUnions[0], table)
                .GetAllObjectIdentifiers()
                .ShouldBe(new[] { "3", "4","6", "7","9","14" }, true);
            lAOUGenerator
                .GetApproximation(upwardUnions[1], table)
                .GetAllObjectIdentifiers()
                .ShouldBe(new[] { "1", "2", "3", "4", "6", "7","8", "9", "10","11", "12", "13", "14", "15" }, true);
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


            boundaryGenerator
                .GetApproximation(upwardUnions[0], table)
                .GetAllObjectIdentifiers()
                .ShouldBe(new[] { "6", "9", "14" }, true);
            boundaryGenerator
                .GetApproximation(upwardUnions[1], table)
                .GetAllObjectIdentifiers()
                .ShouldBe(new[] { "8",  "11" }, true);
        }

        private IInformationTable GetInformationTable()
        {
            var generator = new InformationTableGenerator();
            return generator.GetInformationTable();
        }
    }
}
