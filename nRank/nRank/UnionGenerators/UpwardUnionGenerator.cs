using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.UnionGenerators
{
    class UpwardUnionGenerator : IUnionGenerator
    {
        public IEnumerable<IInformationTable> GenerateUnions(IInformationTable informationTable)
        {
            var classes = informationTable.GetDecicionClassesWorstFirst();
            var decisionAttributeValues = informationTable.GetDecisionAttribute().ToList();
            var classesDict = classes
                .Select((classNr, position) => new { classNr, position })
                .ToDictionary(x => x.classNr, x => x.position);

            foreach(var classNr in classes.Skip(1))
            {
                var minPosition = classesDict[classNr];
                var filterPattern = decisionAttributeValues.Select(x => classesDict[x] >= minPosition);
                yield return informationTable.Filter(filterPattern);
            }
        }
    }
}
