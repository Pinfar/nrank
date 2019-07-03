﻿using nRank.DataStructures;
using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.UnionGenerators
{
    class DownwardUnionGenerator : IUnionGenerator
    {
        public IEnumerable<IUnion> GenerateUnions(IInformationTable informationTable)
        {
            var classes = informationTable.GetDecicionClassesWorstFirst().ToList();
            var decisionAttributeValues = informationTable.GetDecisionAttribute();
            var classesDict = classes
                .Select((classNr, position) => new { classNr, position })
                .ToDictionary(x => x.classNr, x => x.position);
            var coveredClasses = new List<int>();
            var classesPairs = classes.Take(classes.Count - 1).Zip(classes.Skip(1), Tuple.Create);
            foreach (var classPair in classesPairs)
            {
                var classNr = classPair.Item1;
                coveredClasses.Add(classNr);
                var minPosition = classesDict[classNr];
                var filterPattern = decisionAttributeValues.ToDictionary(x => x.Key, x => classesDict[x.Value] <= minPosition);
                yield return new Union(informationTable.Filter(filterPattern), coveredClasses, false, $"Cl{classNr}<=",$"Cl{classPair.Item2}>=");
            }
        }

        public IDictionary<string, IUnion> GenerateUnionsAsDict(IInformationTable informationTable)
        {
            return GenerateUnions(informationTable).ToDictionary(x => x.Symbol);
        }
    }
}
