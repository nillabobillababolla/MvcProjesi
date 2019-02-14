using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Models.Entities;
using TeknikServis.Models.Enums;
using TeknikServis.Models.IdentityModels;

namespace TeknikServis.Models.ViewModels
{
    public class IssueVM
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string CustomerId { get; set; }

        public string OperatorId { get; set; }
        public string TechnicianId { get; set; }

        [DisplayName("Güncel Durum")]
        public IssueStates IssueState { get; set; }

        [DisplayName("Enlem")]
        [Required]
        public string Latitude { get; set; }

        [DisplayName("Boylam")]
        [Required]
        public string Longitude { get; set; }

        [Required]
        [DisplayName("Satın Alma Tarihi")]
        public DateTime PurchasedDate { get; set; }

        [DisplayName("Garanti Durumu")]
        public bool WarrantyState { get; set; }

        [DisplayName("Arıza Kapanma Tarihi")]
        public DateTime? ClosedDate { get; set; }

        [DisplayName("Servis Bedeli")]
        public decimal ServiceCharge { get; set; }

        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }

        [ForeignKey("OperatorId")]
        public virtual User Operator { get; set; }

        [ForeignKey("TechnicianId")]
        public virtual User Technician { get; set; }
    }
}
