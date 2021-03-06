﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using TeknikServis.Models.Enums;
using TeknikServis.Models.IdentityModels;

namespace TeknikServis.Models.ViewModels
{
    public class IssueVM
    {
        public string IssueId { get; set; }

        [Required]
        [DisplayName("Müşteri Id")]
        public string CustomerId { get; set; }

        [DisplayName("Operatör Id")]
        public string OperatorId { get; set; }

        [DisplayName("Teknisyen Id")]
        public string TechnicianId { get; set; }

        [StringLength(250)]
        [DisplayName("Açıklama")]
        public string Description { get; set; }

        [DisplayName("Ürün")]
        public ProductTypes ProductType { get; set; }

        [DisplayName("Fotoğraf")]
        public List<string> PhotoPath { get; set; }

        [DisplayName("Güncel Durum")]
        public IssueStates IssueState { get; set; } = IssueStates.Beklemede;

        [DisplayName("Konum")]
        [Required]
        public Locations Location { get; set; }

        [Required]
        [DisplayName("Satın Alma Tarihi")]
        public DateTime PurchasedDate { get; set; }

        [DisplayName("Garanti Durumu")]
        public bool WarrantyState { get; set; } = false;

        [DisplayName("Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [DisplayName("Servis Bedeli")]
        public decimal ServiceCharge { get; set; } = 100;

        [StringLength(250)]
        [DisplayName("Operatör Notu")]
        public string OptReport { get; set; }

        [StringLength(250)]
        [DisplayName("Teknisyen Raporu")]
        public string TechReport { get; set; }

        [DisplayName("Arıza Kapanma Tarihi")]
        public DateTime? ClosedDate { get; set; }

        public string SurveyCode { get; set; }

        [DisplayName("Fotoğraf")]
        public List<HttpPostedFileBase> PostedPhoto { get; set; }

        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }

        [ForeignKey("OperatorId")]
        public virtual User Operator { get; set; }

        [ForeignKey("TechnicianId")]
        public virtual User Technician { get; set; }
    }
}
