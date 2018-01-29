using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using VeterinarianClinic.Domain;

namespace VeterinarianClinic.Repository.LINQ
{
    [Database(Name = "VeterinarianClinicDB")]
    internal class DBDataContext : DataContext
    {
        public Table<Specie> Species = null;
        public Table<Address> Addresses = null;
        public Table<UserType> UserTypes = null;
        public Table<User> Users = null;
        public Table<Pet> Pets = null;
        public Table<Client> Clients = null;
        public Table<AppointmentType> AppointmentTypes = null;
        public Table<Appointment> Appointments = null;
        public Table<Doctor> Doctors = null;

        public static DBDataContext NewInstance(string connectionString)
        {
            //This one, gives access to the tables through properties
            if(connectionString.Contains("|DataDirectory|"))
            {
                connectionString = connectionString.Replace("|DataDirectory|", Environment.CurrentDirectory);
            }
            DBDataContext db = new DBDataContext(connectionString);

            if (!db.DatabaseExists())
            {
                db.CreateDatabase();
                db.Dispose();

                //A second instance is required, so it reloads the tables that were recently created;
                db = new DBDataContext(connectionString);

                //Seeding the database with the basic user types
                db.UserTypes.InsertOnSubmit(new UserType() { Name = "Admin" });
                db.UserTypes.InsertOnSubmit(new UserType() { Name = "Staff" });

                //Seeding the table specie
                db.Species.InsertOnSubmit(new Specie() { Name = "Dog" });
                db.Species.InsertOnSubmit(new Specie() { Name = "Cat" });
                db.Species.InsertOnSubmit(new Specie() { Name = "Fish" });
                db.Species.InsertOnSubmit(new Specie() { Name = "Horse" });

                db.SubmitChanges();

                db.Users.InsertOnSubmit(new User() { Name = "Admin", Password = "svkLRj9nYEgZo7gWDJD5IQ==", Username = "Admin", UserType = db.UserTypes.FirstOrDefault() });

                db.SubmitChanges();

                db.Addresses.InsertOnSubmit(new Address() { Line1 = "Clinic - 100 Fairway", Apartment = "23", City = "Kitchener", Province = Province.ON, PostalCode = "N2C 1X4" });
                db.AppointmentTypes.InsertOnSubmit(new AppointmentType() { Name = "Vaccine" });
                db.AppointmentTypes.InsertOnSubmit(new AppointmentType() { Name = "Surgery" });
                db.AppointmentTypes.InsertOnSubmit(new AppointmentType() { Name = "Therapeutic" });

                db.SubmitChanges();

                db.Dispose();

                return new DBDataContext(connectionString);
            }

            return db;
        }

        private DBDataContext(string connectionString) : base(connectionString)
        {
        }
    }
}
