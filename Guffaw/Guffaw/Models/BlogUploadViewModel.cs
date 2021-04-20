using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Guffaw.Models
{
    public class BlogUploadViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Post { get; set; }
        public DateTime DateTime { get; set; }
        public string PostedBy { get; set; }
        public string ImageURL { get; set; }
        public byte[] Image { get; set; }
        public List<HttpPostedFileBase> Files { get; set; }
    }
}