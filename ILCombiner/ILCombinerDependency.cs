using System;
using System.Collections.Generic;
using System.Text;

namespace ILCombiner {
    public class ILCombinerDependency {
        public ILCombinerDependency(string name, byte[] buffer) {
            this.Name = name;
            this.Buffer = buffer;
        }

        public string Name { get; }
        public byte[] Buffer { get; }
    }
}
