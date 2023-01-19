using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ListImage
{
    internal class List
    {
        List<ImageList> _list = new List<ImageList>();
        
        string _path = @"\\192.168.0.204\productos\ecommerce";
        string filename,filenamewithoutExt;

        /// <summary>
        /// Extare y crea la lista de imagenes de la carpeta
        /// </summary>
        public void GetList()
        {
            //extraer todo el contenido de la carpeta
            var files = Directory.GetFiles(_path).ToList();

            //recorrer cada archivo
            foreach (var file in files)
            {
                //descarta el contenido
                if (file.Contains("Thumbs") 
                    || file.Contains(".ini") 
                    || file.Contains("Empazhin") 
                    || file.Contains("Mesacron") 
                    || file.Contains("default")
                    || file.Contains("no autorizado"))
                    continue;
                
                //obtiene el nombre del archivo
                filenamewithoutExt = Path.GetFileNameWithoutExtension(file);
                filename = Path.GetFileName(file);

                //validar si es una imagen de sublista sin imagen principal
                if (file.Contains("_"))
                {
                    var code = filenamewithoutExt.Split('_')[0];
                    var ext = filename.Split('.')[1];
                    var verifyFile = (from f in _list
                               where f.Image.File.Equals(code + "." + ext)
                                select f).ToList();
                    if (verifyFile.Any())
                        continue;
                    else
                    {
                        var verifyFile2 = (from f in _list
                                          where f.Image.File.Equals(code)
                                          select f).ToList();
                        
                        if (verifyFile2.Any())
                            continue;
                        else
                        {
                        filename = code;
                        filenamewithoutExt = filename;
                        }
                    }
                }

                //rellena la imagen con el nombre del archivo
                var image = new Image
                {
                    File = filename
                };

                //rellena la lista con la imagen
                var imageList = new ImageList
                {
                    Image = image,
                    SubLists = new List<Image>()
                };

                //comprueba que los png no lleven sublistas asocidas
                if (!filename.Contains(".png"))
                {
                    var sublist = (from f in files
                                   where f.Contains(filenamewithoutExt + "_")
                                   select Path.GetFileName(f)).ToList();

                    foreach (var sub in sublist)
                    {
                        imageList.SubLists.Add(new Image { File = sub });
                    }
                }
                //agregar a la lista principal 
                _list.Add(imageList);
            }
        }

        /// <summary>
        /// crea el archivo txt con la lista de imagenes
        /// </summary>
        public void CreateFile()
        {
            //create a txt file in current domain
            using (var cw = new StreamWriter(@"C:\temp\list.txt"))
            {
                foreach (var item in _list)
                {
                    cw.WriteLine(item.Image.File);
                    foreach (var sub in item.SubLists)
                    {
                        cw.WriteLine("\t" + sub.File);
                    }
                }
            }
        }
    }
}
