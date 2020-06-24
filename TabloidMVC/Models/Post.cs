using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TabloidMVC.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter a Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Enter a Post")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Upload an Image")]
        [DisplayName("Header Image URL")]
        public string ImageLocation { get; set; }

        public DateTime CreateDateTime { get; set; }

        [Required(ErrorMessage = "Select a Publication Date")]
        [DisplayName("Published")]
        [DataType(DataType.Date)]
        public DateTime? PublishDateTime { get; set; }

        public bool IsApproved { get; set; }

        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [DisplayName("Author")]
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
