using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeknikServis.Models.Abstracts;

namespace TeknikServis.Models.Entities
{
    public class Photograph : BaseEntity<string>
    {
        public Photograph()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [DisplayName("Yol")]
        public string Path { get; set; }

        public string IssueId { get; set; }

        [ForeignKey("IssueId")]
        public virtual Issue Issue { get; set; }
    }
}
