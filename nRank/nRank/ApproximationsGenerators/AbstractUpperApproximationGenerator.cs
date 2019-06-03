using nRank.DSetGenerators;
using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.ApproximationsGenerators
{
    abstract class AbstractUpperApproximationGenerator<T> : IApproximationsGenerator where T : IDDSetGenerator, new()
    {
        public IInformationTable GetApproximation(IInformationTable union, IInformationTable originalTable)
        {
            var dsetGenerator = new T();
            var objectsInUnion = union.GetAllObjectIdentifiers();
            var approximation = objectsInUnion
                .SelectMany(x => dsetGenerator.Generate(originalTable, x).GetAllObjectIdentifiers())
                .Distinct();
            var pattern = originalTable.GetAllObjectIdentifiers()
                .ToDictionary(
                    x => x,
                    x => approximation.Contains(x)
                );
            return originalTable.Filter(pattern);
        }
    }
}
