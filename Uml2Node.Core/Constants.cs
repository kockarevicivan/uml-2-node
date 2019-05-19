using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Uml2Node.Core
{
    public static class Constants
    {
        public static readonly IList<String> PossibleIdentities = new ReadOnlyCollection<string> (new List<String> {
            "user",
            "customer",
            "employee",
            "administrator",
        });

        public static class Jokers
        {
            public const string ProjectNameDashCase = "{{ProjectNameDashCase}}";
            public const string RouteImports = "{{RouteImports}}";
            public const string RouteUsage = "{{RouteUsage}}";
            public const string IdentityEntityNamePascalCase = "{{IdentityEntityNamePascalCase}}";
            public const string EntityNamePascalCase = "{{EntityNamePascalCase}}";
            public const string FieldsAsJSON = "{{FieldsAsJSON}}";
        }

        public static class Locations
        {
            public const string TemplateRoot = "../../../templates/";
            public const string TemporaryRoot = "../../../temp/";

            public const string Src = "src/";
            public const string Controllers = Src + "controllers/";
            public const string Middleware = Src + "middleware/";
            public const string Models = Src + "models/";
            public const string Routes = Src + "routes/";
            public const string Services = Src + "services/";
            public const string Utils = Src + "utils/";

            public const string Gitignore = ".gitignore";
            public const string NodemonJson = "nodemon.json";
            public const string PackageJson = "package.json";
            public const string Readme = "README.md";
            public const string TsconfigJson = "tsconfig.json";
            public const string TslintJson = "tslint.json";
            
            public const string ConfigTs = Src + "config.ts";
            public const string ServerTs = Src + "server.ts";
            public const string Transform = Utils + "transform.ts";
            public const string AuthenticationService = Services + "AuthenticationService.ts";
            public const string AuthenticationRoute = Routes + "authentication.ts";
            public const string AuthenticationMiddleware = Middleware + "authenticationMiddleware.ts";
            public const string AuthenticationController = Controllers + "AuthenticationController.ts";

            public const string ServiceTemplate = Services + "Service.ts";
            public const string RouteTemplate = Routes + "route.ts";
            public const string ControllerTemplate = Models + "Model.ts";
            public const string ModelTemplate = Controllers + "Controller.ts";
        }

        public static class Templates
        {
            public const string Import = "import {0}Routes from './{1}/{2}';\n";
            public const string RouteUsage = "app.use('/{0}', {1}Routes);\n";
            public const string JSONField = "\t{0}: {1},\n";
        }
    }
}
