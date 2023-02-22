/*
 * References for BuildAssembly method with "ICodeCompiler"
 * Idea taken from: https://stackoverflow.com/questions/8018417/how-to-load-a-class-from-a-cs-file
 * Detailed: https://www.codeproject.com/Articles/9019/Compiling-and-Executing-Code-at-Runtime
 * More explanation on: https://www.codeproject.com/Articles/12499/Run-Time-Code-Generation-I-Compile-C-Code-using-Mi
 *
 * References for BuildAssembly method with "CodeDomProvider"
 * https://msdn.microsoft.com/en-us/library/system.codedom.compiler.codedomprovider(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-2
 * search for: CodeCompile method
 *
 *
 * Error "error CS1056: Unexpected character '$'" was fixed installing thing mentioned here: https://stackoverflow.com/questions/59332763/why-am-i-getting-error-cs1056-unexpected-character-putting-a-mark-here
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
        // Should probably deprecate this, but a lot of effort went into making it
        // I solemnly refuse to do so :D
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

        // You should probably use this method for everything else
        public static Assembly CompileCode(String sourceFile)
        {
            CodeDomProvider provider = new CSharpCodeProvider();
            CompilerParameters compilerParams = new CompilerParameters();
            compilerParams.GenerateExecutable = false;
            compilerParams.GenerateInMemory = true;

            // Add dll's here
            compilerParams.ReferencedAssemblies.Add("Utility.dll");

            // Invoke compilation.
            // CompilerResults results = provider.CompileAssemblyFromFile(compilerParams, sourceFile); // should have used this, but it's late, 2 late now :'(
            CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, sourceFile);

            // Return the results of compilation.
            if (results.Errors.HasErrors)
            {
                StringBuilder errors = new StringBuilder("Compiler Errors :\r\n");
                foreach (CompilerError error in results.Errors)
                {
                    errors.AppendFormat("Line {0},{1}\t: {2}\n",
                        error.Line, error.Column, error.ErrorText);

                    System.Diagnostics.Debug.WriteLine("Line {0},{1}\t: {2}\n", error.Line, error.Column, error.ErrorText);
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

                bool passedTest = true;
                foreach (Type otherTypeItem in otherTypes)
                {
                    // Get all of the properties in this class
                    List<PropertyInfo> decalredProps = otherTypeItem.GetTypeInfo().DeclaredProperties.ToList();

                    foreach (PropertyInfo decalredProp in decalredProps)
                    {
                        // if (decalredProp.Name == type.Name)
                        // I can't get property type name like this, cause I get var name actually, not property name
                        //   Owner            -     OwnerInformation

                        // Have to check if value is generic type e.g. List<T> and have to check if it isn't Dictionary cause Dictionary is
                        // Seen as T, but contains 2 primitive values, which doesn't get me
                        string propertyTypeName = decalredProp.PropertyType.IsGenericType && decalredProp.PropertyType.GetGenericTypeDefinition() != typeof(Dictionary<,>)
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
