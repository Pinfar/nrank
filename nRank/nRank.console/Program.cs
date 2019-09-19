using nRank.console.FileProcessors;
using nRank.console.Statistics;
using nRank.VCDomLEMAbstractions;
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
            HandleInputParams(args, out file, out path);

            //var reader = new InformationTableReader();
            //var table = reader.Read(path);
            ////RunExperiment(file, consistencyValue, table);
            //RunRuleGenerationTask(file, consistencyValue, table);
            RunPairRuleGenerationTask(file);

        }

        private static void RunRuleGenerationTask(string file, float consistencyValue, IInformationTable table)
        {
            var vcDomLem = new VCDomLEM(true, true);
            var model = vcDomLem.GenerateDecisionRules(table, consistencyValue);
            var resultDir = Path.GetFileNameWithoutExtension(file);
            Directory.CreateDirectory(Path.Combine(".", resultDir));
            SaveRulesFileAsLatex(resultDir, model, consistencyValue);
        }

        private static void RunPairRuleGenerationTask(string path)
        {
            var vcDomLem = new PairVCDomLEM(false, false);
            var reader = new PairInformationTableReader();
            var configReader = new ConfigurationReader();
            var config = configReader.ReadConfiguration(Path.Combine(path, "experiment.properties"));
            var table = reader.Read(Path.Combine(path, config.LearningDataFile));

            var pairwiseCompTab = new nRank.PairwiseDRSA.PairwiseComparisonTable();
            foreach(var relation in config.Pairs)
            {
                pairwiseCompTab.Add(table.Objects[relation.First-1], relation.Symbol, table.Objects[relation.Second-1]);
            }
            var presentItems =  new HashSet<int>(config.Pairs.SelectMany(x => new[] { x.First-1, x.Second-1 }));
            foreach (var obj in table.Objects.Where((x,i) => presentItems.Contains(i)))
            {
                pairwiseCompTab.Add(obj, PairwiseDRSA.PairwiseComparisonTable.RelationType.S, obj);
            }


            var model = vcDomLem.GenerateDecisionRules(pairwiseCompTab, config.Consistency);
            var resultDir = path;
            Directory.CreateDirectory(Path.Combine(".", resultDir));
            File.WriteAllLines(Path.Combine(path, "rules.txt"), RulesToString(model, pairwiseCompTab));
            File.WriteAllLines(Path.Combine(path, "debug.txt"), vcDomLem.GetDebugData(pairwiseCompTab, config.Consistency));
        }

        private static void RunPairRuleGenerationTaskPCT(string path)
        {
            var vcDomLem = new PairVCDomLEM(false, false);
            var reader = new PCTReader();
            var configReader = new ConfigurationReader();
            var config = configReader.ReadConfiguration(Path.Combine(path, "experiment.properties"));
            var table = reader.Read(Path.Combine(path, config.PCTDataFile));

            var model = vcDomLem.GenerateDecisionRules(table, config.Consistency);
            var resultDir = path;
            Directory.CreateDirectory(Path.Combine(".", resultDir));
            File.WriteAllLines(Path.Combine(path, "rules.txt"), RulesToString(model, table));
            File.WriteAllLines(Path.Combine(path, "debug.txt"), vcDomLem.GetDebugData(table, config.Consistency));
        }

        private static IEnumerable<string> RulesToString(List<PairwiseDRSA.IDecisionRule> model, PairwiseDRSA.PairwiseComparisonTable table)
        {
            return model.Select(x => $"{x.ToString()} {ToSupportList(table, x)}");
        }

        private static string ToSupportList(PairwiseDRSA.PairwiseComparisonTable table, PairwiseDRSA.IDecisionRule rule)
        {
            var supportPairs = table.Filter(rule.AsFunc()).ToLabelList();
            return string.Join(", ", supportPairs);
        }

        private static void RunExperiment(string file, float consistencyValue, IInformationTable table)
        {
            var result = new List<string>
            {
                "label;RMSE dla zbioru uczącego;RMSE dla zbioru testowego;Program Wykonywał się;Wygenerowano reguł",
            };
            for (var i = 0; i < 10; i++)
            {
                var splitter = new Splitter();
                var split = splitter.Split(table, 0.75f);
                result.Add(ConductExperiment(consistencyValue, split, "A", false, false));
                result.Add(ConductExperiment(consistencyValue, split, "B", true, false));
                result.Add(ConductExperiment(consistencyValue, split, "C", false, true));
                result.Add(ConductExperiment(consistencyValue, split, "D", true, true));
            }

            var resultDir = Path.GetFileNameWithoutExtension(file);
            //SaveRulesFile(resultDir, model);
            //SavePredictedFile(resultDir, table, model);
            Directory.CreateDirectory(Path.Combine(".", resultDir));
            SaveResultFile(resultDir, result);
        }

        private static void SavePredictedFile(string resultDir, IInformationTable table, IModel model)
        {
            var predicted = model.Predict(table.GetAllObjectIdentifiers().ToList(), table);
            var all = table
                .GetDecisionAttribute()
                .Zip(predicted, (x, y) => new { shouldbe = x, was = y })
                .Select(x => $"{x.shouldbe.Key};{x.shouldbe.Value};{x.was}");

            File.WriteAllLines(Path.Combine(resultDir, "predicted.csv"), all);
        }

        private static void SaveRulesFile(string resultDir, VCDomLEMAbstractions.IModel model, float consistencyValue)
        {
            var result = model.Rules
                .Select(x => $"{x.ToString()} z a = {x.Accuracy} : {{ {string.Join(", ", x.GetCoveredItems())} }}")
                .ToList();


            
            File.WriteAllLines(Path.Combine(resultDir, $"rules{consistencyValue}.txt"), result);
        }

        private static void SaveRulesFileAsLatex(string resultDir, VCDomLEMAbstractions.IModel model, float consistencyValue)
        {
            var result = model.Rules
                .Select(x => $"{x.ToLatexString()} & {x.Accuracy} & {string.Join(", ", x.GetCoveredItems())}")
                .ToList();



            File.WriteAllLines(Path.Combine(resultDir, $"rules{consistencyValue}.txt"), result);
        }

        private static string ConductExperiment(float consistencyValue, SplitInformationTable split, string label, bool parallelizeApproximationProcessing, bool parallelizeRuleEvaluation)
        {
            var vcDomLem = new VCDomLEM(parallelizeApproximationProcessing, parallelizeRuleEvaluation);
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var model = vcDomLem.GenerateDecisionRules(split.TrainingTable, consistencyValue);

            sw.Stop();

            return GetResultMessage(label, split, model, sw.ElapsedMilliseconds);
        }

        private static string GetResultMessage(string label, SplitInformationTable tables, IModel model, long elapsedMiliseconds)
        {
            var predictedTraining = model.Predict(tables.TrainingTable.GetAllObjectIdentifiers().ToList(), tables.TrainingTable);
            var rmseTraining = Metrics.RMSE(tables.TrainingTable.GetDecisionAttribute().Select(x => x.Value), predictedTraining);

            var predictedTesting = model.Predict(tables.TestingTable.GetAllObjectIdentifiers().ToList(), tables.TestingTable);
            var rmseTesting = Metrics.RMSE(tables.TestingTable.GetDecisionAttribute().Select(x => x.Value), predictedTesting);

            return $"{label};{rmseTraining};{rmseTesting};{elapsedMiliseconds};{model.Rules.Count}";
        }

        private static void SaveResultFile(string resultDir, IEnumerable<string> resultFileContent)
        {
            File.WriteAllLines(Path.Combine(resultDir, "result.txt"), resultFileContent);
        }

        private static void HandleInputParams(string[] args, out string file, out string path)
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
            path = Path.Combine(".", file);
        }
    }
}
