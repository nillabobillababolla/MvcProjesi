using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using TeknikServis.Models.Abstracts;

namespace TeknikServis.Models.Entities
{
    public class Category : BaseEntity<string>
    {
        public Category()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [StringLength(100, ErrorMessage = "Kategori Adı 3 ile 100 karakter arasında olabilir", MinimumLength = 3)]
        [DisplayName("Kategori Adı")]
        [Required]
        public string CategoryName { get; set; }

        [DisplayName("Üst Kategori")]
        public string SupCategoryId { get; set; }

        [StringLength(150)]
        [DisplayName("Açıklama")]
        public string Description { get; set; }


        [ForeignKey("SupCategoryId")]
        public virtual Category SupCategory { get; set; }
        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}