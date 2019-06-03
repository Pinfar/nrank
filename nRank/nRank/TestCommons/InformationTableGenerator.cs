using nRank.DataStructures;
using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.TestCommons
{
    class InformationTableGenerator
    {
        public IInformationTable GetInformationTable()
        {
            var informationTable = new InformationTable(
                new Dictionary<string, bool> { { "a1", false }, { "a2", false }, { "a3", false } },
                "d",
                false);
            var items = new[]
            {
                new [] { 1f, 1.5f, 3f, 12f, 2f },
                new [] { 2f, 1.7f, 5f, 9.5f, 2f },
                new [] { 3f, 0.5f, 2f, 2.5f, 1f },
                new [] { 4f, 0.7f, 0.5f, 1.5f, 1f },
                new [] { 5f, 3f, 4.3f, 9f, 3f },
                new [] { 6f, 1f, 2f, 4.5f, 2f },
                new [] { 7f, 1f, 1.2f, 8f, 1f },
                new [] { 8f, 2.3f, 3.3f, 9f, 3f },
                new [] { 9f, 1f, 3f, 5f, 1f },
                new [] { 10f, 1.7f, 2.8f, 3.5f, 2f },
                new [] { 11f, 2.5f, 4f, 11f, 2f },
                new [] { 12f, 0.5f, 3f, 6f, 2f },
                new [] { 13f, 1.2f, 1f, 7f, 2f },
                new [] { 14f, 2f, 2.4f, 6f, 1f },
                new [] { 15f, 1.9f, 4.3f, 14f, 2f },
                new [] { 16f, 2.3f, 4f, 13f, 3f },
                new [] { 17f, 2.7f, 5.5f, 15f, 3f },
            };
            foreach (var item in items)
            {
                informationTable.AddObject(
                    item[0].ToString(),
                    new Dictionary<string, float> { { "a1", item[1] }, { "a2", item[2] }, { "a3", item[3] } },
                    (int)item[4]);
            }
            return informationTable;
        }
    }
}
