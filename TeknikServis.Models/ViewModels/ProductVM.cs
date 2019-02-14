using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Models.Entities;
using TeknikServis.Models.Enums;

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

        [DisplayName("Ürün Markası")]
        public ProductBrands ProductBrand { get; set; }

        [DisplayName("Ürün Tipi")]
        public ProductTypes ProductType { get; set; }

        [StringLength(250)]
        [DisplayName("Açıklama")]
        public string Description { get; set; }
    }
}
