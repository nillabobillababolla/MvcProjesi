using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using TeknikServis.Models.Abstracts;
using TeknikServis.Models.Enums;
using TeknikServis.Models.IdentityModels;

namespace TeknikServis.Models.Entities
{
    public class Issue : BaseEntity<string>
    {
        public Issue()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string CustomerId { get; set; }
        public string OperatorId { get; set; }
        public string TechnicianId { get; set; }

        [DisplayName("Arıza Kapanma Tarihi")]
        public DateTime? ClosedDate { get; set; }

        [DisplayName("Güncel Durum")]
        public IssueStates IssueState { get; set; }

        [DisplayName("Enlem")]
        public string Latitude { get; set; }
        [DisplayName("Boylam")]
        public string Longitude { get; set; }

        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }

        [ForeignKey("OperatorId")]
        public virtual User Operator { get; set; }

        [ForeignKey("TechnicianId")]
        public virtual User Technician { get; set; }
    }
}
