using System.Collections.Generic;
using System.IO;
using System.Text;
using Uml2Node.Core;
using Uml2Node.Core.Interfaces;
using Uml2Node.Core.Model;
using Uml2Node.Generator.Extensions;

namespace Uml2Node.Generator.Services
{
    public class GeneratorService : IGeneratorService
    {
        /// <inheritdoc />
        public void Generate(Project project)
        {
            this.GenerateFolders();
            this.GenerateRootFolder(project);
            this.GenerateSrcFolder(project);
        }

        /// <summary>
        /// Generates necessary folders.
        /// </summary>
        private void GenerateFolders()
        {
            Directory.CreateDirectory(Constants.Locations.TemporaryRoot);
            Directory.CreateDirectory(Constants.Locations.TemporaryRoot + Constants.Locations.Src);
            Directory.CreateDirectory(Constants.Locations.TemporaryRoot + Constants.Locations.Controllers);
            Directory.CreateDirectory(Constants.Locations.TemporaryRoot + Constants.Locations.Middleware);
            Directory.CreateDirectory(Constants.Locations.TemporaryRoot + Constants.Locations.Models);
            Directory.CreateDirectory(Constants.Locations.TemporaryRoot + Constants.Locations.Routes);
            Directory.CreateDirectory(Constants.Locations.TemporaryRoot + Constants.Locations.Services);
            Directory.CreateDirectory(Constants.Locations.TemporaryRoot + Constants.Locations.Utils);
        }

        /// <summary>
        /// Generates root folder of the project.
        /// </summary>
        /// <param name="project">Project that needs to be created.</param>
        private void GenerateRootFolder(Project project)
        {
            string gitignoreContents = ReadFile(Constants.Locations.Gitignore);
            WriteFile(Constants.Locations.Gitignore, gitignoreContents);

            string nodemonContents = ReadFile(Constants.Locations.NodemonJson);
            WriteFile(Constants.Locations.NodemonJson, nodemonContents);

            string packageJsonContents = ReadFile(Constants.Locations.PackageJson)
                .Replace(Constants.Jokers.ProjectNameDashCase, project.DashCaseName);
            WriteFile(Constants.Locations.PackageJson, packageJsonContents);

            string readmeContents = ReadFile(Constants.Locations.Readme)
                .Replace(Constants.Jokers.ProjectNameDashCase, project.DashCaseName);
            WriteFile(Constants.Locations.Readme, readmeContents);

            string tsconfigContents = ReadFile(Constants.Locations.TsconfigJson);
            WriteFile(Constants.Locations.TsconfigJson, tsconfigContents);

            string tslintContents = ReadFile(Constants.Locations.TslintJson);
            WriteFile(Constants.Locations.TslintJson, tslintContents);
        }

        /// <summary>
        /// Generates src folder of the project.
        /// </summary>
        /// <param name="project">Project that needs to be created.</param>
        private void GenerateSrcFolder(Project project)
        {
            string configContents = ReadFile(Constants.Locations.ConfigTs)
                .Replace(Constants.Jokers.ProjectNameDashCase, project.DashCaseName);
            WriteFile(Constants.Locations.ConfigTs, configContents);

            string serverContents = ReadFile(Constants.Locations.ServerTs)
                .Replace(Constants.Jokers.RouteImports, this.MapImports(project, "routes"))
                .Replace(Constants.Jokers.RouteUsage, this.MapRouteUsage(project));

            WriteFile(Constants.Locations.ServerTs, serverContents);

            string transformContents = ReadFile(Constants.Locations.Transform);
            WriteFile(Constants.Locations.Transform, transformContents);

            string authenticationServiceContents = ReadFile(Constants.Locations.AuthenticationService)
                .Replace(Constants.Jokers.IdentityEntityNamePascalCase, project.IdentityEntity.PascalCaseName);
            WriteFile(Constants.Locations.AuthenticationService, authenticationServiceContents);

            this.GenerateServices(project);

            string authenticationRouteContents = ReadFile(Constants.Locations.AuthenticationRoute);
            WriteFile(Constants.Locations.AuthenticationRoute, authenticationRouteContents);

            this.GenerateRoutes(project);

            this.GenerateModels(project);

            string authenticationMiddlewareContents = ReadFile(Constants.Locations.AuthenticationMiddleware);
            WriteFile(Constants.Locations.AuthenticationMiddleware, authenticationMiddlewareContents);

            string authenticationControllerContents = ReadFile(Constants.Locations.AuthenticationController);
            WriteFile(Constants.Locations.AuthenticationController, authenticationControllerContents);

            this.GenerateControllers(project);
        }

        /// <summary>
        /// Generates necessary services.
        /// </summary>
        /// <param name="project">Project that needs to be created.</param>
        private void GenerateServices(Project project)
        {
            string serviceTemplate = ReadFile(Constants.Locations.ServiceTemplate);

            foreach (var entity in project.Entities)
            {
                string finalContents = serviceTemplate
                    .Replace(Constants.Jokers.EntityNamePascalCase, entity.PascalCaseName);
                WriteFile(Constants.Locations.Services + entity.PascalCaseName + ".ts", finalContents);
            }
        }

        /// <summary>
        /// Generates necessary routes.
        /// </summary>
        /// <param name="project">Project that needs to be created.</param>
        private void GenerateRoutes(Project project)
        {
            string routeTemplate = ReadFile(Constants.Locations.RouteTemplate);
            List<string> result = new List<string>();

            foreach (var entity in project.Entities)
            {
                string finalContents = routeTemplate
                    .Replace(Constants.Jokers.EntityNamePascalCase, entity.PascalCaseName);

                WriteFile(Constants.Locations.Routes + entity.CamelCaseName + ".ts", finalContents);
            }
        }

        /// <summary>
        /// Generates necessary models.
        /// </summary>
        /// <param name="project">Project that needs to be created.</param>
        private void GenerateModels(Project project)
        {
            string modelTemplate = ReadFile(Constants.Locations.ModelTemplate);
            List<string> result = new List<string>();

            foreach (var entity in project.Entities)
            {
                string finalContents = modelTemplate
                    .Replace(Constants.Jokers.EntityNamePascalCase, entity.PascalCaseName)
                    .Replace(Constants.Jokers.FieldsAsJSON, this.MapFieldsAsJson(entity));

                WriteFile(Constants.Locations.Models + entity.PascalCaseName + ".ts", finalContents);
            }
        }

        /// <summary>
        /// Generates necessary controllers.
        /// </summary>
        /// <param name="project">Project that needs to be created.</param>
        private void GenerateControllers(Project project)
        {
            string controllerTemplate = ReadFile(Constants.Locations.ControllerTemplate);
            List<string> result = new List<string>();

            foreach (var entity in project.Entities)
            {
                string finalContents = controllerTemplate
                    .Replace(Constants.Jokers.EntityNamePascalCase, entity.PascalCaseName)
                    .Replace(Constants.Jokers.FieldsAsJSON, this.MapFieldsAsJson(entity));

                WriteFile(Constants.Locations.Controllers + entity.PascalCaseName + ".ts", finalContents);
            }
        }

        /// <summary>
        /// Generates JSON content for the fields of the entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Fields of entity object as JSON string.</returns>
        private string MapFieldsAsJson(Entity entity)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var field in entity.Fields)
                stringBuilder.Append(string.Format(Constants.Templates.JSONField, field.Name, field.Type));

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Generates import statements for the entities.
        /// </summary>
        /// <param name="project">Project that contains the entities.</param>
        /// <param name="folder">Folder for which the imports should be created (routes, models etc).</param>
        /// <returns>String content of the wanted imports.</returns>
        private string MapImports(Project project, string folder)
        {
            string capitalNameFolder = folder.ToPascalCase();

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var entity in project.Entities)
                stringBuilder.Append(string.Format(Constants.Templates.Import, entity.CamelCaseName, folder, entity.CamelCaseName));

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Generates app.use statements for each of the entity routes.
        /// </summary>
        /// <param name="project">Project that contains the entities.</param>
        /// <returns>String content of the wanted app.use statements.</returns>
        private string MapRouteUsage(Project project)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var entity in project.Entities)
                stringBuilder.Append(string.Format(Constants.Templates.RouteUsage, entity.CamelCaseName, entity.CamelCaseName));

            return stringBuilder.ToString();
        }


        /// <summary>
        /// Writes the provided contents to the the provided template location.
        /// </summary>
        /// <param name="filePath">Location of the file inside the project.</param>
        /// <param name="contents">Contents that need to be written.</param>
        private static void WriteFile(string filePath, string contents)
        {
            File.WriteAllText(Constants.Locations.TemporaryRoot + filePath, contents);
        }

        /// <summary>
        /// Reads the contensof the provided file inside the template directory.
        /// </summary>
        /// <param name="filePath">Location of the file.</param>
        /// <returns>Contents of the file as string.</returns>
        private static string ReadFile(string filePath)
        {
            return File.ReadAllText(Constants.Locations.TemplateRoot + filePath);
        }
    }
}
