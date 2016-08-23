using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentagon.Models
{
    public class Chapters
    {
        public int ChapterID { get; set; }
        public int TutorialID { get; set; }
        public int HierarchyLevel { get; set; }
        public string ChapterName { get; set; }
        public string Description { get; set; }
        public int TypeOfFile { get; set; }
        public string FileContents { get; set; }
    }
}