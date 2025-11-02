using System.ComponentModel.DataAnnotations;

namespace ForumApp.Models.ViewModels
{
    public class CreatePostViewModel
    {
        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir")]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik zorunludur")]
        [Display(Name = "İçerik")]
        public string Content { get; set; } = string.Empty;
    }
}
