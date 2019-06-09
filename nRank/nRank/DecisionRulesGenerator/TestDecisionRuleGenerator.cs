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

namespace nRank.DecisionRulesGenerator
{
    [TestFixture]
    class TestDecisionRuleGenerator
    {
        [Test]
        public void TestGeneratingRules()
        {
            var generator = new InformationTableGenerator();
            var lAOUGenerator = new LowerApproximationOfDownwardUnionGenerator();
            var UUGenerator = new DownwardUnionGenerator();
            var table = generator.GetInformationTable();
            var upwardUnions = UUGenerator.GenerateUnions(table).ToList();
            var approximation1 = lAOUGenerator
                .GetApproximation(upwardUnions[0], table);

            var dRGenerator = new DecisionRuleGenerator();
            var rules1 = dRGenerator.GenerateRulesFrom(approximation1, table, "Cl1<=").ToList();
            var approximation2 = lAOUGenerator
                .GetApproximation(upwardUnions[1], table);
            var rules2 = dRGenerator.GenerateRulesFrom(approximation2, table, "Cl2<=").ToList();
            rules1.Select(x => x.ToString()).ShouldBe(new[] { "if (f(a3, x) <= 2,5) then x E Cl1<=", "if (f(a2, x) <= 1,2) and (f(a1, x) <= 1) then x E Cl1<=" });
            rules2.Select(x => x.ToString()).ShouldBe(new[] { "if (f(a1, x) <= 2) then x E Cl2<=" });

        }

        [Test]
        public void TestGeneratingRulesFromUpwardUnions()
        {
            var generator = new InformationTableGenerator();
            var lAOUGenerator = new LowerApproximationOfUpwardUnionGenerator();
            var UUGenerator = new UpwardUnionGenerator();
            var table = generator.GetInformationTable();
            var upwardUnions = UUGenerator.GenerateUnions(table).ToList();
            var approximation1 = lAOUGenerator
                .GetApproximation(upwardUnions[0], table);

            var dRGenerator = new DecisionRuleGenerator(">=");
            var rules1 = dRGenerator.GenerateRulesFrom(approximation1, table, "Cl2>=").ToList();
            var approximation2 = lAOUGenerator
                .GetApproximation(upwardUnions[1], table);
            var rules2 = dRGenerator.GenerateRulesFrom(approximation2, table, "Cl3>=").ToList();
            //rules1.Select(x => x.ToString()).ShouldBe(new[] { "if (f(a3, x) <= 2,5) then x E Cl1<=", "if (f(a2, x) <= 1,2) and (f(a1, x) <= 1) then x E Cl1<=" });
            //rules2.Select(x => x.ToString()).ShouldBe(new[] { "if (f(a1, x) <= 2) then x E Cl2<=" });

        }

        [Test]
        public void TestGeneratingRulesFromUpwardUnionsUpperApproximations()
        {
            var generator = new InformationTableGenerator();
            var lAOUGenerator = new UpperApproximationOfUpwardUnionGenerator();
            var UUGenerator = new UpwardUnionGenerator();
            var table = generator.GetInformationTable();
            var upwardUnions = UUGenerator.GenerateUnions(table).ToList();
            var approximation1 = lAOUGenerator
                .GetApproximation(upwardUnions[0], table);

            var dRGenerator = new DecisionRuleGenerator(">=");
            var rules1 = dRGenerator.GenerateRulesFrom(approximation1, table, "Cl2>=").ToList();
            var approximation2 = lAOUGenerator
                .GetApproximation(upwardUnions[1], table);
            var rules2 = dRGenerator.GenerateRulesFrom(approximation2, table, "Cl3>=").ToList();
            //rules1.Select(x => x.ToString()).ShouldBe(new[] { "if (f(a3, x) <= 2,5) then x E Cl1<=", "if (f(a2, x) <= 1,2) and (f(a1, x) <= 1) then x E Cl1<=" });
            //rules2.Select(x => x.ToString()).ShouldBe(new[] { "if (f(a1, x) <= 2) then x E Cl2<=" });

        }

        [Test]
        public void TestGeneratingRulesFromBoundaries()
        {
            var generator = new InformationTableGenerator();
            var uAOUGenerator = new UpperApproximationOfUpwardUnionGenerator();
            var lAOUGenerator = new LowerApproximationOfUpwardUnionGenerator();
            var UUGenerator = new UpwardUnionGenerator();
            var table = generator.GetInformationTable();
            var upwardUnions = UUGenerator.GenerateUnions(table).ToList();
            var lowerApproximation = lAOUGenerator
                .GetApproximation(upwardUnions[0], table);
            var upperApproximation = uAOUGenerator
                .GetApproximation(upwardUnions[0], table);

            var lowerItems = lowerApproximation.GetAllObjectIdentifiers();
            var mask = upperApproximation.GetAllObjectIdentifiers().ToDictionary(x => x, x => !lowerItems.Contains(x));


            var boundary = upperApproximation.Filter(mask);
            var dRGenerator = new DecisionRuleGenerator(new[] { ">=", "<=" });
            var rules = dRGenerator.GenerateRulesFrom(boundary, table, "Cl1 u Cl2").ToList();
            //rules1.Select(x => x.ToString()).ShouldBe(new[] { "if (f(a3, x) <= 2,5) then x E Cl1<=", "if (f(a2, x) <= 1,2) and (f(a1, x) <= 1) then x E Cl1<=" });
            //rules2.Select(x => x.ToString()).ShouldBe(new[] { "if (f(a1, x) <= 2) then x E Cl2<=" });

        }

    }
}
