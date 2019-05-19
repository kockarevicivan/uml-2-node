using System.Collections.Generic;
using Uml2Node.Core.Model;

namespace Uml2Node.Core.Interfaces
{
    public interface IParserService
    {
        List<Entity> Process(string imagePath);
    }
}
