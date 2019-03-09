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
    class InformationTable : IInformationTable
    {
        public string DecisionAttributeName => DecisionAttributeStorage.Name;

        Dictionary<string, InformationAttribute<float>> AttributesStorage = new Dictionary<string, InformationAttribute<float>>();
        InformationAttribute<int> DecisionAttributeStorage;

        public InformationTable() { }

        private InformationTable(
            Dictionary<string, InformationAttribute<float>> attributesStorage,
            InformationAttribute<int> decisionAttributeStorage
        )
        {
            AttributesStorage = attributesStorage;
            DecisionAttributeStorage = decisionAttributeStorage;
        }

        public void AddDecisionAttribute(string name, IEnumerable<int> values, bool isCost = true)
        {
            if (DecisionAttributeStorage != null)
            {
                throw new InvalidOperationException("Multiple decision attributes not supported");
            }
            DecisionAttributeStorage = new InformationAttribute<int>
            {
                Name = name,
                IsCost = isCost,
                Values = values.ToList()
            };
        }

        public IEnumerable<int> GetDecisionAttribute()
        {
            return DecisionAttributeStorage.Values;
        }

        public void AddAttribute(string name, IEnumerable<float> values, bool isCost = true)
        {
            AttributesStorage.Add(name, new InformationAttribute<float>
            {
                Name = name,
                IsCost = isCost,
                Values = values.ToList()
            });
        }

        public IEnumerable<float> GetAttribute(string name)
        {
            return AttributesStorage[name].Values;
        }

        public IInformationTable Filter(IEnumerable<bool> pattern)
        {
            var filteredAttributesStorage = AttributesStorage.ToDictionary(
                x => x.Key,
                x => x.Value.WhereIsTrue(pattern)
            );
            var filteredDecisionAttributeStorage = DecisionAttributeStorage.WhereIsTrue(pattern);
            var table = new InformationTable(filteredAttributesStorage, filteredDecisionAttributeStorage);
            return table;
        }

        public IEnumerable<int> GetDecicionClassesWorstFirst()
        {
            return DecisionAttributeStorage
                .Values
                .Distinct()
                .OrderBy(x => DecisionAttributeStorage.IsCost? -x : x)
                .ToList();
        }
    }
}
