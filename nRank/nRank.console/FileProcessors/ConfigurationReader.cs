using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace nRank.console.FileProcessors
{
    public class ConfigurationReader
    {
        public Configuration ReadConfiguration(string path)
        {
            var lines = File.ReadAllLines(path).ToList();
            var config = new Configuration
            {
                LearningDataFile = GetLearningFileName(lines),
                Pairs = GetPairs(lines),
                Consistency = GetConsistency(lines)
            };
            return config;
        }

        private float GetConsistency(List<string> lines)
        {
            var linePrefix = "consistencyMeasureThreshold = ";
            var consistenctString = lines.FirstOrDefault(x => x.StartsWith(linePrefix));
            if (consistenctString == null) throw new InvalidOperationException("ConsistencyMeasureThreshold in settings file is not set!");
            consistenctString = consistenctString.Replace(linePrefix, "").Trim();
            return float.Parse(consistenctString, CultureInfo.InvariantCulture);
        }

        private List<Relation> GetPairs(List<string> lines)
        {
            var linePrefix = "pairs = ";
            if (lines.Any(x => x.StartsWith(linePrefix)))
            {
                var pairsLine = lines.First(x => x.StartsWith(linePrefix)).Replace(linePrefix, "").Trim();

                var regex = new Regex("\\{(?<n1>[0-9]+),(?<n2>[0-9]+)\\} (?<s>(Sc)|S)");
                var pairs = pairsLine.Split(new[] { ", " }, StringSplitOptions.None)
                    .Select(x => regex.Match(x))
                    .Select(x => new Relation
                    {
                        First = int.Parse(x.Groups["n1"].Value),
                        Second = int.Parse(x.Groups["n2"].Value),
                        Symbol = (x.Groups["s"].Value == "S") ? PairwiseDRSA.PairwiseComparisonTable.RelationType.S : PairwiseDRSA.PairwiseComparisonTable.RelationType.Sc
                    })
                    .ToList();

                return pairs;
            }
            else
            {
                var otherlinePrefix = "referenceRanking = ";
                var pairsLine = lines.First(x => x.StartsWith(otherlinePrefix)).Replace(otherlinePrefix, "").Trim();

                var numbers = pairsLine.Split(new[] { ", " }, StringSplitOptions.None)
                    .Select(x => x.Split(new[] { " " }, StringSplitOptions.None).Select(int.Parse).ToList())
                    .ToList();

                var preferedItems = new List<int>();
                var relations = new List<Relation>();


                foreach (var line in numbers)
                {
                    var tempRelations = line.SelectMany(x => preferedItems, (x, y) => new[] {
                        new Relation
                        {
                            First = x,
                            Second = y,
                            Symbol = PairwiseDRSA.PairwiseComparisonTable.RelationType.Sc
                        },
                            new Relation
                        {
                            First = y,
                            Second = x,
                            Symbol = PairwiseDRSA.PairwiseComparisonTable.RelationType.S
                        }
                    }).SelectMany(x => x);
                    relations.AddRange(tempRelations);
                    preferedItems.AddRange(line);

                    var newRelations = line
                        .SelectMany(x => line, (x, y) => new { x, y })
                        .Where(x => x.x != x.y)
                        .Select(x => new Relation
                            {
                                First = x.x,
                                Second = x.y,
                                Symbol = PairwiseDRSA.PairwiseComparisonTable.RelationType.S
                            }
                        );

                    relations.AddRange(newRelations);
                }

                return relations;
            }
        }

        private string GetLearningFileName(List<string> lines)
        {
            var linePrefix = "learningDataFile = ";
            return lines.First(x => x.StartsWith(linePrefix)).Replace(linePrefix, "").Trim();
        }
    }
}
