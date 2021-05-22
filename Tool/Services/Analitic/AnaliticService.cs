using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Tool.Services.Analitic.Models;

namespace Tool.Services.Analitic
{
    public class AnaliticService
    {
        public bool? GetSettingOnAnalitic { get; set; }

        public Params PARAMS = new Params();
        readonly string pathLocal;

        public AnaliticService(string pathLocal)
        {
            this.pathLocal = pathLocal;
        }

        public void Save_Persont(Image image, string nameFile)
        {
            DirectoryInfo directoryInfo = Directory.CreateDirectory(this.pathLocal);
            string path = string.Format(@"{0}\{1}.jpeg", directoryInfo.FullName, nameFile);

            if (File.Exists(path))
            {
                nameFile = "Repeat" + DateTime.Now.TimeOfDay.Milliseconds.ToString() + "_" + nameFile;
                path = string.Format(@"{0}\{1}.jpeg", directoryInfo.FullName, nameFile);
            }

            image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
