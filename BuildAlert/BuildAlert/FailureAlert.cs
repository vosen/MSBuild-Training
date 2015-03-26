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
            string failPath = Resources.ExtractToTemporaryPath("fail.png");
            Background.Set(failPath);
            return true;
        }
    }
}
