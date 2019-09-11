using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using nRank.PairwiseDRSA;
using NSubstitute;

namespace nRankTests.DataStructures
{
    [TestFixture]
    class InformationObjectTests
    {
        [TestCase("best", "bad", "good", "average", true)]
        [TestCase("best", "average", "good", "bad", false)]
        public void DominationRelationTestOnOrdinal(string val1, string val2, string val3, string val4, bool result)
        {
            var order = new List<string> { "bad", "average", "good", "best" };
            var att1 = new List<IAttribute>
            {
                new OrdinalAttribute("Att1", val1, order)
            };
            var obj1 = new InformationObject(1, "1", att1);
            var att2 = new List<IAttribute>
            {
                new OrdinalAttribute("Att1", val2, order)
            };
            var obj2 = new InformationObject(2, "2", att2);
            var att3 = new List<IAttribute>
            {
                new OrdinalAttribute("Att1", val3, order)
            };
            var obj3 = new InformationObject(3, "3", att3);
            var att4 = new List<IAttribute>
            {
                new OrdinalAttribute("Att1", val4, order)
            };
            var obj4 = new InformationObject(4, "4", att4);

            var pair1 = new InformationObjectPair(obj1, obj2);
            var pair2 = new InformationObjectPair(obj3, obj4);
            pair1.Dominates(pair2).ShouldBe(result);

        }

        [TestCase(10,1,9,2,true)]
        [TestCase(10,5,9,2,false)]
        public void DominationRelationTestOnNominal(int val1, int val2, int val3, int val4, bool result)
        {
            var att1 = new List<IAttribute>
            {
                new NominalAttribute("Att1", new IntValue(val1, AttributeType.Gain))
            };
            var obj1 = new InformationObject(1, "1", att1);
            var att2 = new List<IAttribute>
            {
                new NominalAttribute("Att1", new IntValue(val2, AttributeType.Gain))
            };
            var obj2 = new InformationObject(2, "2", att2);
            var att3 = new List<IAttribute>
            {
                new NominalAttribute("Att1", new IntValue(val3, AttributeType.Gain))
            };
            var obj3 = new InformationObject(3, "3", att3);
            var att4 = new List<IAttribute>
            {
                new NominalAttribute("Att1", new IntValue(val4, AttributeType.Gain))
            };
            var obj4 = new InformationObject(4, "4", att4);

            var pair1 = new InformationObjectPair(obj1, obj2);
            var pair2 = new InformationObjectPair(obj3, obj4);
            pair1.Dominates(pair2).ShouldBe(result);
        }
    }
}
