using System.Linq;
using System.Text;
using System.Security.Cryptography;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace ILCombiner.Combiners {
    internal class EmbeddedAssemblyResolver : ICombiner {
        public ModuleDef Combine(ModuleDef module, ILCombinerDependency[] dependencies) {
            var runtime = ModuleDefMD.Load("ILCombiner.Runtime.dll");

            var type = Helpers.InjectHelper.Inject(runtime.Find("ILCombiner.Runtime.EmbeddedAssemblyResolver", false), module);

            module.Types.Add(type);

            module.GlobalType.FindOrCreateStaticConstructor().Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(type.FindMethod("Initialize")));

            foreach (var dependency in dependencies) {
                module.Resources.Add(new EmbeddedResource(HashMD5(ModuleDefMD.Load(dependency.Buffer).Assembly.GetFullNameWithPublicKeyToken()), dependency.Buffer));
            }

            return module;
        }

        private static string HashMD5(string text) {
            return string.Join("", MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(text)).Select(x => x.ToString("x2")));
        }
    }
}
