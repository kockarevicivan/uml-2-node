using Uml2Node.Core.Interfaces;
using Uml2Node.Generator.Services;
using Uml2Node.Parser.Services;

namespace Uml2Node.IoC.Factories
{
    public static class ServiceFactory
    {   
        /// <summary>
        /// Creates an instance of the parser service.
        /// </summary>
        public static IParserService CreateParserService()
        {
            return new ParserService();
        }

        /// <summary>
        /// Creates an instance of the generator service.
        /// </summary>
        public static IGeneratorService CreateGeneratorService()
        {
            return new GeneratorService();
        }
    }
}
