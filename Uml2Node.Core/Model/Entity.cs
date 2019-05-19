using System.Collections.Generic;

namespace Uml2Node.Core.Model
{
    public class Entity
    {
        public string Name { get; set; }
        public string CamelCaseName { get; set; }
        public string DashCaseName { get; set; }
        public string PascalCaseName { get; set; }

        public List<Field> Fields { get; set; }
    }
}
