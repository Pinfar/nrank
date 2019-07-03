using nRank.TestCommons;
using nRank.UnionGenerators;
using NUnit.Framework;
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
            var DUGenerator = new DownwardUnionGenerator();
            var table = new InformationTableGenerator().GetInformationTable();
            var unions = DUGenerator.GenerateUnions(table).ToList();
            foreach(var union in unions)
            {
                var approximation1 = generator.GetApproximation(union, table, 0.7f);
                var approximation2 = generator.GetApproximation(union, table, 1.0f);
            }
        }
    }
}
