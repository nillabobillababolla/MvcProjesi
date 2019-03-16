using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TeknikServis.Models.Abstracts
{
    public abstract class BaseEntity<T>
    {
        [Key]
        [Column(Order = 1)]
        public T Id { get; set; }

        [DisplayName("Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [DisplayName("Güncelleme Tarihi")]
        public DateTime? UpdatedDate { get; set; }

        public T ShallowCopy()
        {
            return (T)this.MemberwiseClone();
        }

        public T DeepCopy()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

    }
}
