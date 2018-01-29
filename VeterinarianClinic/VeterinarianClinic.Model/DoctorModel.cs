using System;
using System.Collections.Generic;
using System.Linq;
using VeterinarianClinic.Domain;

namespace VeterinarianClinic.Model
{
    public class DoctorModel : BaseModel<Doctor>
    {
        public override ResultOperation Delete(Doctor entity)
        {
            ResultOperation result = new ResultOperation();

            if (entity.Appointments.Any())
            {
                result.Success = false;
                result.Errors.Add("There are appointments for this doctor. Please, remove the appointments first.");
            }
            else
            {
                try
                {
                    Repository.DeleteDoctor(entity);
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Errors.Add(ex.Message);
                }
            }

            return result;
        }

        public override List<Doctor> GetAll(string searchCreateria)
        {
            return Repository.GetDoctors(searchCreateria);
        }

        public override ResultOperation Save(Doctor entity)
        {
            ResultOperation result = new ResultOperation();

            try
            {
                Repository.SaveDoctor(entity);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors.Add(ex.Message);
            }
            return result;
        }
        public List<Doctor> GetDoctors(string searchsearchCriteria)
        {
            try
            {
                return Repository.GetDoctors(String.Empty);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
