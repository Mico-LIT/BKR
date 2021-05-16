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
        public static string ResourceNameTestData => "WKR2.ExampleData.TestData.xls";
        public static string ResourceNameImageTemplate => "WKR2.ExampleData.ImageTemplate.jpg";
        public static string PathLocal => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
