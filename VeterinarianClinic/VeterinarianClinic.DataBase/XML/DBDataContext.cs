using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using VeterinarianClinic.Domain;

namespace VeterinarianClinic.Repository.XML
{
    [Database(Name = "VeterinarianClinicDB")]
    [XmlRoot(ElementName = "VeterinarianClinicDB")]
    public class DBDataContext
    {
        private string fileName;

        public DBDataContext() { }

        public DBDataContext(string fileName)
        {
            this.fileName = fileName;

            AppointmentTypes = new List<AppointmentType>();
            Species = new List<Specie>();
            Addresses = new List<Address>();
            UserTypes = new List<UserType>();
            Users = new List<User>();
            Pets = new List<Pet>();
            Clients = new List<Client>();
            Appointments = new List<Appointment>();
            Doctors = new List<Doctor>();
        }

        public List<Specie> Species { get; set; }

        public List<Address> Addresses { get; set; }

        public List<UserType> UserTypes { get; set; }

        public List<User> Users { get; set; }

        public List<Pet> Pets { get; set; }

        public List<Client> Clients { get; set; }

        public List<Appointment> Appointments { get; set; }

        public List<AppointmentType> AppointmentTypes { get; set; }

        public List<Doctor> Doctors { get; set; }

        public void SubmitChanges()
        {
            XmlSerializer serializer = null;
            TextWriter writer = null;

            try
            {
                //Creating a backup in case it fails;
                if (File.Exists($@"{Environment.CurrentDirectory}\bkp{fileName}"))
                {
                    File.Copy($@"{Environment.CurrentDirectory}\{fileName}",
                        $@"{Environment.CurrentDirectory}\bkp{fileName}", true);
                }

                serializer = new XmlSerializer(this.GetType());
                writer = new StreamWriter($@"{Environment.CurrentDirectory}\{fileName}");

                serializer.Serialize(writer, this);
            }
            catch (Exception ex)
            {
                //Restoring the backup
                if (File.Exists($@"{Environment.CurrentDirectory}\bkp{fileName}"))
                {
                    File.Copy($@"{Environment.CurrentDirectory}\bkp{fileName}",
                    $@"{Environment.CurrentDirectory}\{fileName}", true);
                }
                    
                throw ex;
            }
            finally
            {
                if (writer != null) { writer.Close(); }
                if (serializer != null) { GC.SuppressFinalize(serializer); }
            }
        }

        protected void LoadFile()
        {
            if (File.Exists($@"{Environment.CurrentDirectory}\{fileName}"))
            {
                XmlSerializer serializer = null;
                TextReader reader = null;

                try
                {
                    serializer = new XmlSerializer(this.GetType());
                    reader = new StreamReader(fileName);

                    DBDataContext db =
                        (DBDataContext)serializer.Deserialize(reader);

                    this.Species = db.Species;
                    this.Addresses = db.Addresses;
                    this.UserTypes = db.UserTypes;
                    this.Users = db.Users;
                    this.Pets = db.Pets;
                    this.Clients = db.Clients;
                    this.Appointments = db.Appointments;
                    this.Doctors = db.Doctors;
                    this.AppointmentTypes = db.AppointmentTypes;

                    Addresses.ForEach(item =>
                                item.Client = Clients.FirstOrDefault(r => r.Id == item.IdClient));

                    Users.ForEach(item =>
                                item.UserType = UserTypes.FirstOrDefault(r => r.Id == item.IdUserType));

                    Pets.ForEach(item =>
                    {
                        item.Specie = Species.FirstOrDefault(r => r.Id == item.IdSpecie);
                        item.Owner = Clients.FirstOrDefault(r => r.Id == item.IdOwner);
                    });

                    Clients.ForEach(item =>
                    {
                        item.Pets = Pets.Where(r => r.IdOwner == item.Id).ToList();
                        item.Addresses = Addresses.Where(r => r.IdClient == item.Id).ToList();
                    });

                    Appointments.ForEach(item =>
                    {
                        item.Pet = Pets.FirstOrDefault(r => r.Id == item.IdPet);
                        item.Address = Addresses.FirstOrDefault(r => r.Id == item.IdAddress );
                        item.Doctor = Doctors.FirstOrDefault(r => r.Id == item.IdDoctor);
                        item.Client = Clients.FirstOrDefault(r => r.Id == item.Pet.IdOwner);
                        item.AppointmentType = AppointmentTypes.FirstOrDefault(r => r.Id == item.IdAppointmentType);
                    });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (reader != null) { reader.Close(); }
                    if (serializer != null) { GC.SuppressFinalize(serializer); }
                }
            }
        }

        public static DBDataContext NewInstance(string fileName)
        {
            DBDataContext db = new DBDataContext(fileName);

            if (!File.Exists($@"{Environment.CurrentDirectory}\{fileName}"))
            {
                //Seeding the database with the basic user types
                db.UserTypes.Add(new UserType() { Id = 1, Name = "Admin" });
                db.UserTypes.Add(new UserType() { Id = 2, Name = "Staff" });

                //Seeding the table specie
                db.Species.Add(new Specie() { Id = 1, Name = "Dog" });
                db.Species.Add(new Specie() { Id = 2, Name = "Cat" });
                db.Species.Add(new Specie() { Id = 3, Name = "Fish" });
                db.Species.Add(new Specie() { Id = 4, Name = "Horse" });

                db.Users.Add(new User() { Id = 1, Name = "Admin", Password = "svkLRj9nYEgZo7gWDJD5IQ==", Username = "Admin", IdUserType = 1 });

                db.Addresses.Add(new Address() { Id = -1, Line1 = "Clinic - 100 Fairway", Apartment= "23", City= "Kitchener", Province = Province.ON, PostalCode = "N2C 1X4" });
                db.AppointmentTypes.Add(new AppointmentType() { Id = 1, Name= "Vaccine" });
                db.AppointmentTypes.Add(new AppointmentType() { Id = 2, Name = "Surgery" });
                db.AppointmentTypes.Add(new AppointmentType() { Id = 1, Name = "Therapeutic" });
                db.SubmitChanges();
            }
            else
            {
                db.LoadFile();
            }

            return db;
        }
    }
}
