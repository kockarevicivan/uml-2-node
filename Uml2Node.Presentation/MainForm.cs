using Ionic.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uml2Node.Core;
using Uml2Node.Core.Interfaces;
using Uml2Node.Core.Model;
using Uml2Node.Generator.Extensions;
using Uml2Node.IoC.Factories;

namespace Uml2Node.Presentation
{
    public partial class MainForm : Form
    {
        private IParserService _parser;
        private IGeneratorService _generator;

        private Project _project;

        public MainForm()
        {
            InitializeComponent();

            _parser = ServiceFactory.CreateParserService();
            _generator = ServiceFactory.CreateGeneratorService();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    List<Entity> entities = _parser.Parse(openFileDialog.FileName);
                    Entity identityEntity = entities.First();

                    int numberOfUnknownEntities = 0;

                    foreach (var entity in entities)
                    {
                        if (Constants.PossibleIdentities.Contains(entity.Name.ToLower()))
                            identityEntity = entity;

                        if (string.IsNullOrEmpty(entity.Name))
                        {
                            entity.Name = "Entity" + (numberOfUnknownEntities + 1);
                            numberOfUnknownEntities++;
                        }
                    }

                    /// TODO Normalize entities (check all necessary fields, constraints etc).

                    _project = new Project()
                    {
                        Name = "New project",
                        IdentityEntity = identityEntity,
                        Entities = entities
                    };

                    this.projectNameTextbox.Text = _project.Name;
                    this.schemaTextbox.Text = JsonConvert.SerializeObject(_project, Formatting.Indented);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error ocurred");
            }
        }

        private void generateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_project == null)
            {
                MessageBox.Show("Project not selected.", "Error ocurred");

                return;
            }

            try
            {
                _project.CamelCaseName = _project.Name.ToCamelCase();
                _project.PascalCaseName = _project.CamelCaseName.ToPascalCase();
                _project.DashCaseName= _project.CamelCaseName.ToDashCase();

                foreach (var entity in _project.Entities)
                {
                    entity.CamelCaseName = entity.Name.ToCamelCase();
                    entity.PascalCaseName = entity.CamelCaseName.ToPascalCase();
                    entity.DashCaseName = entity.CamelCaseName.ToDashCase();

                    foreach (var field in entity.Fields)
                    {
                        field.CamelCaseName = field.Name.ToCamelCase();
                        field.PascalCaseName = field.CamelCaseName.ToPascalCase();
                        field.DashCaseName = field.CamelCaseName.ToDashCase();
                    }
                }

                _generator.Generate(_project);

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.DefaultExt = ".zip";
                saveFileDialog.Title = "Save zip file";
                saveFileDialog.FileName = _project.DashCaseName + ".zip";
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "")
                {
                    MessageBox.Show(saveFileDialog.FileName);

                    using (var zip = new ZipFile())
                    {
                        zip.AddDirectory(Constants.Locations.TemporaryRoot);
                        zip.Save(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not generate project.", "Error ocurred");
            }
        }

        private void rebuildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.projectNameTextbox.Text))
            {
                MessageBox.Show("Name of the project cannot be empty.");
                return;
            }

            try
            {
                _project = JsonConvert.DeserializeObject<Project>(this.schemaTextbox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Schema is invalid.");
            }
        }

        private void saveSchemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string schemaText = this.schemaTextbox.Text;

            if (string.IsNullOrEmpty(schemaText))
            {
                MessageBox.Show("Project not selected.", "Error ocurred");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".json";
            saveFileDialog.Title = "Save schema";
            saveFileDialog.FileName = "schema.json";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();

                byte[] info = new UTF8Encoding(true).GetBytes(schemaText);

                fs.Write(info, 0, info.Length);

                fs.Close();
            }
        }
    }
}
