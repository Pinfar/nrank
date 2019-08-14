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
        abstract protected string _allowedGainOperator { get; }

        private T dsetGenerator = new T();

        public IApproximation GetApproximation(IUnion union, IInformationTable originalTable, float consistencyLevel)
        {
            var objectsInUnion = union.InformationTable.GetAllObjectIdentifiers();
            var objectsInUnionSet = new HashSet<string>(objectsInUnion);
            var pattern = originalTable.GetAllObjectIdentifiers()
                .ToDictionary(
                    x => x,
                    x => objectsInUnionSet.Contains(x) && IsInApproximationEpsilon(originalTable, x, objectsInUnionSet, consistencyLevel)
                );
            var approximation = originalTable.Filter(pattern);
            var positiveRegion = approximation
                .GetAllObjectIdentifiers()
                .SelectMany(x => dsetGenerator.Generate(originalTable, x).GetAllObjectIdentifiers())
                .Distinct()
                .ToList();
            return new Approximation(approximation, originalTable, union.Classes, _allowedGainOperator, union.Symbol, union, positiveRegion);
        }

        private bool IsInApproximation(IInformationTable originalTable, string objectId, IList<string> objectsInUnion, float consistencyLevel)
        {
            var dset = dsetGenerator.Generate(originalTable, objectId).GetAllObjectIdentifiers().ToList();
            float commonPart = dset.Intersect(objectsInUnion).Count();
            float dsetCount = dset.Count();
            return (commonPart / dsetCount) >= consistencyLevel;
        }

        private bool IsInApproximationEpsilon(IInformationTable originalTable, string objectId, HashSet<string> objectsInUnion, float consistencyLevel)
        {
            var dset = new HashSet<string>(dsetGenerator.Generate(originalTable, objectId).GetAllObjectIdentifiers());
            var negSet = originalTable.GetAllObjectIdentifiers()
                .Where(x => !objectsInUnion.Contains(x))
                .ToList();
            float commonPart = dset.Intersect(negSet).Count();
            float negSetCount = negSet.Count();
            return (commonPart / negSetCount) <= consistencyLevel;
        }
    }
}
