using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoStudio.Model
{
    public class BuildResult
    {
        public bool Success { get; private set; }
        public string Output { get; private set; }

        public BuildResult(bool result, string outp)
        {
            Success = result;
            Output = outp;
        }
    }
}
