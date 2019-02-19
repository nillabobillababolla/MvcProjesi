using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TeknikServis.Models.Enums;

namespace TeknikServis.Models.IdentityModels
{
    public class User : IdentityUser
    {
        [StringLength(50)]
        [Required]
        [DisplayName("Adı")]
        public string Name { get; set; }

        [StringLength(60)]
        [Required]
        [DisplayName("Soyadı")]
        public string Surname { get; set; }

        [DisplayName("Kayıt Tarihi")]
        public DateTime RegisterDate { get; set; } = DateTime.Now;

        [DisplayName("Aktivasyon Kodu")]
        public string ActivationCode { get; set; }

        public string AvatarPath { get; set; }

        [DisplayName("Konum")]
        public Locations Location { get; set; }
    }
}
