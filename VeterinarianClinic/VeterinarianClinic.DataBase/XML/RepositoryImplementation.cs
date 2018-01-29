using System.Collections.Generic;
using System.Linq;
using VeterinarianClinic.Domain;
using System;

namespace VeterinarianClinic.Repository.XML
{
    public class RepositoryImplementation : IRepository
    {
        private DBDataContext DB;

        internal RepositoryImplementation(string fileName)
        {
            DB = DBDataContext.NewInstance(fileName);
        }

        public void DeleteAddress(Address address)
        {
            address.Client.Addresses.Remove(address);
            
            DB.Addresses.Remove(address);
            DB.SubmitChanges();
        }

        public void DeleteAppointment(Appointment appointment)
        {
            appointment.Doctor.Appointments.Remove(appointment);
            DB.Appointments.Remove(appointment);
            DB.SubmitChanges();
        }

        public void DeleteClient(Client client)
        {
            DB.Clients.Remove(client);
            DB.SubmitChanges();
        }

        public void DeleteDoctor(Doctor doctor)
        {
            DB.Doctors.Remove(doctor);
            DB.SubmitChanges();
        }

        public void DeletePet(Pet pet)
        {
            pet.Owner.Pets.Remove(pet);
            DB.Pets.Remove(pet);
            DB.SubmitChanges();
        }

        public void DeleteUser(User user)
        {
            DB.Users.Remove(user);
            DB.SubmitChanges();
        }

        public void DeleteUserType(UserType userType)
        {
            DB.UserTypes.Remove(userType);
            DB.SubmitChanges();
        }

        public Address GetAddress(int idAddress)
        {
            return DB.Addresses.FirstOrDefault(r => r.Id == idAddress);
        }

        public List<Address> GetAddresses(string searchCriteria)
        {
            var query = from r in DB.Addresses
                        where r.Id != -1
                        select r;

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                searchCriteria = searchCriteria.ToLower();

                query = from r in query
                        where r.City.ToLower().Contains(searchCriteria)
                        || r.PostalCode.ToLower().Contains(searchCriteria)
                        || r.Client.Name.ToLower().Contains(searchCriteria)
                        || r.Line1.ToLower().Contains(searchCriteria)
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

                query = from r in query
                        where r.Name.ToLower().Contains(searchString)
                        || r.Specie.Name.ToLower().Contains(searchString)
                        || r.Owner.Name.ToLower().Contains(searchString)
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
            return DB.Users.FirstOrDefault(r => r.Username.ToLower() == username.ToLower());
        }

        public List<User> GetUsers(string searchCriteria)
        {
            var query = from r in DB.Users
                        select r;

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                string searchString = searchCriteria.ToLower();

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
                if (address.Client.Addresses == null)
                {
                    address.Client.Addresses = new List<Address>();
                }
                address.Client.Addresses.Add(address);

                if (DB.Addresses.Any())
                {
                    address.Id = DB.Addresses.Max(r => r.Id) + 1;
                }
                else
                {
                    address.Id = 1;
                }

                DB.Addresses.Add(address);
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
                if (appointment.Doctor.Appointments == null)
                {
                    appointment.Doctor.Appointments = new List<Appointment>();
                }
                appointment.Doctor.Appointments.Add(appointment);

                if (DB.Appointments.Any())
                {
                    appointment.Id = DB.Appointments.Max(r => r.Id) + 1;
                }
                else
                {
                    appointment.Id = 1;
                }

                DB.Appointments.Add(appointment);
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
                if (DB.Clients.Any())
                {
                    client.Id = DB.Clients.Max(r => r.Id) + 1;
                }
                else
                {
                    client.Id = 1;
                }

                DB.Clients.Add(client);
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
                if (DB.Doctors.Any())
                {
                    doctor.Id = DB.Doctors.Max(r => r.Id) + 1;
                }
                else
                {
                    doctor.Id = 1;
                }

                DB.Doctors.Add(doctor);
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
                if (pet.Owner.Pets == null)
                {
                    pet.Owner.Pets = new List<Pet>();
                }
                pet.Owner.Pets.Add(pet);

                if (DB.Pets.Any())
                {
                    pet.Id = DB.Pets.Max(r => r.Id) + 1;
                }
                else
                {
                    pet.Id = 1;
                }

                DB.Pets.Add(pet);
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
                if (DB.Users.Any())
                {
                    user.Id = DB.Users.Max(r => r.Id) + 1;
                }
                else
                {
                    user.Id = 1;
                }

                DB.Users.Add(user);
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
                if (DB.UserTypes.Any())
                {
                    userType.Id = DB.UserTypes.Max(r => r.Id) + 1;
                }
                else
                {
                    userType.Id = 1;
                }

                DB.UserTypes.Add(userType);
            }

            DB.SubmitChanges();
        }

        public List<Address> GetAddresses(int clientId)
        {
            var query = from r in DB.Addresses
                        where (r.Client != null && r.Client.Id == clientId) 
                        || r.Id == -1
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
                if (DB.AppointmentTypes.Any())
                {
                    appointmentType.Id = DB.AppointmentTypes.Max(r => r.Id) + 1;
                }
                else
                {
                    appointmentType.Id = 1;
                }

                DB.AppointmentTypes.Add(appointmentType);
            }

            DB.SubmitChanges();
        }

        public void DeleteAppointmentType(AppointmentType appointmentType)
        {
            DB.AppointmentTypes.Remove(appointmentType);
            DB.SubmitChanges();
        }

        public List<Appointment> GetAppointments(int appointmentId, Doctor doctor, DateTime date)
        {
            var query = from r in DB.Appointments
                        select r;

            if (doctor != null && date != null)
            {
                query = from r in query
                        where r.Id != appointmentId
                        && r.Doctor.Id == doctor.Id
                        && r.DateTimeOfAppointment.Date.Equals(date.Date)
                        select r;

            }

            return query.ToList();
        }
    }
}
