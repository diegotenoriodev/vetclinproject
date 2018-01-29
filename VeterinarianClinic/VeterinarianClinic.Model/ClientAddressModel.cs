using System;
using System.Collections.Generic;
using System.Linq;
using VeterinarianClinic.Domain;

namespace VeterinarianClinic.Model
{
    public class AddressModel : BaseModel<Address>
    {
        public override ResultOperation Delete(Address entity)
        {
            ResultOperation result = new ResultOperation();

            if(Repository.GetAppointments(string.Empty).Any(r => r.IdAddress == entity.Id))
            {
                result.Success = false;
                result.Errors.Add("There are appointments for this address. Please delete the appointments first.");
            }
            else
            {

                try
                {
                    Repository.DeleteAddress(entity);
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

        public override List<Address> GetAll(string searchCreateria)
        {
            var items = Repository.GetAddresses(searchCreateria);

            //Removing the first address from the list, the first is always the clinic address
            items.Remove(items.OrderBy(r => r.Id).FirstOrDefault());

            return items;
        }

        public override ResultOperation Save(Address entity)
        {
            ResultOperation result = new ResultOperation();
            try
            {
                Repository.SaveAddress(entity);
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
            catch(Exception)
            {
                return null;
            }
        }
    }
}
