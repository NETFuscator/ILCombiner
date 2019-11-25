using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Security.Cryptography;

namespace ILCombiner.Runtime {
    internal class EmbeddedAssemblyResolver {
        internal static void Initialize() {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }

        private static string HashMD5(string text) {
            return string.Join("", MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(text)).Select(x => x.ToString("x2")));
        }

        private static Assembly ResolveAssembly(object sender, ResolveEventArgs args) {
            var stream = typeof(EmbeddedAssemblyResolver).Assembly.GetManifestResourceStream(HashMD5(args.Name));

            if (stream == null)
                return null;

            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);

            return Assembly.Load(buffer);
        }
    }
}