using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCombiner.Cli {
    internal class Program {
        static void Main(string[] args) {
            if (args.Length < 2) {
                Console.WriteLine("ILCombiner.Cli.exe <file> <dependencies...>");
                return;
            }

            try {
                var dependencies = args.Skip(1).ToArray();

                ILCombiner.Embed(args[0], dependencies, ILCombiner.CombinerMethod.AssemblyMerger).Write($"{Path.GetDirectoryName(args[0])}\\{Path.GetFileNameWithoutExtension(args[0])}-merged{Path.GetExtension(args[0])}");
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
