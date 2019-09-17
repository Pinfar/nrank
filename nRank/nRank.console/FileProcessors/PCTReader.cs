using nRank.PairwiseDRSA;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace nRank.console.FileProcessors
{
    public class PCTReader
    {
        private string _label;
        private string _decision;
        private Dictionary<string, bool> _isAttributeCost;
        private Dictionary<string, Func<string, IAttributePair>> _builders;
        private List<string> _columns;
        private Dictionary<string, bool> _isAttributePair;

        public PairwiseComparisonTable Read(string path)
        {
            var lines = File.ReadAllLines(path).ToList();
            CreatePreferenceDict(lines);
            CreateBaseTable(lines);
            var table = FillTable(lines);
            return table;
        }

        private void CreatePreferenceDict(List<string> lines)
        {
            var preferences = GetSection(lines, "**PREFERENCES");

            _isAttributeCost = preferences
                .Select(x => x.Split(new[] { ": " }, StringSplitOptions.None))
                .ToDictionary(x => x[0], x => x[1] == "cost");
        }

        private void CreateBaseTable(List<string> lines)
        {
            var preferences = GetSection(lines, "**PREFERENCES");
            var attributes = GetSection(lines, "**ATTRIBUTES");

            var regex = new Regex("^[-+] (?<label>.*):\\s+(?<type>(\\[.*\\])|(\\(.*\\)))");

            _columns = attributes.Where(x => regex.IsMatch(x)).Select(x => regex.Match(x).Groups["label"].Value).ToList();
            _isAttributePair = _columns.ToDictionary(x => x, x => true);

            var labelLine = attributes
                .SingleOrDefault(x => x.EndsWith("description"));

            if (labelLine == null) throw new Exception("Description column is required!");
            var label = regex.Match(labelLine).Groups["label"].Value;
            _label = label;
            attributes.Remove(labelLine);

            var decisionLine = attributes
                .SingleOrDefault(x => x.StartsWith("decision"));

            if (decisionLine == null) throw new Exception("Decision column is required!");

            var label1 = decisionLine.Replace("decision:", "").Trim();
            _decision = label1;
            attributes.Remove(decisionLine);
            attributes = attributes.Where(x => !x.StartsWith("+ " + label1) && !x.StartsWith("- " + label1)).ToList();

            _builders = attributes.Select(x => Split(x, regex)).ToDictionary(x => x.Item1, x => x.Item2);
        }

        private Tuple<string, Func<string, IAttributePair>> Split(string value, Regex regex)
        {
            var match = regex.Match(value);

            var label = match.Groups["label"].Value;
            var type = match.Groups["type"].Value;
            Func<string, IAttributePair> creator = null;
            if (type == "(integer)")
            {
                creator = CreateIntCreator(label);
                _isAttributePair[label] = false;
            }
            else if (type == "(continuous)")
            {
                creator = CreateFloatCreator(label);
                _isAttributePair[label] = false;
            }
            else
            {
                creator = CreateOrdinalCreator(label, type);
            }

            return Tuple.Create(label, creator);
        }

        private Func<string, IAttributePair> CreateIntCreator(string name)
        {
            var attType = _isAttributeCost[name] ? AttributeType.Cost : AttributeType.Gain;
            return x =>
            {
                int val = int.Parse(x);
                return new NominalAttributePair(name, new IntPreferable(val, attType));
            };
        }

        private Func<string, IAttributePair> CreateFloatCreator(string name)
        {
            var attType = _isAttributeCost[name] ? AttributeType.Cost : AttributeType.Gain;
            return x =>
            {
                var val = float.Parse(x, CultureInfo.InvariantCulture);
                return new NominalAttributePair(name, new FloatPreferable(val, attType));
            };
        }

        private Func<string, IAttributePair> CreateOrdinalCreator(string name, string values)
        {
            var attType = _isAttributeCost[name] ? AttributeType.Cost : AttributeType.Gain;

            var preference = values
                .Replace("[", "")
                .Replace("]", "")
                .Split(',')
                .Select(y => y.Trim())
                .ToList();

            if (_isAttributeCost[name]) preference.Reverse();
            return x =>
            {
                var pair = x.Split(new[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries).Select(y => y.Trim()).ToList();
                if (pair.Count != 2) throw new InvalidOperationException("Invalid pair");
                return new OrdinalAttributePair( new OrdinalAttribute(name, pair[0], preference), new OrdinalAttribute(name, pair[1], preference));
            };
        }

        private List<string> GetSection(List<string> lines, string name)
        {
            var endMark = new[] { "**END" };

            return lines
                .SkipWhile(x => x != name)
                .Skip(1)
                .TakeWhile(x => !endMark.Contains(x) && !x.StartsWith("**"))
                .Where(x => x.Trim() != "")
                .ToList();
        }

        private PairwiseComparisonTable FillTable(List<string> lines)
        {
            var examples = GetSection(lines, "**EXAMPLES");
            var preferences = GetSection(lines, "**PREFERENCES");
            

            var table = new PairwiseComparisonTable();

            foreach (var line in examples)
            {
                var splitLine = line
                    .Split(new[] { " ", "\t", ",","{","}" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToList();
                var index = 0;
                int firstIdentifier = 0;
                int secondIdentifier = 0;
                PairwiseComparisonTable.RelationType decision = default(PairwiseComparisonTable.RelationType);


                var attributes = new List<IAttributePair>();
                foreach (var column in _columns)
                {
                    if(column == _label)
                    {
                        firstIdentifier = int.Parse(splitLine[index], CultureInfo.InvariantCulture);
                        index++;
                        secondIdentifier = int.Parse(splitLine[index], CultureInfo.InvariantCulture);
                        index++;
                    }
                    else if(column == _decision)
                    {
                        var decisionInt = float.Parse(splitLine[index], CultureInfo.InvariantCulture);
                        index++;
                        if(decisionInt == 0)
                        {
                            decision = PairwiseComparisonTable.RelationType.S;
                        }
                        else
                        {
                            decision = PairwiseComparisonTable.RelationType.Sc;
                        }
                    }
                    else if(_isAttributePair[column])
                    {
                        var value1 = splitLine[index];
                        index++;
                        var value2 = splitLine[index];
                        index++;
                        var attribute = _builders[column]($"{value1},{value2}");
                        attributes.Add(attribute);
                    }
                    else
                    {
                        var attribute = _builders[column](splitLine[index]);
                        index++;
                        attributes.Add(attribute);
                    }
                }


                
                table.Add(attributes,firstIdentifier,secondIdentifier, decision);
            }

            return table;
        }
    }
}
