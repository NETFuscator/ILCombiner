using dnlib.DotNet;

namespace ILCombiner {
    public class ILCombinerDependency {
        public ILCombinerDependency(string name, byte[] buffer) {
            this.Name = name;
            this.Buffer = buffer;
            this.Module = ModuleDefMD.Load(buffer);
        }

        public string Name { get; }
        public byte[] Buffer { get; }
        public ModuleDef Module { get; }
    }
}
