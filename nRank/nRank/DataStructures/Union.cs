using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.DataStructures
{
    class Union : IUnion
    {
        public Union(IInformationTable informationTable, int[] classes, bool isUpward)
        {
            InformationTable = informationTable;
            Classes = classes;
            IsUpward = isUpward;
        }

        public IInformationTable InformationTable { get; }
        public int[] Classes { get; }
        public bool IsUpward { get; }
    }
}
