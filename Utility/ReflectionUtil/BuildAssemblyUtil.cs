/*
 * Idea taken from: https://www.codeproject.com/Articles/9019/Compiling-and-Executing-Code-at-Runtime
 * More explanation on: https://www.codeproject.com/Articles/12499/Run-Time-Code-Generation-I-Compile-C-Code-using-Mi
 */

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace Utility.ReflectionUtil
{
    public static class BuildAssemblyUtil
    {
        public static Assembly BuildAssembly(string code)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            ICodeCompiler compiler = provider.CreateCompiler();
            CompilerParameters compilerparams = new CompilerParameters();
            compilerparams.GenerateExecutable = false;
            compilerparams.GenerateInMemory = true;
            CompilerResults results =
                compiler.CompileAssemblyFromSource(compilerparams, code);
            if (results.Errors.HasErrors)
            {
                StringBuilder errors = new StringBuilder("Compiler Errors :\r\n");
                foreach (CompilerError error in results.Errors)
                {
                    errors.AppendFormat("Line {0},{1}\t: {2}\n",
                        error.Line, error.Column, error.ErrorText);
                }
                throw new Exception(errors.ToString());
            }
            return results.CompiledAssembly;
        }

        public static List<TypeInfo> GetDefinedTypeInfo(Assembly asm)
        {
            return asm.DefinedTypes.ToList();
        }

        public static List<Type> GetExportedTypes(Assembly asm)
        {
            return asm.ExportedTypes.ToList();
        }

        public static List<Type> FilterExportedTypes(List<Type> rawTypes)
        {
            List<Type> retList = new List<Type>();

            foreach (var type in rawTypes)
            {
                List<Type> otherTypes = rawTypes.Where(s => s != type).ToList();

                var passedTest = true;
                foreach (var otherTypeItem in otherTypes)
                {
                    // Get all of the properties in this class
                    List<PropertyInfo> decalredProps = otherTypeItem.GetTypeInfo().DeclaredProperties.ToList();

                    foreach (var decalredProp in decalredProps)
                    {
                        // if (decalredProp.Name == type.Name)
                        // I can't get property type name like this, cause I get var name actually, not property name
                        //   Owner            -     OwnerInformation

                        string propertyTypeName = decalredProp.PropertyType.IsGenericType
                            ? decalredProp.PropertyType.GetGenericArguments().Single().Name
                            : decalredProp.PropertyType.Name;

                        if (propertyTypeName == type.Name)
                        {
                            passedTest = false;
                        }
                    }
                }
                // Only if type passed all of the tests, then we insert it into return list
                if (passedTest)
                    retList.Add(type);
            }

            return retList;
        }
    }
}
