using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TeknikServis.Models.Entities;
using TeknikServis.Models.Enums;
using TeknikServis.Models.IdentityModels;

namespace TeknikServis.Models.ViewModels
{
    public class IssueVM
    {
        [Required]
        public string CustomerId { get; set; }
        public string OperatorId { get; set; }
        public string TechnicianId { get; set; }

        [Required]
        public string ProductId { get; set; }

        [StringLength(250)]
        [DisplayName("Açıklama")]
        public string Description { get; set; }

        public string PhotoPath { get; set; }
        public string BillPath { get; set; }

        [DisplayName("Güncel Durum")]
        public IssueStates IssueState { get; set; } = IssueStates.Created;

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

        [DisplayName("Arıza Oluşturma Tarihi")]
        public DateTime OpenedDate { get; set; } = DateTime.Now;

        [DisplayName("Servis Bedeli")]
        public decimal ServiceCharge { get; set; } = 100;

        [StringLength(250)]
        [DisplayName("Rapor")]
        public string Report { get; set; }

        [DisplayName("Arıza Kapanma Tarihi")]
        public DateTime? ClosedDate { get; set; }

        public HttpPostedFile PostedBill { get; set; }
        public HttpPostedFile PostedPhoto { get; set; }

        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }

        [ForeignKey("OperatorId")]
        public virtual User Operator { get; set; }

        [ForeignKey("TechnicianId")]
        public virtual User Technician { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
