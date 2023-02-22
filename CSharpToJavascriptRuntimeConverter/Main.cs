using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Utility;
using Utility.Extensions;
using Utility.FileUtil;
using Utility.Generators;
using Utility.Models;
using Utility.ReflectionUtil;

namespace CSharpToJavascriptRuntimeConverter
{
    public partial class Main : Form
    {
        private static string filePath;

        public Main()
        {
            InitializeComponent();
            List<SelectViewModel> listOfGenerateOptions = EnumExtensions.Enumerate<EGenerateOptions>()
                .Select(t => new SelectViewModel(t.ToInt(), t.GetDisplayNameOrDescription()))
                .ToList();

            generateTypesDropdown.DataSource = listOfGenerateOptions;
            generateTypesDropdown.DropDownStyle = ComboBoxStyle.DropDownList;

            // ReSharper disable once LocalizableElement
            namespaceInput.Text = "class";
        }

        private void SelectFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectFileDialog = new OpenFileDialog();
            selectFileDialog.Multiselect = true;
            selectFileDialog.ShowDialog();
            // ReSharper disable once LocalizableElement
            selectFileDialog.Filter = "allfiles|*.cs";
            filePath = selectFileDialog.FileName;
            fileNameLabel.Text = selectFileDialog.SafeFileName;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                ShowFileMissingErrorMessage();
                return;
            }

            try
            {
                // String which contains whole file contents
                string fileContent = FileContentUtil.GetFileContent(filePath);

                // Get option values for generator options
                JsGeneratorOptions options = GetOptions();

                // Before we compile stuff we need to delete pre-processor commands like "#nullable disable"
                fileContent = fileContent.RemoveOccurrences("#");

                // Create compiler and execute content of file
                Assembly asm = BuildAssemblyUtil.CompileCode(fileContent);

                // Get all types from assembly using reflection
                List<Type> rawTypes = BuildAssemblyUtil.GetExportedTypes(asm);

                // Filter types, and take only parent ones (the ones that are not referenced in parent, and standalone one)
                // If referenced children will be generated automatically, so no need to duplicate stuff.
                List<Type> types = BuildAssemblyUtil.FilterExportedTypes(rawTypes);

                // Finally generate
                StringBuilder sbFinal = new StringBuilder();
                foreach (Type type in types)
                {
                    switch (options.ConversionType)
                    {
                        case EGenerateOptions.Javascript:
                            sbFinal.AppendLine(JsGenerator.Generate(new[] { type }, options));
                            break;
                        case EGenerateOptions.Ecma6:
                            sbFinal.AppendLine(Ecma6Generator.Generate(new[] { type }, options));
                            break;
                        case EGenerateOptions.KnockoutEcma6:
                            sbFinal.AppendLine(Ecma6KnockoutGenerator.Generate(new[] { type }, options));
                            break;
                        default:
                            throw new Exception(
                                "Somehow Conversion type is not set or is out of bounds. I have no idea what happened.");
                    }
                }

                codeTextEditor.Text = sbFinal.ToString();
            }
            catch (Exception exception)
            {
                // Since we presumably should know what we are doing here, we are just showing message from exception
                // Would be good idea to log this somewhere tho.
                ShowErrorMessage(exception.Message);
            }
        }

        private JsGeneratorOptions GetOptions()
        {
            return new JsGeneratorOptions
            {
                CamelCase = camelCaseCheckBox.Checked,
                OutputNamespace = namespaceInput.Text,
                IncludeMergeFunction = includeMergeFunctionCheckBox.Checked,
                IncludeEqualsFunction = includeEqualsFunctionCheckBox.Checked,
                ClassNameConstantsToRemove = new List<string>() { "Dto" },
                RespectDataMemberAttribute = respectDataMemberAttributeCheckBox.Checked,
                RespectDefaultValueAttribute = respectDefaultValueAttributeCheckBox.Checked,
                TreatEnumsAsStrings = treatEnumsAsStringsCheckBox.Checked,

                ConversionType = (EGenerateOptions)((SelectViewModel)generateTypesDropdown.SelectedItem).Value,
                IncludeHeaders = includeHeadersCheckBox.Checked,
                IncludeUnmapFunctions = unmapFunctionCheckBox.Checked,
                IncludeIsLoadingVar = isLoadingCheckBox.Checked

                //  CustomFunctionProcessors =
                //new List<Action<StringBuilder, IEnumerable<PropertyBag>, JsGeneratorOptions>>()
                //{
                //    (builder, bags, arg3) =>
                //    {
                //        builder.AppendLine("\tthis.helloWorld = function () {");
                //        builder.AppendLine("\t\tconsole.log('hello');");
                //        builder.AppendLine("\t}");
                //    }
                //}
            };
        }

        private void ShowFileMissingErrorMessage()
        {
            ShowErrorMessage("You have to select file.");
        }

        private void ShowErrorMessage(string message)
        {
            // string message = "You have to select file.";
            string caption = "Error !";
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            // Displays the MessageBox.
            MessageBox.Show(message, caption, buttons);
        }
    }
}
