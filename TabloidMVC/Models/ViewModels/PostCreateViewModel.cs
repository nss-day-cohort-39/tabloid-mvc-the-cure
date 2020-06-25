using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostCreateViewModel
    {
        public Post Post { get; set; }
        public List<Category> CategoryOptions { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
