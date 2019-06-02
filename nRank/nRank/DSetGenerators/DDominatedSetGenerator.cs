using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.DSetGenerators
{
    //D-B(x)
    class DDominatedSetGenerator
    {
        public IInformationTable Generate(IInformationTable informationTable, string objectIdentifier)
        {
            var mask = informationTable.GetAllObjectIdentifiers()
                .ToDictionary(x => x, x => informationTable.Outranks(objectIdentifier, x));

            return informationTable.Filter(mask);
        }
    }
}
