using nRank.TestCommons;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.DecisionRules
{
    [TestFixture]
    class TestDecisionRule
    {
        [Test]
        public void TestToString()
        {
            var dr = new ImmutableDecisionRule("att1", "<=",3.5f, "1<=");
            dr.ToString().ShouldBe("if (f(att1, x) <= 3,5) then x E Cl1<=");
        }

        [Test]
        public void TestAnd()
        {
            var dr = new ImmutableDecisionRule("att1", "<=", 3.5f, "1<=").And("att2", "<=", 4.5f);
            dr.ToString().ShouldBe("if (f(att1, x) <= 3,5) and (f(att2, x) <= 4,5) then x E Cl1<=");
        }

        [Test]
        public void TestSatisfy()
        {
            var generator = new InformationTableGenerator();
            var table = generator.GetInformationTable();
            var dr = new ImmutableDecisionRule("a1", "<=", 1.5f, "1<=");

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
        public void TestSatisfyWithAnd()
        {
            var generator = new InformationTableGenerator();
            var table = generator.GetInformationTable();
            var dr = new ImmutableDecisionRule("a1", "<=", 1.5f, "1<=").And("a3", ">=", 12f);

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
            var dr = ImmutableDecisionRule.GetAlwaysTrueRule("1<=");

            dr.Satisfy(table).Values.ShouldAllBe(x => true);
        }

        [Test]
        public void TestIsEmpty()
        {
            new ImmutableDecisionRule("a1", "<=", 1.5f, "1<=").IsEmpty().ShouldBeFalse();
            ImmutableDecisionRule.GetAlwaysTrueRule("1<=").IsEmpty().ShouldBeTrue();
            ImmutableDecisionRule.GetAlwaysTrueRule("1<=").And("a3", ">=", 12f).IsEmpty().ShouldBeFalse();
        }

        [Test]
        public void TestSatisfyWithAndRules()
        {
            var generator = new InformationTableGenerator();
            var table = generator.GetInformationTable();
            var dr1 = new ImmutableDecisionRule("a1", "<=", 1.5f, "1<=");
            var dr2 = new ImmutableDecisionRule("a3", ">=", 12f, "1<=");
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
            var dr = new ImmutableDecisionRule("a1", "<=", 1.5f, "1<=");
            var f1table = table.Filter(dr);

            var dr1 = new ImmutableDecisionRule("a1", "<=", 1f, "1<=");
            dr1.IsCreatingSubsetOf(table, f1table);
        }
    }
}
