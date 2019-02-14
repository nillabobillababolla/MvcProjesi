using System.ComponentModel.DataAnnotations;

namespace TeknikServis.Models.ViewModels
{
    public class ExternalLoginVM
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }
    }
}
