using System;
using System.Collections.Generic;
using VeterinarianClinic.Domain;

namespace VeterinarianClinic.Model
{
    public class AppointmentModel : BaseModel<Appointment>
    {
        public override ResultOperation Delete(Appointment entity)
        {
            ResultOperation result = new ResultOperation();

            try
            {
                Repository.DeleteAppointment(entity);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors.Add(ex.Message);
            }

            return result;
        }

        public override List<Appointment> GetAll(string searchCreateria)
        {
            return Repository.GetAppointments(searchCreateria);
        }

        public override ResultOperation Save(Appointment entity)
        {
            ResultOperation result = new ResultOperation();
            try
            {
                Repository.SaveAppointment(entity);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public List<Client> GetClients()
        {
            try
            {
                return Repository.GetClients(string.Empty);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Doctor> GetDoctors()
        {
            try
            {
                return Repository.GetDoctors(string.Empty);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Pet> GetPets(int clientId)
        {
            try
            {
                return Repository.GetPets(clientId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<AppointmentType> GetAppointmentTypes()
        {
            try
            {
                return Repository.GetAppointmentTypes();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Address> GetAddresses(int clientId)
        {
            try
            {
                return Repository.GetAddresses(clientId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private List<Appointment> GetAppointments(int appointmentId, Doctor doctor, DateTime date)
        {
            try
            {
                return Repository.GetAppointments(appointmentId, doctor, date);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public  List<int> GetAvailableHours(int appointmentId, Doctor doctor, DateTime date)
        {
            List<int> hoursAvailable = new List<int>();
            List<Appointment> appointments = this.GetAppointments(appointmentId, doctor, date);
            hoursAvailable = this.getDefaultAvailableHours();

            if(appointments != null)
            {
                foreach (Appointment a in appointments)
                {
                    hoursAvailable.Remove(a.DateTimeOfAppointment.Hour);
                }
            }

           return hoursAvailable;
        }

        private List<int> getDefaultAvailableHours()
        {
            List<int> hoursAvailable = new List<int>();
            for (int i = 8; i < 18; i++)
            {
                hoursAvailable.Add(i);
            }
            return hoursAvailable;
        }
        
    }


}
