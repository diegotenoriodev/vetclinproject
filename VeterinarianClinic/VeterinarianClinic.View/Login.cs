using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinarianClinic.Domain;

namespace VeterinarianClinic.View
{
    public class Login
    {
        public static Login UserLogin { get; private set; }

        public bool IsAdmin { get; set; }

        public string Name { get; set; }

        public static void CreateLogin(User user)
        {
            UserLogin = new Login()
            {
                IsAdmin = user.IdUserType == 1,
                Name = user.Name
            };
        }
    }
}
