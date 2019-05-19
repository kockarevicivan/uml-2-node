using Newtonsoft.Json;
using System.Collections.Generic;

namespace Uml2Node.Core.Model
{
    public class Project
    {
        public string Name { get; set; }

        [JsonIgnore]
        public string CamelCaseName { get; set; }
        [JsonIgnore]
        public string PascalCaseName { get; set; }
        [JsonIgnore]
        public string DashCaseName { get; set; }

        public List<Entity> Entities { get; set; }

        public Entity IdentityEntity { get; set; }
    }
}
