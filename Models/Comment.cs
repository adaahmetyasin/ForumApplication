using System.ComponentModel.DataAnnotations;

namespace ForumApp.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Yorum içeriği zorunludur")]
        [StringLength(1000, ErrorMessage = "Yorum en fazla 1000 karakter olabilir")]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }

        // Foreign Keys
        [Required]
        public int PostId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        // Navigation properties
        public virtual Post? Post { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
