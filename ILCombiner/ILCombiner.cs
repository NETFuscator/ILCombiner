using System.IO;

using ILCombiner.Combiners;

using dnlib;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace ILCombiner {
    public class ILCombiner {
        public enum CombinerMethod {
            EmbeddedAssemblyResolver,
            AssemblyMerger
        }

        public static ModuleDef Embed(ModuleDef module, ILCombinerDependency[] dependencies, CombinerMethod method) {
            ICombiner combiner;

            switch (method) {
                case CombinerMethod.EmbeddedAssemblyResolver:
                    combiner = new EmbeddedAssemblyResolver();
                    break;

                case CombinerMethod.AssemblyMerger:
                    combiner = new AssemblyMerger();
                    break;

                default:
                    throw new ILCombinerException("Unknown or unsupported method!");
            }

            return combiner.Combine(module, dependencies);
        }

        public static ModuleDef Embed(ModuleDef module, string[] dependencyPaths, CombinerMethod method) {
            var dependencies = new ILCombinerDependency[dependencyPaths.Length];

            for (var i = 0; i < dependencies.Length; i++)
                dependencies[i] = new ILCombinerDependency(Path.GetFileName(dependencyPaths[i]), File.ReadAllBytes(dependencyPaths[i]));

            return Embed(module, dependencies, method);
        }

        public static ModuleDef Embed(string modulepath, string[] dependencyPaths, CombinerMethod method) {
            return Embed(ModuleDefMD.Load(File.ReadAllBytes(modulepath)), dependencyPaths, method);
        }
    }
}
