using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class InformationTable
    {
        public InformationTable(IEnumerable<InformationObject> objects)
        {
            Objects = objects.ToList();
        }

        public List<InformationObject> Objects { get; }
    }
}
