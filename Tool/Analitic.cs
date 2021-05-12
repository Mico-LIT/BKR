using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Tool
{
    public class Analitic
    {
        public static bool GetSettingOnAnalitic { get; set; }
        [Serializable]
        public class Params
        {
            public int Params_1 { get; set; }
            public int Params_2 { get; set; }

        }

        static public Params PARAMS = new Params();
        internal static void Save_Persont(Image image , string str)
        {
            DirectoryInfo di = Directory.CreateDirectory(Print.PATH_LOCAL + @"\Analitic");
            string path= string.Format(di.FullName + @"\{0}.jpeg",str);
            if (File.Exists(path)) {
                str = "Repeat" + DateTime.Now.TimeOfDay.Milliseconds.ToString() +"_"+ str;
                path = string.Format(di.FullName + @"\{0}.jpeg", str);
                image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
            image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

            }
        }

    }
}
