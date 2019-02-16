using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TeknikServis.Models.Abstracts;
using TeknikServis.Models.Enums;

namespace TeknikServis.Models.Entities
{
    public class Product : BaseEntity<string>
    {
        public Product()
        {
            Id = Guid.NewGuid().ToString();
        }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "Ürün adı 1 ile 100 karakter aralığında olmalıdır.")]
        [Required]
        [DisplayName("Ürün Adı")]
        public string ProductName { get; set; }
        
        [StringLength(250)]
        [DisplayName("Açıklama")]
        public string Description { get; set; }
    }
}