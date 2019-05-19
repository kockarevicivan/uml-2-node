using System.Collections.Generic;

namespace Uml2Node.Core.Model
{
    public class Project
    {
        public string CamelCaseName { get; set; }
        public string PascalCaseName { get; set; }
        public string DashCaseName { get; set; }
        public string IdentityEntityCamelCaseName { get; set; }
        public string IdentityEntityPascalCaseName { get; set; }
        public string IdentityEntityDashCaseName { get; set; }

        public List<Entity> Entities { get; set; }
    }
}
