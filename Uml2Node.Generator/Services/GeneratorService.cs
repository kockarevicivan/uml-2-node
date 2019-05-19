using System.Collections.Generic;
using System.IO;
using System.Text;
using Uml2Node.Core.Interfaces;
using Uml2Node.Core.Model;
using Uml2Node.Generator.Extensions;

namespace Uml2Node.Generator.Services
{
    public class GeneratorService : IGeneratorService
    {
        private static string _templateRoot = "../../../templates/";
        private static string _tempRoot = "../../../temp/";


        public void Generate(Project project)
        {
            this.GenerateFolders();
            this.GenerateRootFolder(project);
            this.GenerateSrcFolder(project);
        }

        private void GenerateFolders()
        {
            Directory.CreateDirectory(_tempRoot);
            Directory.CreateDirectory(_tempRoot + "src/");
            Directory.CreateDirectory(_tempRoot + "src/controllers");
            Directory.CreateDirectory(_tempRoot + "src/middleware");
            Directory.CreateDirectory(_tempRoot + "src/models");
            Directory.CreateDirectory(_tempRoot + "src/routes");
            Directory.CreateDirectory(_tempRoot + "src/services");
            Directory.CreateDirectory(_tempRoot + "src/utils");
        }

        private void GenerateRootFolder(Project project)
        {
            string gitignoreContents = ReadFile(".gitignore");
            WriteFile(".gitignore", gitignoreContents);

            string nodemonContents = ReadFile("nodemon.json");
            WriteFile("nodemon.json", nodemonContents);

            string packageJsonContents = ReadFile("package.json").Replace("{{ProjectNameDashCase}}", project.DashCaseName);
            WriteFile("package.json", packageJsonContents);

            string readmeContents = ReadFile("README.md").Replace("{{ProjectNameDashCase}}", project.DashCaseName);
            WriteFile("README.md", readmeContents);

            string tsconfigContents = ReadFile("tsconfig.json");
            WriteFile("tsconfig.json", tsconfigContents);

            string tslintContents = ReadFile("tslint.json");
            WriteFile("tslint.json", tslintContents);
        }

        private void GenerateSrcFolder(Project project)
        {
            string configContents = ReadFile("src/config.ts").Replace("{{ProjectNameDashCase}}", project.DashCaseName);
            WriteFile("src/config.ts", configContents);

            string serverContents = ReadFile("src/server.ts")
                .Replace("{{RouteImports}}", this.MapImports(project, "routes"))
                .Replace("{{RouteUsage}}", this.MapRouteUsage(project));
            WriteFile("src/server.ts", serverContents);

            string transformContents = ReadFile("src/utils/transform.ts");
            WriteFile("src/utils/transform.ts", transformContents);

            string authenticationServiceContents = ReadFile("src/services/AuthenticationService.ts")
                .Replace("{{IdentityEntityNamePascalCase}}", project.IdentityEntityPascalCaseName);
            WriteFile("src/services/AuthenticationService.ts", authenticationServiceContents);

            this.GenerateServices(project);

            string authenticationRouteContents = ReadFile("src/routes/authentication.ts");
            WriteFile("src/routes/authentication.ts", authenticationRouteContents);

            this.GenerateRoutes(project);

            this.GenerateModels(project);

            string authenticationMiddlewareContents = ReadFile("src/middleware/authenticationMiddleware.ts");
            WriteFile("src/middleware/authenticationMiddleware.ts", authenticationMiddlewareContents);

            string authenticationControllerContents = ReadFile("src/controller/AuthenticationController.ts");
            WriteFile("src/controller/AuthenticationController.ts", authenticationControllerContents);

            this.GenerateControllers(project);
        }


        private void GenerateServices(Project project)
        {
            string serviceTemplate = ReadFile("src/services/Service.ts");

            foreach (var entity in project.Entities)
            {
                string finalContents = serviceTemplate.Replace("{{EntityNamePascalCase}}", entity.PascalCaseName);
                WriteFile("src/services/" + entity.PascalCaseName + ".ts", finalContents);
            }
        }

        private void GenerateRoutes(Project project)
        {
            string routeTemplate = ReadFile("src/routes/route.ts");
            List<string> result = new List<string>();

            foreach (var entity in project.Entities)
            {
                string finalContents = routeTemplate
                    .Replace("{{EntityNamePascalCase}}", entity.PascalCaseName);

                WriteFile("src/routes/" + entity.CamelCaseName + ".ts", finalContents);
            }
        }

        private void GenerateModels(Project project)
        {
            string modelTemplate = ReadFile("src/models/Model.ts");
            List<string> result = new List<string>();

            foreach (var entity in project.Entities)
            {
                string finalContents = modelTemplate
                    .Replace("{{EntityNamePascalCase}}", entity.PascalCaseName)
                    .Replace("{{FieldsAsJSON}}", this.MapFieldsAsJson(entity));

                WriteFile("src/model/" + entity.PascalCaseName + ".ts", finalContents);
            }
        }

        private void GenerateControllers(Project project)
        {
            string controllerTemplate = ReadFile("src/controllers/Controller.ts");
            List<string> result = new List<string>();

            foreach (var entity in project.Entities)
            {
                string finalContents = controllerTemplate
                    .Replace("{{EntityNamePascalCase}}", entity.PascalCaseName)
                    .Replace("{{FieldsAsJSON}}", this.MapFieldsAsJson(entity));

                WriteFile("src/controllers/" + entity.PascalCaseName + ".ts", finalContents);
            }
        }


        private string MapFieldsAsJson(Entity entity)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var field in entity.Fields)
                stringBuilder.Append("\t" + field.Name + ": " + field.Type + ",\n");

            return stringBuilder.ToString();
        }

        private string MapImports(Project project, string folder)
        {
            string capitalNameFolder = folder.ToPascalCase();

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var entity in project.Entities)
                stringBuilder.Append("import " + entity.CamelCaseName + "Routes from './" + folder + "/" + entity.CamelCaseName + "';\n");

            return stringBuilder.ToString();
        }

        private string MapRouteUsage(Project project)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var entity in project.Entities)
                stringBuilder.Append("app.use('/" + entity.CamelCaseName + "', " + entity.CamelCaseName + "Routes);\n");

            return stringBuilder.ToString();
        }


        private static void WriteFile(string filePath, string contents)
        {
            File.WriteAllText(_tempRoot + filePath, contents);
        }

        private static string ReadFile(string filePath)
        {
            return File.ReadAllText(_templateRoot + filePath);
        }
    }
}
