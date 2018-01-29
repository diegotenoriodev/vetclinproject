using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using VeterinarianClinic.Domain;
using System;

namespace VeterinarianClinic.Repository.LINQ
{
    public class RepositoryImplementation : IRepository
    {
        private DBDataContext DB;

        internal RepositoryImplementation(string connectionString)
        {
            DB = DBDataContext.NewInstance(connectionString);
            DataLoadOptions Load = new DataLoadOptions();

            Load.LoadWith<User>(u => u.UserType);
            Load.LoadWith<Pet>(p => p.Owner);
            Load.LoadWith<Pet>(p => p.Specie);
            Load.LoadWith<Address>(a => a.Client);
            Load.LoadWith<Appointment>(a => a.Pet);
            Load.LoadWith<Appointment>(a => a.Address);
            Load.LoadWith<Appointment>(a => a.Doctor);
            Load.LoadWith<Appointment>(a => a.AppointmentType);

            DB.LoadOptions = Load;
        }

        public void DeleteAddress(Address address)
        {
            DB.Addresses.DeleteOnSubmit(address);
            DB.SubmitChanges();
        }

        public void DeleteAppointment(Appointment appointment)
        {
            DB.Appointments.DeleteOnSubmit(appointment);
            DB.SubmitChanges();
        }

        public void DeleteClient(Client client)
        {
            DB.Clients.DeleteOnSubmit(client);
            DB.SubmitChanges();
        }

        public void DeleteDoctor(Doctor doctor)
        {
            DB.Doctors.DeleteOnSubmit(doctor);
            DB.SubmitChanges();
        }

        public void DeletePet(Pet pet)
        {
            DB.Pets.DeleteOnSubmit(pet);
            DB.SubmitChanges();
        }

        public void DeleteUser(User user)
        {
            DB.Users.DeleteOnSubmit(user);
            DB.SubmitChanges();
        }

        public void DeleteUserType(UserType userType)
        {
            DB.UserTypes.DeleteOnSubmit(userType);
            DB.SubmitChanges();
        }

        public Address GetAddress(int idAddress)
        {
            return DB.Addresses.FirstOrDefault(r => r.Id == idAddress);
        }

        public List<Address> GetAddresses(string searchCriteria)
        {
            var query = from r in DB.Addresses
                        select r;

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                searchCriteria = searchCriteria.ToLower();

                query = from r in query
                        where r.City.ToLower().Contains(searchCriteria)
                        || r.PostalCode.ToLower().Contains(searchCriteria)
                        || r.Client.Name.ToLower().Contains(searchCriteria)
                        || r.Province.ToString().ToLower().Contains(searchCriteria)
                        select r;
            }

            return query.ToList();
        }

        public Appointment GetAppointment(int idAppointment)
        {
            return DB.Appointments.FirstOrDefault(r => r.Id == idAppointment);
        }

        public List<Appointment> GetAppointments(string searchCriteria)
        {
            var query = from r in DB.Appointments
                        select r;

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                foreach (var item in searchCriteria.Split(' '))
                {
                    string searchString = item.ToLower();
                    //With SQLMethods you translate this for SQL Like, 
                    //doing so, you reduce the ammount of rows that become objects
                    query = from r in query
                            where r.Pet.Name.ToLower().Contains(searchString)
                            || r.Pet.Owner.Name.ToLower().Contains(searchString)
                            || r.Doctor.Name.ToLower().Contains(searchString)
                            select r;
                }
            }

            return query.ToList();
        }

        public Client GetClient(int idClient)
        {
            return DB.Clients.FirstOrDefault(r => r.Id == idClient);
        }

        public Client GetClient(string sin)
        {
            return DB.Clients.FirstOrDefault(r => r.SIN == sin);
        }

        public List<Client> GetClients(string searchCriteria)
        {
            var query = from r in DB.Clients
                        select r;

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                string searchString = searchCriteria.ToLower();
                //With SQLMethods you translate this for SQL Like,
                //doing so, you reduce the ammount of rows that become objects
                query = from r in query
                        where r.Name.ToLower().Contains(searchString)
                        || r.PhoneNumber.ToLower().Contains(searchString)
                        || r.SIN.ToLower().Contains(searchString)
                        select r;
            }

            return query.ToList();
        }

        public Doctor GetDoctor(int idDoctor)
        {
            return DB.Doctors.FirstOrDefault(r => r.Id == idDoctor);
        }

        public List<Doctor> GetDoctors(string searchCriteria)
        {
            var query = from r in DB.Doctors
                        select r;

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                string searchString = searchCriteria.ToLower();
                //With SQLMethods you translate this for SQL Like,
                //doing so, you reduce the ammount of rows that become objects
                query = from r in query
                        where r.Name.ToLower().Contains(searchString)
                        || r.PhoneNumber.ToLower().Contains(searchString)
                        || r.Email.ToLower().Contains(searchString)
                        select r;
            }

            return query.ToList();
        }

        public Pet GetPet(int idPet)
        {
            return DB.Pets.FirstOrDefault(r => r.Id == idPet);
        }

        public List<Pet> GetPets(int ownerId)
        {
            var query = from r in DB.Pets
                        where r.Owner.Id == ownerId
                        select r;

            return query.ToList();
        }

        public List<Pet> GetPets(string searchCriteria)
        {
            var query = from r in DB.Pets
                        select r;

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                string searchString = searchCriteria.ToLower();
                //With SQLMethods you translate this for SQL Like,
                //doing so, you reduce the ammount of rows that become objects
                query = from r in query
                        where r.Name.ToLower().Contains(searchString)
                        || r.Specie.Name.ToLower().Contains(searchString)
                        || r.Race.ToLower().Contains(searchString)
                        select r;
            }

            return query.ToList();
        }



        public User GetUser(int idUser)
        {
            return DB.Users.FirstOrDefault(r => r.Id == idUser);
        }

        public User GetUser(string username)
        {
            return DB.Users.FirstOrDefault(r => r.Username == username);
        }

        public List<User> GetUsers(string searchCriteria)
        {
            var query = from r in DB.Users
                        select r;

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                string searchString = searchCriteria.ToLower();
                //With SQLMethods you translate this for SQL Like,
                //doing so, you reduce the ammount of rows that become objects
                query = from r in query
                        where r.Name.ToLower().Contains(searchString)
                        || r.UserType.Name.ToLower().Contains(searchString)
                        || r.Username.ToLower().Contains(searchString)
                        select r;
            }

            return query.ToList();
        }
        public List<Specie> GetSpecie()
        {
            return DB.Species.ToList();
        }

        public List<UserType> GetUsersType()
        {
            return DB.UserTypes.ToList();
        }

        public UserType GetUserType(int id)
        {
            return DB.UserTypes.FirstOrDefault(r => r.Id == id);
        }

        public UserType GetUserType(string name)
        {
            return DB.UserTypes.FirstOrDefault(r => r.Name == name);
        }

        public void SaveAddress(Address address)
        {
            if (address.Id != 0)
            {
                Address old = GetAddress(address.Id);

                old.Apartment = address.Apartment;
                old.City = address.City;
                old.Line1 = address.Line1;
                old.PostalCode = address.PostalCode;
                old.Province = address.Province;
            }
            else
            {
                DB.Addresses.InsertOnSubmit(address);
            }

            DB.SubmitChanges();
        }

        public void SaveAppointment(Appointment appointment)
        {
            if (appointment.Id != 0)
            {
                Appointment old = GetAppointment(appointment.Id);

                old.DateTimeOfAppointment = appointment.DateTimeOfAppointment;
                old.Doctor = appointment.Doctor;
                old.Pet = appointment.Pet;
                old.AppointmentType = appointment.AppointmentType;
            }
            else
            {
                DB.Appointments.InsertOnSubmit(appointment);
            }

            DB.SubmitChanges();
        }

        public void SaveClient(Client client)
        {
            if (client.Id != 0)
            {
                Client old = GetClient(client.Id);

                old.Name = client.Name;
                old.PhoneNumber = client.PhoneNumber;
                old.SIN = client.SIN;
            }
            else
            {
                DB.Clients.InsertOnSubmit(client);
            }

            DB.SubmitChanges();
        }

        public void SaveDoctor(Doctor doctor)
        {
            if (doctor.Id != 0)
            {
                Doctor old = GetDoctor(doctor.Id);

                old.Email = doctor.Email;
                old.Name = doctor.Name;
                old.PhoneNumber = doctor.PhoneNumber;
            }
            else
            {
                DB.Doctors.InsertOnSubmit(doctor);
            }

            DB.SubmitChanges();
        }

        public void SavePet(Pet pet)
        {
            if (pet.Id != 0)
            {
                Pet old = GetPet(pet.Id);

                old.BirthDay = pet.BirthDay;
                old.Name = pet.Name;
                old.Owner = pet.Owner;
                old.Race = pet.Race;
                old.Specie = pet.Specie;
                old.Weight = pet.Weight;
            }
            else
            {
                DB.Pets.InsertOnSubmit(pet);
            }

            DB.SubmitChanges();
        }

        public void SaveUser(User user)
        {
            if (user.Id != 0)
            {
                User old = GetUser(user.Id);

                old.Name = user.Name;
                old.Username = user.Username;
                old.UserType = user.UserType;

                if (!string.IsNullOrEmpty(user.Password))
                {
                    old.Password = user.Password;
                }
            }
            else
            {
                DB.Users.InsertOnSubmit(user);
            }

            DB.SubmitChanges();
        }

        public void SaveUserType(UserType userType)
        {
            if (userType.Id != 0)
            {
                UserType old = GetUserType(userType.Id);

                old.Name = userType.Name;
            }
            else
            {
                DB.UserTypes.InsertOnSubmit(userType);
            }

            DB.SubmitChanges();
        }

        public List<Address> GetAddresses(int clientId)
        {
            var query = from r in DB.Addresses
                        where r.IdClient == clientId
                        select r;

            return query.ToList();
        }


        public List<AppointmentType> GetAppointmentTypes()
        {
            return DB.AppointmentTypes.ToList();
        }

        public AppointmentType GetAppointmentType(int id)
        {
            return DB.AppointmentTypes.FirstOrDefault(r => r.Id == id);
        }

        public AppointmentType GetAppointmentType(string name)
        {
            return DB.AppointmentTypes.FirstOrDefault(r => r.Name == name);
        }

        public void SaveAppointmentType(AppointmentType appointmentType)
        {
            if (appointmentType.Id != 0)
            {
                AppointmentType old = GetAppointmentType(appointmentType.Id);

                old.Name = appointmentType.Name;
            }
            else
            {
                DB.AppointmentTypes.InsertOnSubmit(appointmentType);
            }

            DB.SubmitChanges();
        }

        public void DeleteAppointmentType(AppointmentType appointmentType)
        {
            DB.AppointmentTypes.DeleteOnSubmit(appointmentType);
            DB.SubmitChanges();
        }

        public List<Appointment> GetAppointments(int appointmentId, Doctor doctor, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
