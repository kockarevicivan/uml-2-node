using Newtonsoft.Json;

namespace Uml2Node.Core.Model
{
    public class Field
    {
        public string Name { get; set; }

        [JsonIgnore]
        public string CamelCaseName { get; set; }
        [JsonIgnore]
        public string DashCaseName { get; set; }
        [JsonIgnore]
        public string PascalCaseName { get; set; }

        public string Type { get; set; }
    }
}
