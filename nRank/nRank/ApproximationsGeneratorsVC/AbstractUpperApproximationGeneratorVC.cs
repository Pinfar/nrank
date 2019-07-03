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
    abstract class AbstractUpperApproximationGeneratorVC<T> where T : IDDSetGenerator, new()
    {
        abstract protected AbstractLowerApproximationGeneratorVC<T> _oppositeGenerator { get; }
        abstract protected IEnumerable<string> _allowedOperators { get; }

        public IApproximation GetApproximation(IUnion union, IInformationTable originalTable, float consistencyLevel, IDictionary<string, IUnion> unionDict)
        {
            var oppositeApproximation = _oppositeGenerator.GetApproximation(unionDict[union.OppositeSymbol], originalTable, consistencyLevel);
            var filterDict = originalTable.GetAllObjectIdentifiers().ToDictionary(x => x, x => true);
            foreach (var objectId in oppositeApproximation.ApproximatedInformationTable.GetAllObjectIdentifiers())
            {
                filterDict[objectId] = false;
            }
            var approximation = originalTable.Filter(filterDict);
            return new Approximation(approximation, originalTable, union.Classes, _allowedOperators, union.Symbol);
        }
    }
}
