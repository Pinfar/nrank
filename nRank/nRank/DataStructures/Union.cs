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
        public Union(IInformationTable informationTable, IEnumerable<int> classes, bool isUpward)
        {
            InformationTable = informationTable;
            Classes = new HashSet<int>(classes);
            IsUpward = isUpward;
        }

        public IInformationTable InformationTable { get; }
        public ISet<int> Classes { get; }
        public bool IsUpward { get; }
    }
}
