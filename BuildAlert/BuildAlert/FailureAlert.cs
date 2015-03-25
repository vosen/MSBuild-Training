using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildAlert
{
    public class FailureAlert : Task
    {
        public override bool Execute()
        {
            Background.Set(@"D:\Users\vosen\Documents\Visual Studio 2013\Projects\MotoStudio\Examples\Targets\fail.png");
            return true;
        }
    }
}
