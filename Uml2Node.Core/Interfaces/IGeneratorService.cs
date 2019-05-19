using Uml2Node.Core.Model;

namespace Uml2Node.Core.Interfaces
{
    public interface IGeneratorService
    {
        /// <summary>
        /// Generates the project files from the provided project object.
        /// </summary>
        /// <param name="project">Object of the wanted project.</param>
        void Generate(Project project);
    }
}
