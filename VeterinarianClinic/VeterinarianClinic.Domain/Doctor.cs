using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Xml.Serialization;

namespace VeterinarianClinic.Domain
{
    [Table(Name = "Doctor")]
    public class Doctor : ICloneable
    {
        public object Clone()
        {
            return new Doctor()
            {
                Id = this.Id, 
                Email = this.Email,
                Name = this.Name,
                PhoneNumber = this.PhoneNumber
            };
        }

        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public string Email { get; set; }

        [Column]
        public string PhoneNumber { get; set; }

        [XmlIgnore]
        public List<Appointment> Appointments { get; set; }

        public Doctor()
        {
            Appointments = new List<Appointment>();
        }
    }
}
