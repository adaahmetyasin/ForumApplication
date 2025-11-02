using System.ComponentModel.DataAnnotations;

namespace ForumApp.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik zorunludur")]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }

        // Foreign Key
        [Required]
        public string UserId { get; set; } = string.Empty;

        // Navigation properties
        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
