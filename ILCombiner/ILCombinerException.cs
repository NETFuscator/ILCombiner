using System;
using System.Collections.Generic;
using System.Text;

namespace ILCombiner {
    public class ILCombinerException : Exception {
        public ILCombinerException(string message) : base(message) { }
    }
}
