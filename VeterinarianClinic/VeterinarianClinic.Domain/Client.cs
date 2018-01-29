using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Xml.Serialization;

namespace VeterinarianClinic.Domain
{
    [Table(Name = "Client")]
    public class Client : ICloneable
    {
        public object Clone()
        {
            return new Client()
            {
                Id = this.Id, 
                Name = this.Name, 
                PhoneNumber = this.PhoneNumber,
                SIN = this.SIN, 
                Addresses = this.Addresses, 
                Pets = this.Pets
            };
        }

        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public string PhoneNumber { get; set; }

        [Column]
        public string SIN { get; set; }

        [XmlIgnore]
        public List<Address> Addresses { get; set; }

        [XmlIgnore]
        public List<Pet> Pets { get; set; }

        public Client()
        {
            Addresses = new List<Address>();
            Pets = new List<Pet>();
        }
    }
}