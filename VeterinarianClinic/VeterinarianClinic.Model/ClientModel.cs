using System;
using System.Collections.Generic;
using System.Linq;
using VeterinarianClinic.Domain;

namespace VeterinarianClinic.Model
{
    public class ClientModel : BaseModel<Client>
    {
        public override ResultOperation Delete(Client entity)
        {
            ResultOperation result = new ResultOperation();

            if (entity.Addresses.Any())
            {
                result.Success = false;
                result.Errors.Add("Client has addresses registered. Please delete the addresses first.");
            }
            else if (entity.Pets.Any())
            {
                result.Success = false;
                result.Errors.Add("Client has pets registered. Please delete the pets first.");
            }
            else
            {
                try
                {
                    Repository.DeleteClient(entity);
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

        public override List<Client> GetAll(string searchCreateria)
        {
            return Repository.GetClients(searchCreateria);
        }

        public override ResultOperation Save(Client entity)
        {
            ResultOperation result = new ResultOperation();
            try
            {
                Repository.SaveClient(entity);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors.Add(ex.Message);
            }
            return result;
        }
    }
}
