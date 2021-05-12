using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WKR2.Core
{
    class AppSettings
    {
        static public string PathLocal => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
