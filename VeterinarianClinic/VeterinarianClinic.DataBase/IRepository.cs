using System;
using System.Collections.Generic;
using VeterinarianClinic.Domain;

namespace VeterinarianClinic.Repository
{
    public interface IRepository
    {
        void SaveAddress(Address address);
        Address GetAddress(int idAddress);
        List<Address> GetAddresses(string searchCriteria);
        List<Address> GetAddresses(int clientId);
        void DeleteAddress(Address address);

        void SaveAppointment(Appointment appointment);
        Appointment GetAppointment(int idAppointment);
        List<Appointment> GetAppointments(string searchCriteria);
        void DeleteAppointment(Appointment appointment);
        
        void SaveClient(Client client);
        Client GetClient(int idClient);
        Client GetClient(string sin);
        List<Client> GetClients(string searchCriteria);
        void DeleteClient(Client client);

        void SaveDoctor(Doctor doctor);
        Doctor GetDoctor(int idDoctor);
        List<Doctor> GetDoctors(string searchCriteria);
        void DeleteDoctor(Doctor doctor);

        void SavePet(Pet pet);
        Pet GetPet(int idPet);
        List<Specie> GetSpecie();
        List<Pet> GetPets(string searchCriteria);
        List<Pet> GetPets(int ownerId);
        void DeletePet(Pet pet);

        void SaveUser(User user);
        User GetUser(int idUser);
        User GetUser(string username);
        List<User> GetUsers(string searchCriteria);
        void DeleteUser(User user);

        void SaveUserType(UserType userType);
        UserType GetUserType(int id);
        UserType GetUserType(string name);
        List<UserType> GetUsersType();
        void DeleteUserType(UserType userType);

        void SaveAppointmentType(AppointmentType appointmentType);
        AppointmentType GetAppointmentType(int id);
        AppointmentType GetAppointmentType(string name);
        List<AppointmentType> GetAppointmentTypes();
        List<Appointment> GetAppointments(int appointmentId, Doctor doctor, DateTime date);
        void DeleteAppointmentType(AppointmentType appointmentType);
    }
}
