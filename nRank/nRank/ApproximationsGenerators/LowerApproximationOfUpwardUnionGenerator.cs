using nRank.DSetGenerators;
using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.ApproximationsGenerators
{
    //B_(Clt>=)
    class LowerApproximationOfUpwardUnionGenerator : IApproximationsGenerator
    {
        public IInformationTable GetApproximation(IInformationTable union, IInformationTable originalTable)
        {
            var dsetGenerator = new DDominatingSetGenerator();
            var objectsInUnion = union.GetAllObjectIdentifiers();
            var pattern = originalTable.GetAllObjectIdentifiers()
                .ToDictionary(
                    x => x,
                    x => dsetGenerator.Generate(originalTable, x).GetAllObjectIdentifiers().All(y => objectsInUnion.Contains(y))
                );
            return originalTable.Filter(pattern);

        }
    }
}
