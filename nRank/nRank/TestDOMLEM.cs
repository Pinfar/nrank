using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank
{
    [TestFixture]
    class TestDOMLEM
    {
        [Test]
        public void GenerateDecisionRules()
        {
            var decisionRuleGenerator = Substitute.For<IDecisionRuleGenerator>();
            var roughSetGenerator = Substitute.For<IRoughSetGenerator>();
            var informationTable = Substitute.For<IInformationTable>();
            var lowerApproximation1 = Substitute.For<ILowerApproximation>();
            var lowerApproximation2 = Substitute.For<ILowerApproximation>();
            roughSetGenerator
                .GenerateLowerApproximations(informationTable)
                .Returns(new List<ILowerApproximation> { lowerApproximation1, lowerApproximation2 });
            var rule1 = Substitute.For<IDecisionRule>();
            var rule2 = Substitute.For<IDecisionRule>();
            var rule3 = Substitute.For<IDecisionRule>();
            decisionRuleGenerator
                .GenerateMinimalRulesFrom(lowerApproximation1)
                .Returns(new List<IDecisionRule> { rule1, rule2 });
            decisionRuleGenerator
                .GenerateMinimalRulesFrom(lowerApproximation2)
                .Returns(new List<IDecisionRule> { rule2, rule3 });

            var domlem = new DOMLEM(roughSetGenerator, decisionRuleGenerator);
            var generatedRules = domlem.GenerateDecisionRules(informationTable);

            generatedRules.ShouldBe(new HashSet<IDecisionRule> { rule1, rule2, rule3 });
        }
    }
}
