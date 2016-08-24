using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentagon.Models
{
    public class ChapterContent
    {
        public string ChapterName { get; set; }
        public string Description { get; set; }
        public int TypeOfFile { get; set; }
        public string FileContents { get; set; }
    }
}