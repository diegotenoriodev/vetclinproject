using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using VeterinarianClinic.Domain;

namespace VeterinarianClinic.Model
{
    public class UserModel : BaseModel<User>
    {
        public override ResultOperation Delete(User entity)
        {
            ResultOperation result = new ResultOperation();

            if (entity.Id == 1)
            {
                result.Success = false;
                result.Errors.Add("You cannot remove the Admin!");
            }
            else
            {
                try
                {
                    Repository.DeleteUser(entity);
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

        public override List<User> GetAll(string searchCreateria)
        {
            return Repository.GetUsers(searchCreateria);
        }

        public override ResultOperation Save(User entity)
        {
            ResultOperation result = new ResultOperation();

            try
            {
                if (entity.ChangePassword)
                {
                    entity.Password = GetPasswordHash(entity.Password);
                }

                Repository.SaveUser(entity);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors.Add(ex.Message);
            }

            return result;
        }

        public List<UserType> GetUsersType()
        {
            try
            {
                return Repository.GetUsersType();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetPasswordHash(string password)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    password = Convert.ToBase64String(ms.ToArray());
                }
            }
            return password;
        }

        /// <summary>
        /// Returns the user if the user exists and is authenticated 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public User AuthenticateUser(string username, string pwd)
        {
            User user = Repository.GetUser(username);

            if (user != null)
            {
                string pwdHash = GetPasswordHash(pwd);

                if (user.Password == pwdHash)
                {
                    return user;
                }
            }

            return null;
        }
    }
}
