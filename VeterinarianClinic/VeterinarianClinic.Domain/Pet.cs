using System;
using System.Data.Linq.Mapping;
using System.Xml.Serialization;

namespace VeterinarianClinic.Domain
{
    [Table(Name = "Pet")]
    public class Pet : ICloneable
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column]
        public int IdSpecie { get; set; }

        [Column]
        public string Race { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public DateTime BirthDay { get; set; }

        [Column]
        public decimal Weight { get; set; }

        [Column]
        public int IdOwner { get; set; }

        [XmlIgnore]
        public string BirthDayStr
        {
            get
            {
                return BirthDay.ToString("MM/dd/yyyy");
            }
        }

        private Specie specie;
        private Client owner;

        [Association(IsForeignKey = true, ThisKey = "IdSpecie")]
        [XmlIgnore]
        public Specie Specie { get; set; }

        [Association(IsForeignKey = true, ThisKey = "IdOwner")]
        [XmlIgnore]
        public Client Owner { get; set; }

        public Pet()
        {
            BirthDay = DateTime.Now.Date.AddDays(-1);
        }

        public object Clone()
        {
            return new Pet()
            {
                BirthDay = this.BirthDay,
                Id = this.Id,
                IdOwner = this.IdOwner,
                IdSpecie = this.IdSpecie,
                Name = this.Name,
                Owner = this.Owner,
                Race = this.Race,
                Specie = this.specie,
                Weight = this.Weight
            };
        }
    }
}
