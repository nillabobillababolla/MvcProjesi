using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeknikServis.Models.ViewModels
{
    public class ProductVM
    {
        [Required]
        public string Id { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "Ürün adı 1 ile 100 karakter aralığında olmalıdır.")]
        [Required]
        [DisplayName("Ürün Adı")]
        public string ProductName { get; set; }
        
        [StringLength(250)]
        [DisplayName("Açıklama")]
        public string Description { get; set; }
    }
}
