using System.IO;
using System.Collections.Generic;

using dnlib;
using dnlib.DotNet;

namespace ILCombiner {
    public class ILCombinerContext {
        public ILCombinerContext(ModuleDef module, ModuleDef[] dependencies) {
            this.Module = module;
            this.Dependencies = dependencies;
            this.Importer = new Importer(module);
        }

        public ModuleDef Module { get; }
        public ModuleDef[] Dependencies { get; }
        public Importer Importer { get; }
    }
}