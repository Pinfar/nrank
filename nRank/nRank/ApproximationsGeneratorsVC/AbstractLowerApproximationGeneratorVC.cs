using nRank.DataStructures;
using nRank.DSetGenerators;
using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.ApproximationsGeneratorsVC
{
    abstract class AbstractLowerApproximationGeneratorVC<T> where T : IDDSetGenerator, new()
    {
        abstract protected IEnumerable<string> _allowedOperators { get; }

        private T dsetGenerator = new T();

        public IApproximation GetApproximation(IUnion union, IInformationTable originalTable, float consistencyLevel)
        {
            var objectsInUnion = union.InformationTable.GetAllObjectIdentifiers().ToList();
            var pattern = originalTable.GetAllObjectIdentifiers()
                .ToDictionary(
                    x => x,
                    x => IsInApproximation(originalTable, x, objectsInUnion, consistencyLevel)
                );
            return new Approximation(originalTable.Filter(pattern), originalTable, union.Classes, _allowedOperators, union.Symbol);
        }

        private bool IsInApproximation(IInformationTable originalTable, string objectId, IList<string> objectsInUnion, float consistencyLevel)
        {
            var dset = dsetGenerator.Generate(originalTable, objectId).GetAllObjectIdentifiers().ToList();
            float commonPart = dset.Intersect(objectsInUnion).Count();
            float dsetCount = dset.Count();
            return (commonPart / dsetCount) >= consistencyLevel;
        }
    }
}
