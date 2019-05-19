using System;
using System.Windows.Forms;
using Uml2Node.Core.Interfaces;
using Uml2Node.Core.Model;
using Uml2Node.IoC.Factories;

namespace Uml2Node.Presentation
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    IParserService parser = ServiceFactory.CreateParserService();
                    IGeneratorService generator = ServiceFactory.CreateGeneratorService();

                    generator.Generate(new Project()
                    {
                        CamelCaseName = "testapp",///TODO
                        IdentityEntityCamelCaseName = "User",///TODO
                        Entities = parser.Process(openFileDialog.FileName)
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    }
}
