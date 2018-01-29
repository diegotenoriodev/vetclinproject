using System;
using System.Data.Linq.Mapping;
using System.Xml.Serialization;

namespace VeterinarianClinic.Domain
{
    [Table(Name = "User")]
    public class User : ICloneable
    {
        public object Clone()
        {
            return new User()
            {
                Id = this.Id,
                Name = this.Name,
                Username = this.Username,
                Password = this.Password,
                IdUserType = this.IdUserType,
                userType = this.UserType,
                ChangePassword = this.ChangePassword
            };
        }

        [XmlIgnore]
        public bool ChangePassword { get; set; }

        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public string Username { get; set; }

        [Column]
        public string Password { get; set; }

        [Column]
        public int IdUserType { get; set; }

        private UserType userType;

        [Association(IsForeignKey = true, ThisKey = "IdUserType")]
        [XmlIgnore]
        public UserType UserType
        {
            get
            {
                return userType;
            }
            set
            {
                userType = value;

                if (userType != null)
                {
                    IdUserType = userType.Id;
                }
            }
        }

    }
}