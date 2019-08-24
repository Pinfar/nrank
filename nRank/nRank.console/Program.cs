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

namespace nRank.console
{
    class Program
    {
        static void Main(string[] args)
        {
            string file, path, consistency;
            HandleInputParams(args, out file, out path, out consistency);
            float consistencyValue = float.Parse(consistency);

            var reader = new InformationTableReader();
            var table = reader.Read(path);

            

            var result = new List<string>
            {
                "label;RMSE dla zbioru uczącego;RMSE dla zbioru testowego;Program Wykonywał się;Wygenerowano reguł",
            };
            for(var i=0;i<10;i++)
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

        private static void SaveRulesFile(string resultDir, VCDomLEMAbstractions.IModel model)
        {
            var result = model.Rules
                .Select(x => $"{x.ToString()} z a = {x.Accuracy}")
                .ToList();


            
            File.WriteAllLines(Path.Combine(resultDir, "rules.txt"), result);
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

        private static void HandleInputParams(string[] args, out string file, out string path, out string consistency)
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
            if (args.Length < 2)
            {
                Console.WriteLine("Insert consistency treshold!");
                consistency = Console.ReadLine();
            }
            else
            {
                consistency = args[1];
            }
        }
    }
}
