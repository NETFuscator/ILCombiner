﻿using System.IO;

using ILCombiner.Combiners;

using dnlib;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace ILCombiner {
    public class ILCombiner {
        public enum MergeMethod {
            EmbeddedAssemblyResolver
        }

        public static ModuleDef Embed(ModuleDef module, ILCombinerDependency[] dependencies, MergeMethod method) {
            ICombiner combiner;

            switch (method) {
                case MergeMethod.EmbeddedAssemblyResolver:
                    combiner = new EmbeddedAssemblyResolver();
                    break;

                default:
                    throw new ILCombinerException("Unknown or unsupported method!");
            }

            return combiner.Combine(module, dependencies);
        }

        public static ModuleDef Embed(ModuleDef module, string[] dependencyPaths, MergeMethod method) {
            var dependencies = new ILCombinerDependency[dependencyPaths.Length];

            for (var i = 0; i < dependencies.Length; i++)
                dependencies[i] = new ILCombinerDependency(Path.GetFileName(dependencyPaths[i]), File.ReadAllBytes(dependencyPaths[i]));

            return Embed(module, dependencies, method);
        }

        public static ModuleDef Embed(string modulepath, string[] dependencyPaths, MergeMethod method) {
            return Embed(ModuleDefMD.Load(File.ReadAllBytes(modulepath)), dependencyPaths, method);
        }
    }
}
