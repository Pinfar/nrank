using nRank.console.FileProcessors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;

namespace nRank.console
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            string file, path;
            bool debug;
            HandleInputParams(args, out file, out path, out debug);

            //var reader = new InformationTableReader();
            //var table = reader.Read(path);
            ////RunExperiment(file, consistencyValue, table);
            //RunRuleGenerationTask(file, consistencyValue, table);
            RunPairRuleGenerationTask(file, debug);

        }

        private static void RunPairRuleGenerationTask(string path, bool writeDebug)
        {
            var vcDomLem = new PairVCDomLEM(true, true);
            var configReader = new ConfigurationReader();
            var config = configReader.ReadConfiguration(Path.Combine(path, "experiment.properties"));
            nRank.PairwiseDRSA.PairwiseComparisonTable pairwiseCompTab;
            if (config.PCTDataFile == null)
            {
                var reader = new PairInformationTableReader();
                var table = reader.Read(Path.Combine(path, config.LearningDataFile));

                pairwiseCompTab = new nRank.PairwiseDRSA.PairwiseComparisonTable();
                foreach (var relation in config.Pairs)
                {
                    pairwiseCompTab.Add(table.Objects[relation.First - 1], relation.Symbol, table.Objects[relation.Second - 1]);
                }
                var presentItems = new HashSet<int>(config.Pairs.SelectMany(x => new[] { x.First - 1, x.Second - 1 }));
                foreach (var obj in table.Objects.Where((x, i) => presentItems.Contains(i)))
                {
                    pairwiseCompTab.Add(obj, PairwiseDRSA.PairwiseComparisonTable.RelationType.S, obj);
                }
            }
            else
            {
                var reader = new PCTReader();
                pairwiseCompTab = reader.Read(Path.Combine(path, config.PCTDataFile));
            }

            Stopwatch sw = new Stopwatch();

            sw.Start();

            var model = vcDomLem.GenerateDecisionRules(pairwiseCompTab, config.Consistency);

            sw.Stop();

            var resultDir = path;
            Directory.CreateDirectory(Path.Combine(".", resultDir));
            
            File.WriteAllLines(Path.Combine(path, "rules.txt"), RulesToString(model, pairwiseCompTab, writeDebug));
            IEnumerable<string> otherDebug = new List<string> 
            {
                $"Time elapsed: {sw.ElapsedMilliseconds }" ,
                $"PCT size: {pairwiseCompTab.AsInformationObjectPairs().Count}",
                $"S relation items count: {pairwiseCompTab.Filter(x => x.Relation == PairwiseDRSA.PairwiseComparisonTable.RelationType.S).AsInformationObjectPairs().Count}",
                $"Sc relation items count: {pairwiseCompTab.Filter(x => x.Relation == PairwiseDRSA.PairwiseComparisonTable.RelationType.Sc).AsInformationObjectPairs().Count}",
                $"Generated rules count: {model.Count}"
            };
            var debugData = writeDebug? vcDomLem.GetDebugData(pairwiseCompTab, config.Consistency) : new List<string>();
            File.WriteAllLines(Path.Combine(path, "debug.txt"), otherDebug.Concat(debugData));
        }

        private static IEnumerable<string> RulesToString(List<PairwiseDRSA.IDecisionRule> model, PairwiseDRSA.PairwiseComparisonTable table, bool debugMode)
        {
            if (debugMode)
            {
                return model.Select(x => $"{x.ToString()} {ToSupportList(table, x)}");
            }
            else
            {
                return model.Select(x => $"{x.ToString()}");
            }
        }

        private static string ToSupportList(PairwiseDRSA.PairwiseComparisonTable table, PairwiseDRSA.IDecisionRule rule)
        {
            var supportPairs = table.Filter(rule.AsFunc()).ToLabelList();
            return string.Join(", ", supportPairs);
        }


        private static void HandleInputParams(string[] args, out string file, out string path, out bool debug)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Insert experiment file name!");
                file = Console.ReadLine();
            }
            else
            {
                file = args[0];
            }

            if (args.Length >= 2)
            {
                debug = args[1] == "debug";
            }
            else
            {
                debug = false;
            }

            path = Path.Combine(".", file);
        }
    }
}
