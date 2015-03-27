using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BuildAlert
{
    static class Resources
    {
        public static string ExtractToTemporaryPath(string path)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()+".bmp");
            Assembly a = Assembly.GetExecutingAssembly();
            using(Stream s = a.GetManifestResourceStream("BuildAlert." + path))
            {
                using(BinaryReader r = new BinaryReader(s))
                {
                    using(var outp = File.Create(tempPath))
                    {
                        s.CopyTo(outp);
                    }
                }
            }
            return tempPath;
        }
    }
}
