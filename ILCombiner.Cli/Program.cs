using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCombiner.Cli {
    internal class Program {
        static void Main(string[] args) {
            if (args.Length < 3) {
                Console.WriteLine("ILCombiner.Cli.exe <file> <dependencies...>");
                return;
            }

            try {
                var dependencies = args.Skip(1).ToArray();

                ILCombiner.Embed(args[0], dependencies, ILCombiner.MergeMethod.EmbeddedAssemblyResolver).Write(args[0]);

                foreach (var dependency in dependencies)
                    File.Delete(dependency);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
