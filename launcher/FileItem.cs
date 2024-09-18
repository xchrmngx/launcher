using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace launcher
{
    public class FileItem
    {
        public string FileName { get; set; }
        public string FileNameWithoutExtension { get; set; }
        public ImageSource FileIcon { get; set; }
    }
}
