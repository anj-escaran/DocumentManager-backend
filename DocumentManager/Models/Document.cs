using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManager.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string ControlNumber { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int Revision { get; set; }
        public DateTime UploadedDate { get; set; }
        public string FilePath { get; set; }
        public string Filename { get; set; }
    }
}
