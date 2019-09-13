using System;
using System.Collections.Generic;
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
                Pairs = GetPairs(lines)
            };
            return config;
        }

        private List<Relation> GetPairs(List<string> lines)
        {
            var linePrefix = "pairs = ";
            var pairsLine = lines.First(x => x.StartsWith(linePrefix)).Replace(linePrefix, "").Trim();

            var regex = new Regex( "\\{(?<n1>[0-9]+),(?<n2>[0-9]+)\\} (?<s>(Sc)|S)");
            var pairs = pairsLine.Split(new[] { ", " }, StringSplitOptions.None)
                .Select(x => regex.Match(x))
                .Select(x => new Relation
                {
                    First = int.Parse(x.Groups["n1"].Value),
                    Second = int.Parse(x.Groups["n2"].Value),
                    Symbol = (x.Groups["s"].Value == "S")? PairwiseDRSA.PairwiseComparisonTable.RelationType.S : PairwiseDRSA.PairwiseComparisonTable.RelationType.Sc
                })
                .ToList();

            return pairs;
        }

        private string GetLearningFileName(List<string> lines)
        {
            var linePrefix = "learningDataFile = ";
            return lines.First(x => x.StartsWith(linePrefix)).Replace(linePrefix, "").Trim();
        }
    }
}
