using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.DSetGenerators
{
    //D+B(x)
    class DDominatingSetGenerator : IDDSetGenerator
    {
        public IInformationTable Generate(IInformationTable informationTable, string objectIdentifier)
        {
            var mask = informationTable.GetAllObjectIdentifiers()
                .ToDictionary(x =>x, x => informationTable.Outranks(x, objectIdentifier));

            return informationTable.Filter(mask);
        }
    }
}
