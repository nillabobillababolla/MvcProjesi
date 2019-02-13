using System;
using System.ComponentModel;
using TeknikServis.Models.Abstracts;
using TeknikServis.Models.Enums;

namespace TeknikServis.Models.Entities
{
    public class Issue : BaseEntity<Guid>
    {
        public Issue()
        {
            this.Id = Guid.NewGuid();
        }

        public string CustomerId { get; set; }
        public string OperatorId { get; set; }
        public string TechnicianId { get; set; }

        [DisplayName("Oluşturma Tarihi")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [DisplayName("Güncel Durum")]
        public IssueStates IssueState { get; set; }

        [DisplayName("Enlem")]
        public string Latitude { get; set; }
        [DisplayName("Boylam")]
        public string Longitude { get; set; }
    }
}
