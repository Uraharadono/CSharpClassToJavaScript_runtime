using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Utility
{
    public class JsGeneratorOptions
    {
        // TODO: Please see if these default values can be removed, they are anoying as hell.
        public bool CamelCase { get; set; } = false;
        public string OutputNamespace { get; set; } = "models";
        public bool IncludeMergeFunction { get; set; } = false;
        public bool IncludeEqualsFunction { get; set; } = false;
        public List<string> ClassNameConstantsToRemove { get; set; } = new List<string>();
        public bool RespectDataMemberAttribute { get; set; } = false;
        public bool RespectDefaultValueAttribute { get; set; } = false;
        public List<Action<StringBuilder, IEnumerable<PropertyBag>, JsGeneratorOptions>> CustomFunctionProcessors { get; set; }
        public bool TreatEnumsAsStrings { get; set; } = false;
        public EGenerateOptions ConversionType { get; set; }
        public bool IncludeHeaders { get; set; } = false;
        public bool IncludeUnmapFunctions { get; set; } = false;
        public bool IncludeIsLoadingVar { get; set; } = false;
    }

    public enum EGenerateOptions
    {
        [Description("Javascript")]
        Javascript = 0,
        [Description("Ecma6 Javascript")]
        Ecma6 = 1,
        [Description("Ecma6 with Knockout")]
        KnockoutEcma6 = 2
    }
}
