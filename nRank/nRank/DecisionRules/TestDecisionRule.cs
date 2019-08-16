using nRank.TestCommons;
using nRank.VCDomLEMAbstractions;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using nRank.DataStructures;

namespace nRank.DecisionRules
{
    [TestFixture]
    class TestDecisionRule
    {
        IApproximation _approximation;

        [SetUp]
        public void Init()
        {
            _approximation = Substitute.For<IApproximation>();
            _approximation.Symbol.Returns("Cl1<=");

            var generator = new InformationTableGenerator();
            var table = generator.GetInformationTable();
            _approximation.OriginalInformationTable.Returns(table);
        }

        [Test]
        public void TestToString()
        {
            var dr = new ImmutableDecisionRule("att1", "<=",3.5f, _approximation);
            dr.ToString().ShouldBe("if (f(att1, x) <= 3,5) then x E Cl1<=");
        }

        [Test]
        public void TestAnd()
        {
            var dr = new ImmutableDecisionRule("att1", "<=", 3.5f, _approximation).And("att2", "<=", 4.5f);
            dr.ToString().ShouldBe("if (f(att1, x) <= 3,5) and (f(att2, x) <= 4,5) then x E Cl1<=");
        }

        [Test]
        public void TestSatisfy()
        {
            var generator = new InformationTableGenerator();
            var table = generator.GetInformationTable();
            var dr = new ImmutableDecisionRule("a1", "<=", 1.5f, _approximation);

            dr.Satisfy(table).ShouldBe(new Dictionary<string, bool>
            {
                {"1", true },
                {"2", false },
                {"3", true },
                {"4", true },
                {"5", false },
                {"6", true },
                {"7", true },
                {"8", false },
                {"9", true },
                {"10", false },
                {"11", false },
                {"12", true },
                {"13", true },
                {"14", false },
                {"15", false },
                {"16", false },
                {"17", false }
            });
        }

        [Test]
        public void TestSatisfyObjects()
        {
            var generator = new InformationTableGenerator();
            var table = generator.GetInformationTable();
            var dr = new ImmutableDecisionRule("a1", "<=", 1.5f, _approximation);

            dr.GetSatisfiedObjectsIdentifiers(table).ShouldBe(new []
            {
                "1",
                "3",
                "4",
                "6",
                "7",
                "9",
                "12",
                "13",
            });
        }

        [Test]
        public void TestSatisfyWithAnd()
        {
            var generator = new InformationTableGenerator();
            var table = generator.GetInformationTable();
            var dr = new ImmutableDecisionRule("a1", "<=", 1.5f, _approximation).And("a3", ">=", 12f);

            dr.Satisfy(table).ShouldBe(new Dictionary<string, bool>
            {
                {"1", true },
                {"2", false },
                {"3", false },
                {"4", false },
                {"5", false },
                {"6", false },
                {"7", false },
                {"8", false },
                {"9", false },
                {"10", false },
                {"11", false },
                {"12", false },
                {"13", false },
                {"14", false },
                {"15", false },
                {"16", false },
                {"17", false }
            });
        }

        [Test]
        public void TestAlwaysTrue()
        {
            var generator = new InformationTableGenerator();
            var table = generator.GetInformationTable();
            var dr = ImmutableDecisionRule.GetAlwaysTrueRule(_approximation);

            dr.Satisfy(table).Values.ShouldAllBe(x => true);
        }

        [Test]
        public void TestIsEmpty()
        {
            new ImmutableDecisionRule("a1", "<=", 1.5f, _approximation).IsEmpty().ShouldBeFalse();
            ImmutableDecisionRule.GetAlwaysTrueRule(_approximation).IsEmpty().ShouldBeTrue();
            ImmutableDecisionRule.GetAlwaysTrueRule(_approximation).And("a3", ">=", 12f).IsEmpty().ShouldBeFalse();
        }

        [Test]
        public void TestSatisfyWithAndRules()
        {
            var generator = new InformationTableGenerator();
            var table = generator.GetInformationTable();
            var dr1 = new ImmutableDecisionRule("a1", "<=", 1.5f, _approximation);
            var dr2 = new ImmutableDecisionRule("a3", ">=", 12f, _approximation);
            var dr = dr1.And(dr2);

            dr.Satisfy(table).ShouldBe(new Dictionary<string, bool>
            {
                {"1", true },
                {"2", false },
                {"3", false },
                {"4", false },
                {"5", false },
                {"6", false },
                {"7", false },
                {"8", false },
                {"9", false },
                {"10", false },
                {"11", false },
                {"12", false },
                {"13", false },
                {"14", false },
                {"15", false },
                {"16", false },
                {"17", false }
            });
        }

        [Test]
        public void TestIsCreatingSubsetOf()
        {
            var generator = new InformationTableGenerator();
            var table = generator.GetInformationTable();
            var dr = new ImmutableDecisionRule("a1", "<=", 1.5f, _approximation);
            var f1table = table.Filter(dr);
            _approximation.ApproximatedInformationTable.Returns(f1table);
            _approximation.OriginalInformationTable.Returns(table);
            _approximation.Classes.Returns(new HashSet<int>() { 1, 2 });
            _approximation.GetNegatedApproximatedInformationTable().Returns(f1table.Negation(table).GetAllObjectIdentifiers());


            var dr1 = new ImmutableDecisionRule("a1", "<=", 1f, _approximation);
            dr1.SatisfiesConsistencyLevel(1).ShouldBeTrue();
        }

        [Test]
        public void TestCreateOptimizedRule()
        {
            var generator = new InformationTableGenerator();
            var table = generator.GetInformationTable();
            var dr = new ImmutableDecisionRule("a1", "<=", 1f, _approximation);
            var f1table = table.Filter(dr);
            _approximation.ApproximatedInformationTable.Returns(f1table);
            _approximation.Classes.Returns(new HashSet<int>() { 1, 2 });
            _approximation.GetNegatedApproximatedInformationTable().Returns(f1table.Negation(table).GetAllObjectIdentifiers());

            var dr1 = new ImmutableDecisionRule("a1", "<=", 1.5f, _approximation);
            var optimizedRule = dr.And(dr1).CreateOptimizedRule(0.2f, f1table.GetAllObjectIdentifiers());
            optimizedRule.ToString().ShouldBe("if (f(a1, x) <= 1,5) then x E Cl1<=");
        }

        [Test]
        public void TestIsSatisfiedFor()
        {
            var generator = new InformationTableGenerator();
            var table = generator.GetInformationTable();
            var dr = new ImmutableDecisionRule("a1", "<=", 1.5f, _approximation);

            dr.IsSatisfiedFor(table,"1").ShouldBeTrue();
            dr.IsSatisfiedFor(table,"2").ShouldBeFalse();
        }

        [Test]
        public void TestGetValidClasses()
        {
            var table = new InformationTable(new Dictionary<string, bool> { { "A", false } }, "b", false);
            foreach (var i in Enumerable.Range(1, 3))
            {
                table.AddObject(i.ToString(), new Dictionary<string, float> { { "A", i } }, i);
            }
            var generator = new VCDomLEM();
            var model = generator.GenerateDecisionRules(table, 0);
            var result = Enumerable.Range(1, 3)
                .Select(x => new
                {
                    Id = x,
                    Classes = model.Predict(new[] { x.ToString() }, table).Single()
                }).ToList();

            foreach(var item in result)
            {
                item.Classes.ShouldBe(item.Id);
            }

        }
    }
}
