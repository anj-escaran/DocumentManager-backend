using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DocumentManager.Models
{
    public class DocumentFile
    {
        public string ControlNumber { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int Revision { get; set; }
        public IFormFile File { get; set; }
    }
}
