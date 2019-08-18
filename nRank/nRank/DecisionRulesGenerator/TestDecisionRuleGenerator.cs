using nRank.ApproximationsGeneratorsVC;
using nRank.TestCommons;
using nRank.UnionGenerators;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.DecisionRulesGenerator
{
    [TestFixture]
    class TestDecisionRuleGenerator
    {
        [Test]
        public void TestGeneratingRules()
        {
            var generator = new InformationTableGenerator();
            var lAOUGenerator = new LowerApproximationOfDownwardUnionGeneratorVC();
            var UUGenerator = new DownwardUnionGenerator();
            var table = generator.GetInformationTable();
            var upwardUnions = UUGenerator.GenerateUnions(table).ToList();
            var approximation1 = lAOUGenerator
                .GetApproximation(upwardUnions[0], table, 0);

            var dRGenerator = new DecisionRuleGenerator();
            var rules1 = dRGenerator.GenerateRulesFrom(approximation1, 0).ToList();
            var approximation2 = lAOUGenerator
                .GetApproximation(upwardUnions[1], table,0);
            var rules2 = dRGenerator.GenerateRulesFrom(approximation2, 0).ToList();
            rules1.Select(x => x.ToString()).ShouldBe(new[] { "if (f(a3, x) <= 2,5) then x E Cl1<=", "if (f(a2, x) <= 1,2) and (f(a1, x) <= 1) then x E Cl1<=" });
            rules2.Select(x => x.ToString()).ShouldBe(new[] { "if (f(a1, x) <= 2) then x E Cl2<=" });

        }

        [Test]
        public void TestGeneratingRulesFromUpwardUnions()
        {
            var generator = new InformationTableGenerator();
            var lAOUGenerator = new LowerApproximationOfUpwardUnionGeneratorVC();
            var UUGenerator = new UpwardUnionGenerator();
            var table = generator.GetInformationTable();
            var upwardUnions = UUGenerator.GenerateUnions(table).ToList();
            var approximation1 = lAOUGenerator
                .GetApproximation(upwardUnions[0], table, 0);

            var dRGenerator = new DecisionRuleGenerator();
            var rules1 = dRGenerator.GenerateRulesFrom(approximation1, 0).ToList();
            var approximation2 = lAOUGenerator
                .GetApproximation(upwardUnions[1], table, 0);
            var rules2 = dRGenerator.GenerateRulesFrom(approximation2, 0).ToList();
            rules1.Select(x => x.ToString()).ShouldBe(new[] {
                "if (f(a3, x) >= 9) then x E Cl2>=",
                "if (f(a2, x) >= 2,8) and (f(a3, x) >= 6) then x E Cl2>=",
                "if (f(a2, x) >= 2,8) and (f(a1, x) >= 1,7) then x E Cl2>=",
                "if (f(a1, x) >= 1,2) and (f(a3, x) >= 7) then x E Cl2>="
            });
            rules2.Select(x => x.ToString()).ShouldBe(new[] {
                "if (f(a1, x) >= 2,7) then x E Cl3>=",
                "if (f(a3, x) >= 13) and (f(a1, x) >= 2,3) then x E Cl3>="
            });

        }

        [Test]
        public void TestGeneratingRulesFromUpwardUnionsUpperApproximations()
        {
            var generator = new InformationTableGenerator();
            var lAOUGenerator = new UpperApproximationOfUpwardUnionGeneratorVC();
            var UUGenerator = new UpwardUnionGenerator();
            var DUGenerator = new DownwardUnionGenerator();
            var table = generator.GetInformationTable();
            var unionDict = DUGenerator.GenerateUnionsAsDict(table);
            var upwardUnions = UUGenerator.GenerateUnions(table).ToList();
            var approximation1 = lAOUGenerator
                .GetApproximation(upwardUnions[0], table, 0, unionDict);

            var dRGenerator = new DecisionRuleGenerator();
            var rules1 = dRGenerator.GenerateRulesFrom(approximation1, 0).ToList();
            var approximation2 = lAOUGenerator
                .GetApproximation(upwardUnions[1], table, 0, unionDict);
            var rules2 = dRGenerator.GenerateRulesFrom(approximation2, 0).ToList();
            rules1.Select(x => x.ToString()).ShouldBe(new[] { "if (f(a2, x) >= 2,4) then x E Cl2>=", "if (f(a1, x) >= 1,2) then x E Cl2>=", "if (f(a3, x) >= 4,5) and (f(a2, x) >= 2) then x E Cl2>=" });
            rules2.Select(x => x.ToString()).ShouldBe(new[] { "if (f(a1, x) >= 2,3) then x E Cl3>=" });

        }

    }
}
