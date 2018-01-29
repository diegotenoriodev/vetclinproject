using System;
using System.Data.Linq.Mapping;
using System.Xml.Serialization;

namespace VeterinarianClinic.Domain
{
    [Table(Name = "Appointment")]
    public class Appointment : ICloneable
    {
        public object Clone()
        {
            return new Appointment()
            {
                Address = this.Address,
                AppointmentType = this.AppointmentType,
                Client = this.Client,
                DateTimeOfAppointment = this.DateTimeOfAppointment,
                Doctor = this.Doctor,
                Id = this.Id,
                IdAddress = this.IdAddress,
                IdAppointmentType = this.IdAppointmentType,
                IdDoctor = this.IdDoctor,
                IdPet = this.IdPet,
                Pet = this.Pet
            };
        }

        public Appointment()
        {
            DateTimeOfAppointment = DateTime.Now.Date.AddDays(1);
        }

        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        private DateTime dateTimeAppointment;

        [Column]
        public DateTime DateTimeOfAppointment { get => dateTimeAppointment; set => dateTimeAppointment = value; }

        [Column]
        public int IdPet { get; set; }

        [Column]
        public int IdDoctor { get; set; }

        [Column(CanBeNull = true)]
        public int? IdAddress { get; set; }

        [Column]
        public int IdAppointmentType { get; set; }

        private Client client;
        public Client Client
        {
            get
            {
                if (pet != null) { return pet.Owner; }

                return client;
            }
            set { client = value; }
        }

        public string DateTimeAppointmentStr
        {
            get
            {
                return dateTimeAppointment.ToString("MM/dd/yyyy HH:mm");
            }
        }

        [XmlIgnore]
        public int? Time
        {
            get
            {
                if (DateTimeOfAppointment != null && DateTimeOfAppointment.Hour != 0)
                {
                    return DateTimeOfAppointment.Hour;
                }

                return null;
            }
            set
            {
                if (DateTimeOfAppointment != null && value.HasValue)
                {
                    dateTimeAppointment = DateTimeOfAppointment.Date.AddHours(value.Value);
                }
            }
        }
        private Address address;
        private Pet pet;
        private Doctor doctor;
        private AppointmentType appointmentType;

        [Association(IsForeignKey = true, ThisKey = "IdAddress")]
        [XmlIgnore]
        public Address Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
                if (address != null)
                {
                    IdAddress = address.Id;
                }
            }
        }

        [Association(IsForeignKey = true, ThisKey = "IdPet")]
        [XmlIgnore]
        public Pet Pet
        {
            get
            {
                return pet;
            }
            set
            {
                pet = value;
                if (pet != null)
                {
                    IdPet = pet.Id;
                }
            }
        }

        [Association(IsForeignKey = true, ThisKey = "IdDoctor")]
        [XmlIgnore]
        public Doctor Doctor
        {
            get { return doctor; }
            set
            {
                doctor = value;
                if (doctor != null) { IdDoctor = doctor.Id; }
            }
        }

        [Association(IsForeignKey = true, ThisKey = "IdAppointmentType")]
        [XmlIgnore]
        public AppointmentType AppointmentType
        {
            get
            {
                return appointmentType;
            }
            set
            {
                appointmentType = value;
                if (appointmentType != null)
                {
                    IdAppointmentType = appointmentType.Id;
                }
            }
        }
    }
}