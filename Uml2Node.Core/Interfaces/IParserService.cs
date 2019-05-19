using System.Collections.Generic;
using Uml2Node.Core.Model;

namespace Uml2Node.Core.Interfaces
{
    public interface IParserService
    {   
        /// <summary>
        /// Parses the provided diagram and returns list of entity objects.
        /// </summary>
        /// <param name="imagePath">Image location of the diagram.</param>
        /// <returns>List of entities on the diagram.</returns>
        List<Entity> Parse(string imagePath);
    }
}
