using System.Data.Linq.Mapping;

namespace VeterinarianClinic.Domain
{
    [Table(Name = "AppointmentType")]
    public class AppointmentType
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column]
        public string Name
        {
            get; set;
        }
    }
}
