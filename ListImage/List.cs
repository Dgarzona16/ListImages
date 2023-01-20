using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ListImage
{
    internal class List
    {
        List<Image> img = new List<Image>();
        
        string _path = @"\\192.168.0.204\productos\ecommerce";
        string filename,filenamewithoutExt;

        /// <summary>
        /// Extare y crea la lista de imagenes de la carpeta
        /// </summary>
        public void GetList()
        {
            List<string> extension = new List<string> { ".jpg", ".png"};

            //extraer todo el contenido de la carpeta
            var files = Directory.GetFiles(_path)
                .Where(x => extension.Contains(Path.GetExtension(x).ToLower()))
                .ToList();

            files = files
                .Where(x =>!(Path.GetFileName(x).Contains("Mesacron")
                            || Path.GetFileName(x).Contains("default")
                            || Path.GetFileName(x).Contains("Empazhin")
                            || Path.GetFileName(x).Contains("Thumb")
                            || Path.GetFileName(x).Contains("no autorizado")))
                .ToList();

            img = (from f in files
                   select new Image { File = Path.GetFileName(f) }).ToList();
        }

        /// <summary>
        /// crea el archivo txt con la lista de imagenes
        /// </summary>
        public void CreateFile()
        {
            //create a txt file in current domain
            using (var cw = new StreamWriter(@"C:\temp\list.txt"))
            {
                foreach (var item in img)
                {
                    cw.WriteLine(item.File);
                }
            }
        }
    }
}
