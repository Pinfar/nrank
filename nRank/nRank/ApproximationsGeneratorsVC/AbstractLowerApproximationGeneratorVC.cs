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
        abstract protected string GetSymbol(IEnumerable<int> classes);

        private T dsetGenerator = new T();

        public IApproximation GetApproximation(IUnion union, IInformationTable originalTable)
        {
            var objectsInUnion = union.InformationTable.GetAllObjectIdentifiers().ToList();
            var pattern = originalTable.GetAllObjectIdentifiers()
                .ToDictionary(
                    x => x,
                    x => IsInApproximation(originalTable, x, objectsInUnion)
                );
            return new Approximation(originalTable.Filter(pattern), originalTable, union.Classes, _allowedOperators, GetSymbol(union.Classes));
        }

        private bool IsInApproximation(IInformationTable originalTable, string objectId, IList<string> objectsInUnion)
        {
            var dset = dsetGenerator.Generate(originalTable, objectId).GetAllObjectIdentifiers().ToList();
            float commonPart = dset.Intersect(objectsInUnion).Count();
            float dsetCount = dset.Count();
            return (commonPart / dsetCount) >= 1;
        }
    }
}
