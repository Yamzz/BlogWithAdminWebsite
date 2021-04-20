using Guffaw.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Guffaw.Models
{
    public class HomeViewModel
    {
        public IEnumerable<BlogPost> BlogPostVM { get; set; }
        public Contact ContactVM { get; set; }
    }
}