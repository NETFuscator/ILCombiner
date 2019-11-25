using dnlib.DotNet;

namespace ILCombiner.Combiners {
    internal interface ICombiner {
        ModuleDef Combine(ModuleDef module, ILCombinerDependency[] dependencies);
    }
}
