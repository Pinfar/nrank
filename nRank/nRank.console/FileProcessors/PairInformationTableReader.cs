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
    public class PairInformationTableReader
    {
        private string _label;
        private Dictionary<string, bool> _isAttributeCost;
        private Dictionary<string, Func<string, IAttribute>> _builders;
        private List<string> _columns;

        public InformationTable Read(string path)
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

            var regex = new Regex("^[-+] (?<label>.*):\\s+(?<type>[\\(\\[].*[\\]\\)])");

            _columns = attributes.Where(x => regex.IsMatch(x)).Select(x => regex.Match(x).Groups["label"].Value).ToList();

            var labelLine = attributes
                .SingleOrDefault(x => x.EndsWith("description"));

            if (labelLine != null)
            {
                var label = regex.Match(labelLine).Groups["label"].Value;
                _label = label;
                attributes.Remove(labelLine);
            }

            var decisionLine = attributes
                .SingleOrDefault(x => x.StartsWith("decision"));

            if (decisionLine != null)
            {
                var label = decisionLine.Replace("decision:", "").Trim();
                attributes.Remove(decisionLine);
                attributes = attributes.Where(x => !x.StartsWith("+ " + label) && !x.StartsWith("- " + label)).ToList();
            }

            _builders = attributes.Select(x => Split(x, regex)).ToDictionary(x => x.Item1, x => x.Item2);
        }

        private Tuple<string, Func<string, IAttribute>> Split(string value, Regex regex)
        {
            var match = regex.Match(value);

            var label = match.Groups["label"].Value;
            var type = match.Groups["type"].Value;
            Func<string, IAttribute> creator = null;
            if (type == "(integer)")
            {
                creator = CreateIntCreator(label);
            }
            else if (type == "(continuous)")
            {
                creator = CreateFloatCreator(label);
            }
            else
            {
                creator = CreateOrdinalCreator(label, type);
            }

            return Tuple.Create(label, creator);
        }

        private Func<string, IAttribute> CreateIntCreator(string name)
        {
            var attType = _isAttributeCost[name] ? AttributeType.Cost : AttributeType.Gain;
            return x =>
            {
                int val = int.Parse(x);
                return new NominalAttribute(name, new IntValue(val, attType));
            };
        }

        private Func<string, IAttribute> CreateFloatCreator(string name)
        {
            var attType = _isAttributeCost[name] ? AttributeType.Cost : AttributeType.Gain;
            return x =>
            {
                var val = float.Parse(x, CultureInfo.InvariantCulture);
                return new NominalAttribute(name, new FloatValue(val, attType));
            };
        }

        private Func<string, IAttribute> CreateOrdinalCreator(string name, string values)
        {
            var attType = _isAttributeCost[name] ? AttributeType.Cost : AttributeType.Gain;
            return x =>
            {
                var preference = values
                    .Replace("[", "")
                    .Replace("]", "")
                    .Split(',')
                    .Select(y => y.Trim())
                    .ToList();

                if (_isAttributeCost[name]) preference.Reverse();
                return new OrdinalAttribute(name, x, preference);
            };
        }

        private List<string> GetSection(List<string> lines, string name)
        {
            var endMark = new[] { "**END", "" };

            return lines
                .SkipWhile(x => x != name)
                .Skip(1)
                .TakeWhile(x => !endMark.Contains(x))
                .ToList();
        }

        private InformationTable FillTable(List<string> lines)
        {
            var examples = GetSection(lines, "**EXAMPLES");
            var preferences = GetSection(lines, "**PREFERENCES");

            var index = 1;

            var objects = new List<InformationObject>();

            foreach (var line in examples)
            {
                var record = line
                    .Split(new[] { " ", "\t", "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Zip(_columns, (x, y) => new { Key = y, Value = x })
                    .ToDictionary(x => x.Key, x => x.Value);


                string label;
                if (_label != null)
                {
                    label = record[_label];
                    record.Remove(_label);
                }
                else
                {
                    label = index.ToString();
                }
                var attributes = record.Where(x => _builders.ContainsKey(x.Key)).Select(x => _builders[x.Key](x.Value)).ToList();

                //var attributes = new List<IAttribute>();
                var item = new InformationObject(index, label, attributes);

                index++;
                objects.Add(item);
            }

            return new InformationTable(objects);
        }
    }
}
