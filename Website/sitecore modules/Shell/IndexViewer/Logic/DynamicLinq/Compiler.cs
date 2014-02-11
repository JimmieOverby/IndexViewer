using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.CSharp;

namespace IndexViewer.Logic.DynamicLinq
{
    public class Compiler
    {
        public static Tuple<string, IEnumerable<object>> CompileAndRun(string code, string searchItemAssemblyName)
        {
            CompilerParameters options = new CompilerParameters();
            Directory.GetCurrentDirectory();
            options.GenerateInMemory = true;
            options.TreatWarningsAsErrors = false;
            options.GenerateExecutable = false;
            options.CompilerOptions = "/optimize";
            string[] strArray = new string[] { "System.dll", "mscorlib.dll", "System.Core.dll" };
            string[] strArray2 = new string[] { "Sitecore.Kernel.dll", "Sitecore.Buckets.dll", "Sitecore.ContentSearch.dll", "Sitecore.ContentSearch.Linq.dll", searchItemAssemblyName + ".dll" };
            for (int i = 0; i < strArray2.Length; i++)
            {
                strArray2[i] = Path.Combine(Path.GetDirectoryName(HttpContext.Current.Request.PhysicalApplicationPath + @"bin\"), strArray2[i]);
            }
            options.ReferencedAssemblies.AddRange(strArray2);
            options.ReferencedAssemblies.AddRange(strArray);
            CompilerResults results = new CSharpCodeProvider().CompileAssemblyFromSource(options, new string[] { code });
            if (results.Errors.HasErrors)
            {
                string str = "Compile error: ";
                foreach (CompilerError error in results.Errors)
                {
                    str = str + "rn" + error.ToString();
                }
                return new Tuple<string, IEnumerable<object>>(str, new List<object>());
            }
            ExploreAssembly(results.CompiledAssembly);
            Module module = results.CompiledAssembly.GetModules()[0];
            Type type = null;
            MethodInfo method = null;
            MethodInfo info2 = null;
            if (module != null)
            {
                type = module.GetType("Test.Program");
            }
            if (type != null)
            {
                method = type.GetMethod("Main");
                info2 = type.GetMethod("RunTimer");
            }
            if (method != null)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                IEnumerable<object> enumerable = method.Invoke(null, new object[] { "here in dyna code" }) as IEnumerable<object>;
                stopWatch.Stop();
                return new Tuple<string, IEnumerable<object>>(stopWatch.ElapsedMilliseconds.ToString(), enumerable);
            }
            return new Tuple<string, IEnumerable<object>>("0", new List<object>());
        }



        private static void ExploreAssembly(Assembly assembly)
        {
            Console.WriteLine("Modules in the assembly:");
            foreach (Module module in assembly.GetModules())
            {
                Console.WriteLine("{0}", module);
                foreach (Type type in module.GetTypes())
                {
                    Console.WriteLine("t{0}", type.Name);
                    foreach (MethodInfo info in type.GetMethods())
                    {
                        Console.WriteLine("tt{0}", info.Name);
                    }
                }
            }
        }

        
    }
}