﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Generators
{
    public static class Ecma6KnockoutGenerator
    {
        public static JsGeneratorOptions Options { get; set; } = new JsGeneratorOptions();

        private static readonly List<Type> AllowedDictionaryKeyTypes = new List<Type>()
        {
            typeof(int),
            typeof(string),
            typeof(Enum)
        };

        public static string Generate(IEnumerable<Type> typesToGenerate, JsGeneratorOptions generatorOptions = null)
        {
            var passedOptions = generatorOptions ?? Options;
            if (passedOptions == null)
            {
                throw new ArgumentNullException(nameof(passedOptions), "Options cannot be null.");
            }
            var propertyClassCollection = TypePropertyDictionaryGenerator.GetPropertyDictionaryForTypeGeneration(typesToGenerate, passedOptions);
            var js = GenerateJs(propertyClassCollection, passedOptions);
            return js;
        }

        static string GenerateJs(IEnumerable<PropertyBag> propertyCollection, JsGeneratorOptions generationOptions)
        {
            var options = generationOptions;
            var sbOut = new StringBuilder();

            foreach (var type in propertyCollection.GroupBy(r => r.TypeName))
            {
                var typeDefinition = type.First().TypeDefinition;

                var sb = new StringBuilder();
                if (options.IncludeHeaders) BuildClassHeaders(sb);

                BuildClassConstructor(type, sb, options);

                var propList = type.GroupBy(t => t.PropertyName).Select(t => t.First()).ToList();
                foreach (var propEntry in propList)
                {
                    switch (propEntry.TransformablePropertyType)
                    {
                        case PropertyBag.TransformablePropertyTypeEnum.CollectionType:
                            BuildArrayProperty(sb, propEntry, options);
                            break;
                        case PropertyBag.TransformablePropertyTypeEnum.DictionaryType:
                            BuildDictionaryProperty(sb, propEntry, options);
                            break;
                        case PropertyBag.TransformablePropertyTypeEnum.ReferenceType:
                            BuildObjectProperty(sb, propEntry, options);
                            break;
                        case PropertyBag.TransformablePropertyTypeEnum.Primitive:
                            BuildPrimitiveProperty(propEntry, sb, options);
                            break;
                    }
                }

                if (options.IncludeIsLoadingVar) BuildLoadingVar(sb);

                sb.AppendLine("\t}"); // first close constructor parenthesis

                if (options.IncludeUnmapFunctions) BuildUnmapFunction(sb, propList, generationOptions);

                if (options.IncludeMergeFunction && !typeDefinition.IsEnum)
                {
                    sb.AppendLine();
                    BuildMergeFunctionForClass(sb, propList, options);
                }

                if (options.IncludeEqualsFunction && !typeDefinition.IsEnum)
                {
                    sb.AppendLine();
                    BuildEqualsFunctionForClass(sb, propList, options);
                }

                if (options.CustomFunctionProcessors?.Any() == true)
                {
                    foreach (var customProcessor in options.CustomFunctionProcessors)
                    {
                        sb.AppendLine();
                        customProcessor(sb, propList, options);
                    }
                }

                BuildClassClosure(sb);

                sbOut.AppendLine(sb.ToString());
                sbOut.AppendLine();
            }

            return sbOut.ToString();
        }

        // ======================= Special Utility Functions =======================
        private static void BuildClassHeaders(StringBuilder sb)
        {
            sb.AppendLine("import $ from 'jquery';");
            sb.AppendLine("import ko from 'knockout';");
            sb.AppendLine("import ajax from '../../services/ajax-service';");
            sb.AppendLine("import validation from '../../common/validator';");
            sb.AppendLine("import toastr from '../../common/toastr';");
            sb.AppendLine("import { isNullOrWs, formatDateTime } from '../../common/util';");
            sb.AppendLine("");
        }

        private static void BuildLoadingVar(StringBuilder sb)
        {
            sb.AppendLine("\t\tthis.isLoading = ko.observable(false); ");
        }

        private static void BuildUnmapFunction(StringBuilder sb, List<PropertyBag> properties, JsGeneratorOptions options)
        {
            sb.AppendLine($"\tunmap() {{");
            sb.AppendLine($"\t\t return {{");
            foreach (var fileProperty in properties)
            {
                // Note: I don't see point in creating unmap for anything beside primitive types, so I will leave others out
                if (fileProperty.TransformablePropertyType == PropertyBag.TransformablePropertyTypeEnum.Primitive)
                {
                    sb.AppendLine($"\t\t\t {Helpers.ToCamelCase(fileProperty.PropertyName, options.CamelCase)}: this.{Helpers.ToCamelCase(fileProperty.PropertyName, options.CamelCase)}()");
                }
            }
            sb.AppendLine($"\t\t}}");
        }

        // ======================= Specific Build Functions =======================

        private static void BuildClassClosure(StringBuilder sb)
        {
            sb.AppendLine("}");
        }

        private static void BuildClassConstructor(IGrouping<string, PropertyBag> type, StringBuilder sb, JsGeneratorOptions options)
        {
            if (
                type.Any(
                    p =>
                        (p.CollectionInnerTypes != null && p.CollectionInnerTypes.Any(q => !q.IsPrimitiveType)) ||
                        p.TransformablePropertyType == PropertyBag.TransformablePropertyTypeEnum.ReferenceType))
            {
                sb.AppendLine(
                    $"export default {options.OutputNamespace} {Helpers.GetName(type.First().TypeName, options.ClassNameConstantsToRemove)} {{");
                sb.AppendLine($"\tconstructor (data) {{");
            }
            else if (type.First().TypeDefinition.IsEnum)
            {
                // WE have 2 options: 
                #region Option 1
                //export default class Enum
                //{
                //    static enumerate()
                //    {
                //        return Object.values(this).map((value) => ({
                //            id: value,
                //        name: this.display(value)
                //        }));
                //    }
                //}

                //import Enum from 'common/enum';
                //export class EStoreFilters extends Enum
                //    {
                //    static all = 0
                //    static withoutProduct = 1
                //    static display(value)
                //    {
                //        if (value === EStoreFilters.all)
                //            return 'Show all';
                //        if (value === EStoreFilters.withoutProduct)
                //            return 'Without this product';
                //        throw new Error(`EStoreFilters Enum does not contain value '${value}'`);
                //    }
                //}
                #endregion
                #region Option 2
                //export const EImageType = {
                //    front: 0,
                //    back: 1,
                //    detailOne: 2,
                //    detailTwo: 3,
                //    detailThree: 4
                //};
                #endregion

                // Option 2
                sb.AppendLine(
                    $"export const {options.OutputNamespace} {Helpers.GetName(type.First().TypeName, options.ClassNameConstantsToRemove)} = {{");
            }
            else
            {
                sb.AppendLine(
                    $"export default {options.OutputNamespace} {Helpers.GetName(type.First().TypeName, options.ClassNameConstantsToRemove)} {{");

                sb.AppendLine($"\tconstructor (data) {{");
            }
        }

        // TODO: Remove this function. Reason: I don't need copy constructor and can't find a scenario where I will use it.
        private static void BuildEqualsFunctionForClass(StringBuilder sb, IEnumerable<PropertyBag> propList,
            JsGeneratorOptions options)
        {
            // Generate an equals function for two objects
            sb.AppendLine("\tthis.$equals = function (compareObj) {");
            sb.AppendLine("\t\tif (!compareObj) { return false; }");
            foreach (var propEntry in propList)
            {
                switch (propEntry.TransformablePropertyType)
                {
                    case PropertyBag.TransformablePropertyTypeEnum.CollectionType:
                        sb.AppendLine(
                            $"\t\tif (compareObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} !== this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
                        sb.AppendLine($"\t\t\tif (!compareObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
                        sb.AppendLine($"\t\t\t\treturn false;");
                        sb.AppendLine($"\t\t\t}}");
                        sb.AppendLine($"\t\t\tif (!this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
                        sb.AppendLine($"\t\t\t\treturn false;");
                        sb.AppendLine($"\t\t\t}}");
                        sb.AppendLine($"\t\t\tif (compareObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}.length != this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}.length) {{");
                        sb.AppendLine($"\t\t\t\treturn false;");
                        sb.AppendLine($"\t\t\t}}");
                        sb.AppendLine(
                            $"\t\t\tfor (i = 0; i < this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}.length; i++) {{");
                        var collectionType = propEntry.CollectionInnerTypes.First();

                        if (!collectionType.IsPrimitiveType)
                        {
                            sb.AppendLine(
                                $"\t\t\t\tif (!this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[i].$equals(compareObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[i])) {{ return false; }};");

                        }
                        else
                        {
                            sb.AppendLine(
                                $"\t\t\t\tif (this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[i] !== compareObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[i]) {{ return false; }};");
                        }
                        sb.AppendLine($"\t\t\t}}");
                        sb.AppendLine($"\t\t}}");

                        break;
                    case PropertyBag.TransformablePropertyTypeEnum.DictionaryType:
                        sb.AppendLine(
                            $"\t\tif (compareObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} !== this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
                        sb.AppendLine($"\t\t\tif (!compareObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
                        sb.AppendLine($"\t\t\t\treturn false;");
                        sb.AppendLine($"\t\t\t}}");
                        sb.AppendLine($"\t\t\tif (!this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
                        sb.AppendLine($"\t\t\t\treturn false;");
                        sb.AppendLine($"\t\t\t}}");
                        /*sb.AppendLine(
                            $"\t\tif (this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} != null) {{");*/
                        sb.AppendLine(
                            $"\t\t\tfor (var key in this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
                        sb.AppendLine(
                            $"\t\t\t\tif (!compareObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}.hasOwnProperty(key)) {{");
                        sb.AppendLine(
                            $"\t\t\t\t\treturn false;");
                        sb.AppendLine("\t\t\t\t}");
                        var valueType = propEntry.CollectionInnerTypes.First(p => !p.IsDictionaryKey);

                        if (!valueType.IsPrimitiveType)
                        {
                            sb.AppendLine(
                                $"\t\t\t\tif (!this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[key].$equals(compareObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[key])) {{ return false; }};");
                        }
                        else
                        {
                            sb.AppendLine(
                                $"\t\t\t\tif (this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[key] !== compareObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[key]) {{ return false; }};");
                        }
                        sb.AppendLine("\t\t\t}");
                        sb.AppendLine("\t\t}");
                        break;
                    case PropertyBag.TransformablePropertyTypeEnum.ReferenceType:
                        sb.AppendLine(
                            $"\t\tif (compareObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} !== this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
                        sb.AppendLine($"\t\t\tif (!compareObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
                        sb.AppendLine($"\t\t\t\treturn false;");
                        sb.AppendLine($"\t\t\t}}");
                        sb.AppendLine($"\t\t\tif (!this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
                        sb.AppendLine($"\t\t\t\treturn false;");
                        sb.AppendLine($"\t\t\t}}");
                        sb.AppendLine(
                                $"\t\t\tif (!this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}.$equals(compareObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)})) {{ return false; }};");
                        sb.AppendLine("\t\t}");
                        break;
                    case PropertyBag.TransformablePropertyTypeEnum.Primitive:
                        sb.AppendLine(
                            $"\t\tif (this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} !== compareObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{ return false; }};");
                        break;
                }
            }
            sb.AppendLine("\treturn true;");
            sb.AppendLine("\t}");
        }

        // TODO: Remove this function. Reason: I really need to think about this method more, in terms of "BindValues" as I am used to.
        private static void BuildMergeFunctionForClass(StringBuilder sb, IEnumerable<PropertyBag> propList,
                    JsGeneratorOptions options)
        {
            //Generate a merge function to merge two objects
            sb.AppendLine("\tthis.$merge = function (mergeObj) {");
            sb.AppendLine("\t\tif (!mergeObj) { mergeObj = { }; }");
            foreach (var propEntry in propList)
            {
                switch (propEntry.TransformablePropertyType)
                {
                    case PropertyBag.TransformablePropertyTypeEnum.CollectionType:
                        sb.AppendLine(
                            $"\t\tif (!mergeObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
                        sb.AppendLine($"\t\t\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} = [];");
                        sb.AppendLine("\t\t}");
                        sb.AppendLine(
                            $"\t\tif (this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} != null) {{");
                        sb.AppendLine(string.Format("\t\t\tthis.{0}.splice(0, this.{0}.length);", Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)));
                        sb.AppendLine("\t\t}");
                        sb.AppendLine(
                            $"\t\tif (mergeObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
                        sb.AppendLine(
                            $"\t\t\tif (this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} === null) {{");
                        sb.AppendLine($"\t\t\t\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} = [];");
                        sb.AppendLine("\t\t\t}");
                        sb.AppendLine(
                            $"\t\t\tfor (i = 0; i < mergeObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}.length; i++) {{");
                        sb.AppendLine(string.Format("\t\t\t\tthis.{0}.push(mergeObj.{0}[i]);", Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)));
                        sb.AppendLine("\t\t\t}");
                        sb.AppendLine("\t\t}");
                        break;
                    case PropertyBag.TransformablePropertyTypeEnum.DictionaryType:
                        sb.AppendLine(
                            $"\t\tif (this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} != null) {{");
                        sb.AppendLine(
                            $"\t\t\tfor (var key in this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
                        sb.AppendLine(
                            $"\t\t\t\tif (this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}.hasOwnProperty(key)) {{");
                        sb.AppendLine(
                            $"\t\t\t\t\tdelete this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[key];");
                        sb.AppendLine("\t\t\t\t}");
                        sb.AppendLine("\t\t\t}");
                        sb.AppendLine("\t\t}");
                        sb.AppendLine(
                            $"\t\tif (mergeObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
                        sb.AppendLine(
                            $"\t\t\tfor (var key in mergeObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
                        sb.AppendLine(
                            $"\t\t\t\tif (mergeObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}.hasOwnProperty(key)) {{");
                        sb.AppendLine(
                            $"\t\t\t\t\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[key] = mergeObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[key];");
                        sb.AppendLine("\t\t\t\t}");
                        sb.AppendLine("\t\t\t}");
                        sb.AppendLine("\t\t}");
                        break;
                    case PropertyBag.TransformablePropertyTypeEnum.ReferenceType:
                        sb.AppendLine(
                            $"\t\tif (mergeObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} == null) {{");
                        sb.AppendLine($"\t\t\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} = null;");
                        sb.AppendLine(
                            $"\t\t}} else if (this.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} != null) {{");
                        sb.AppendLine(
                            $"\t\t\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}.$merge(mergeObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)});");
                        sb.AppendLine("\t\t} else {");
                        sb.AppendLine(
                            $"\t\t\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} = mergeObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)};");
                        sb.AppendLine("\t\t}");
                        break;
                    case PropertyBag.TransformablePropertyTypeEnum.Primitive:
                        sb.AppendLine(
                            $"\t\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} = mergeObj.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)};");
                        break;
                }
            }
            sb.AppendLine("\t}");
        }

        private static void BuildPrimitiveProperty(PropertyBag propEntry, StringBuilder sb, JsGeneratorOptions options)
        {
            if (propEntry.TypeDefinition.IsEnum)
            {
                sb.AppendLine(
                    propEntry.PropertyType == typeof(string)
                        ? $"\t{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}: '{propEntry.DefaultValue}',"
                        : $"\t{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}: {propEntry.DefaultValue},");
            }
            else if (propEntry.HasDefaultValue)
            {
                // Todo: Check what to do with this part
                var writtenValue = propEntry.DefaultValue is bool
                    ? propEntry.DefaultValue.ToString().ToLower()
                    : propEntry.DefaultValue;
                sb.AppendLine(
                    $"\tif (!data.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
                sb.AppendLine(
                    propEntry.PropertyType == typeof(string)
                        ? $"\t\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} = '{writtenValue}';"
                        : $"\t\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} = {writtenValue};");
                sb.AppendLine("\t} else {");
                sb.AppendLine(
                    $"\t\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} = data.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)};");
                sb.AppendLine("\t}");
            }
            else
            {
                sb.AppendLine(
                    $"\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} = ko.observable(data.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)});");
            }
        }

        private static void BuildObjectProperty(StringBuilder sb, PropertyBag propEntry, JsGeneratorOptions options)
        {
            sb.AppendLine();
            sb.AppendLine($"\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} = new {propEntry.PropertyType.Name}(data.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)});");
        }

        private static void BuildArrayProperty(StringBuilder sb, PropertyBag propEntry, JsGeneratorOptions options)
        {
            sb.AppendLine();
            var collectionType = propEntry.CollectionInnerTypes.First();

            if (!collectionType.IsPrimitiveType)
            {
                sb.AppendLine(
                    $"\tconst mapped{propEntry.PropertyName} = data.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}.map(s => new { Helpers.GetName(propEntry.PropertyType.GetGenericArguments().Single().Name, options.ClassNameConstantsToRemove)}(s));");

                sb.AppendLine(
                    $"\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} = ko.observableArray(mapped{propEntry.PropertyName})");
            }
            else
            {
                sb.AppendLine(
                    $"\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} = ko.observableArray(data.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)})");
            }
        }

        // Implementation of Dictionary will be left same as in Vanilla javascript for now, because of unknown behaviour of observableArrays with this type of variables
        private static void BuildDictionaryProperty(StringBuilder sb, PropertyBag propEntry, JsGeneratorOptions options)
        {
            sb.AppendLine($"\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} = {{}};");
            sb.AppendLine($"\tif(data.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)} != null) {{");
            sb.AppendLine(
                $"\t\tfor (var key in data.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}) {{");
            sb.AppendLine(
                $"\t\t\tif (data.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}.hasOwnProperty(key)) {{");

            var keyType = propEntry.CollectionInnerTypes.First(p => p.IsDictionaryKey);
            if (!AllowedDictionaryKeyTypes.Contains(keyType.Type))
            {
                throw new Exception(
                    $"Dictionaries must have strings, enums, or integers as keys, error found in type: {propEntry.TypeName}");
            }
            var valueType = propEntry.CollectionInnerTypes.First(p => !p.IsDictionaryKey);

            if (!valueType.IsPrimitiveType)
            {
                sb.AppendLine(
                    $"\t\t\t\tif (!overrideObj.{Helpers.GetName(valueType.Type.Name, options.ClassNameConstantsToRemove)}) {{");
                sb.AppendLine(
                    $"\t\t\t\t\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[key] = new {options.OutputNamespace}.{Helpers.GetName(valueType.Type.Name, options.ClassNameConstantsToRemove)}(data.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[key]);");
                sb.AppendLine("\t\t\t\t} else {");
                sb.AppendLine(
                    $"\t\t\t\t\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[key] = new overrideObj.{Helpers.GetName(valueType.Type.Name, options.ClassNameConstantsToRemove)}(data.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[key], overrideObj);");

                sb.AppendLine("\t\t\t\t}");
            }
            else
            {
                sb.AppendLine(
                    $"\t\t\t\tthis.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[key] = data.{Helpers.ToCamelCase(propEntry.PropertyName, options.CamelCase)}[key];");
            }
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");
        }
    }
}
