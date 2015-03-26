using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoStudio.Model
{
    class StringLogger : ConsoleLogger
    {
        private StringBuilder sb;

        public StringLogger()
        {
            base.WriteHandler = new WriteHandler(Write);
        }

        public override void Initialize(IEventSource eventSource)
        {
            sb = new StringBuilder();
            base.Initialize(eventSource);
        }

        public override void Initialize(IEventSource eventSource, int nodeCount)
        {
            sb = new StringBuilder();
            base.Initialize(eventSource, nodeCount);
        }

        public string GetText()
        {
            return sb.ToString();
        }

        private void Write(string message)
        {
            sb.Append(message);
        }
    }
}
