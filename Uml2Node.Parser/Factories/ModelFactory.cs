﻿using System.Collections.Generic;
using System.Linq;
using Uml2Node.Core.Model;

namespace Uml2Node.Parser.Factories
{
    internal static class ModelFactory
    {
        /// <summary>
        /// Creates an instance of the entity object.
        /// </summary>
        /// <param name="entityString">JSON string of the entity.</param>
        /// <returns>Entity object.</returns>
        internal static Entity CreateEntity(string entityString)
        {
            Entity newEntity = new Entity();

            List<string> lines = entityString.Split('\n').Where(l => !string.IsNullOrEmpty(l.Trim())).ToList();

            newEntity.Name = lines.ElementAt(0).Trim();
            newEntity.CamelCaseName = newEntity.Name.ToLower();
            lines.Remove(lines.First());

            newEntity.Fields = new List<Field>();

            foreach (var line in lines)
                newEntity.Fields.Add(CreateField(line.Trim()));

            return newEntity;
        }

        /// <summary>
        /// Creates an instance of the field object.
        /// </summary>
        /// <param name="fieldString">JSON string of the field.</param>
        /// <returns>Field object.</returns>
        private static Field CreateField(string fieldString)
        {
            Field newField = new Field();

            string[] tokens = fieldString.Split(':');

            if (tokens.Length == 1)
            {
                newField.Type = "string";
                newField.Name = tokens[0].Trim();
            }
            else if (tokens.Length > 1)
            {
                newField.Type = tokens[0].Trim();
                newField.Name = tokens[1].Trim();
            }

            return newField;
        }
    }
}
