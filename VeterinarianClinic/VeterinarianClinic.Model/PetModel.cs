using System;
using System.Collections.Generic;
using System.Linq;
using VeterinarianClinic.Domain;

namespace VeterinarianClinic.Model
{
    public class PetModel : BaseModel<Pet>
    {
        public override ResultOperation Delete(Pet entity)
        {
            ResultOperation result = new ResultOperation();

            if (Repository.GetAppointments(string.Empty).Any(r => r.IdPet == entity.Id))
            {
                result.Success = false;
                result.Errors.Add("There are appointments for this pet. Please remove the appointments first.");
            }
            else
            {
                try
                {
                    Repository.DeletePet(entity);
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

        public override List<Pet> GetAll(string searchCreateria)
        {
            return Repository.GetPets(searchCreateria);
        }

        public override ResultOperation Save(Pet entity)
        {
            ResultOperation result = new ResultOperation();

            try
            {
                Repository.SavePet(entity);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public List<Pet> GetPets(string searchsearchCriteria)
        {
            try
            {
                return Repository.GetPets(String.Empty);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Specie> GetSpecies()
        {
            return Repository.GetSpecie();
        }

        public List<Client> GetClients(string empty)
        {
            try
            {
                return Repository.GetClients(String.Empty);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

}
