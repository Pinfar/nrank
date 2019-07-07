using nRank.Extensions;
using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * 
 ***ATTRIBUTES
+name: (nominal) description
+price: (continuous)
+accel: (continuous)
+pick_up: (continuous)
+brakes: (continuous)
+road_h: (continuous)
+rank: (integer) decision

**PREFERENCES
price: cost
accel: cost
pick_up: cost
brakes: gain
road_h: gain
rank: cost

**EXAMPLES

Fiat_Tipo		18342	30.7	37.2	2.33	3	?
Alfa_33			15335	30.2	41.6	2	2.5	?
Nissan_Sunny		16973	29	34.9	2.66	2.5	2
Mazda_323		15460	30.4	35.8	1.66	1.5	?
Mitsubishi_Colt		15131	29.7	35.6	1.66	1.75	?
Toyota_Corolla		13841	30.8	36.5	1.33	2	?
Honda_Civic		18971	28	35.6	2.33	2	?
Opel_Astra		18319	28.9	35.3	1.66	2	?
Ford_Escort		19800	29.4	34.7	2	1.75	4
Renault_19		16966	30	37.7	2.33	3.25	?
Peugeot_309_16V		17537	28.3	34.8	2.33	2.75	1
Peugeot_309		15980	29.6	35.3	2.33	2.75	?
Mitsubishi_Galant	17219	30.2	36.9	1.66	1.25	3
Renault_21		21334	28.9	36.7	2	2.25	5

**END
 *
 * 
 */
namespace nRank.DataStructures
{
    public class InformationTable : IInformationTable
    {
        public string DecisionAttributeName { get; }
        private readonly bool _isDecisionAttributeCost;

        Dictionary<string, InformationObject> ObjectsStorage = new Dictionary<string, InformationObject>();
        private readonly Dictionary<string, bool> _isAttributeCost;

        public InformationTable(Dictionary<string, bool> isAttributeCost, string decisionAttributeName, bool isDecisionAttributeCost)
        {
            _isAttributeCost = isAttributeCost;
            _isDecisionAttributeCost = isDecisionAttributeCost;
            DecisionAttributeName = decisionAttributeName;
        }

        private InformationTable(Dictionary<string, bool> isAttributeCost, string decisionAttributeName, bool isDecisionAttributeCost, Dictionary<string, InformationObject> objectsStorage)
            :this(isAttributeCost, decisionAttributeName, isDecisionAttributeCost)
        {
            ObjectsStorage = objectsStorage;
        }


        public void AddObject(string identifier, Dictionary<string, float> attributes, int decisionAttributeValue)
        {
            if (ObjectsStorage.ContainsKey(identifier)) throw new InvalidOperationException("Object identifiers must be unique!");

            ObjectsStorage.Add(identifier, new InformationObject
            {
                Identifier = identifier,
                DecisionAttributeName = DecisionAttributeName,
                DecisionAttributeValue = decisionAttributeValue,
                Attributes = attributes.ToDictionary(
                    x => x.Key, 
                    x => x.Value
                )
            });
        }

        public Dictionary<string, int> GetDecisionAttribute()
        {
            return ObjectsStorage.ToDictionary(x=>x.Key, x => x.Value.DecisionAttributeValue);
        }

        public IEnumerable<float> GetAttribute(string name)
        {
            return ObjectsStorage.Select(x => x.Value.Attributes[name]).ToList();
        }

        public Dictionary<string, float> GetObjectAttributes(string identifier)
        {
            return ObjectsStorage[identifier].Attributes;
        }

        public IEnumerable<string> GetAllObjectIdentifiers()
        {
            return ObjectsStorage.Keys;
        }

        public IInformationTable Filter(Dictionary<string, bool> pattern)
        {
            var filteredObjects = ObjectsStorage
                .Where(x => pattern.ContainsKey(x.Key) && pattern[x.Key])
                .ToDictionary(x => x.Key, x => x.Value);
            var table = new InformationTable(_isAttributeCost, DecisionAttributeName, _isDecisionAttributeCost, filteredObjects);
            return table;
        }

        public IEnumerable<int> GetDecicionClassesWorstFirst()
        {
            return GetDecisionAttribute()
                .Values
                .Distinct()
                .OrderBy(x => _isDecisionAttributeCost ? -x : x)
                .ToList();
        }

        public bool Outranks(string identifier1, string identifier2)
        {
            if (!ObjectsStorage.ContainsKey(identifier1)) throw new InvalidOperationException($"{identifier1} - information table does not contain this object!");
            if (!ObjectsStorage.ContainsKey(identifier2)) throw new InvalidOperationException($"{identifier2} - information table does not contain this object!");
            var object1 = ObjectsStorage[identifier1];
            var object2 = ObjectsStorage[identifier2];            
            return _isAttributeCost.All(x => IsBetterOnAttribute(object1, object2, x.Key));
        }

        private bool IsBetterOnAttribute(InformationObject object1, InformationObject object2, string attributeName)
        {
            var attributeValue1 = object1.Attributes[attributeName];
            var attributeValue2 = object2.Attributes[attributeName];
            return _isAttributeCost[attributeName] ? 
                attributeValue1 <= attributeValue2 : 
                attributeValue1 >= attributeValue2;
        }

        public IInformationTable Filter(IDecisionRule rule)
        {
            var pattern = rule.Satisfy(this);
            return Filter(pattern);
        }

        public IEnumerable<string> GetAllAttributes()
        {
            return _isAttributeCost.Keys;
        }

        public IInformationTable Intersect(IInformationTable table)
        {
            var pattern = ObjectsStorage.Keys.ToDictionary(x => x, x => true);
            return table.Filter(pattern);
        }

        public IInformationTable Negation(IInformationTable originalTable)
        {
            var selfObjects = GetAllObjectIdentifiers().ToList();
            var filter = originalTable.GetAllObjectIdentifiers().ToDictionary(x => x, x => !selfObjects.Contains(x));
            return originalTable.Filter(filter);
        }

        public int Count()
        {
            return ObjectsStorage.Count();
        }

        public bool IsAttributeCost(string name)
        {
            return _isAttributeCost[name];
        }
    }
}
