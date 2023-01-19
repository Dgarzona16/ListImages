using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListImage
{
    public class Image
    {
        public string File { get; set; }
    }

    public class ImageList
    {
        public Image Image { get; set; }
        public List<Image> SubLists { get; set; }
    }
}
