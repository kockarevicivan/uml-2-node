using Uml2Node.Core.Interfaces;
using Uml2Node.Generator.Services;
using Uml2Node.Parser.Services;

namespace Uml2Node.IoC.Factories
{
    public static class ServiceFactory
    {
        public static IParserService CreateParserService()
        {
            return new ParserService();
        }

        public static IGeneratorService CreateGeneratorService()
        {
            return new GeneratorService();
        }
    }
}
