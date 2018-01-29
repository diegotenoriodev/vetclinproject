using System;
using System.Data.Linq.Mapping;
using System.Xml.Serialization;

namespace VeterinarianClinic.Domain
{
    public enum Province
    {
        ON,
        QC,
        NS,
        NB,
        MB,
        BC,
        PE,
        SK,
        AB,
        NL
    }

    [Table(Name = "Address")]
    public class Address : ICloneable
    {

        public object Clone()
        {
            return new Address()
            {
                Id = this.Id, 
                Line1 = this.Line1,
                Apartment = this.Apartment,
                City = this.City, 
                Province = this.Province,
                PostalCode = this.PostalCode,
                Client = this.Client
            };
        }

        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column]
        public string Line1 { get; set; }

        [Column]
        public string Apartment { get; set; }

        [Column]
        public string City { get; set; }

        [Column]
        public Province Province { get; set; }

        [Column]
        public string PostalCode { get; set; }

        [Column]
        public int IdClient { get; set; }

        private Client client;

        [XmlIgnore]
        public Client Client
        {
            get
            {
                return client;
            }
            set
            {
                client = value;

                if (client != null)
                {
                    IdClient = client.Id;
                }
            }
        }

        public string Name
        {
            get
            {
                return string.Format("{0} {1} - {2}", Line1, Apartment, City);
            }
        }

        public Address()
        {
            Province = Province.ON;
        }
    }
}