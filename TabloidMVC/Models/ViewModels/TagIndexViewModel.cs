using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class TagIndexViewModel
    {
        public Post Post { get; set; }
        public List<Tag> PostTags { get; set; }
    }
}
