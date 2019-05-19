using Newtonsoft.Json;
using System.Collections.Generic;

namespace Uml2Node.Core.Model
{
    public class Entity
    {
        public string Name { get; set; }

        [JsonIgnore]
        public string CamelCaseName { get; set; }
        [JsonIgnore]
        public string DashCaseName { get; set; }
        [JsonIgnore]
        public string PascalCaseName { get; set; }

        public List<Field> Fields { get; set; }
    }
}
