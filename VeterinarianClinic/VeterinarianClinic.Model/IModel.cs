using System.Collections.Generic;

namespace VeterinarianClinic.Model
{
    public class ResultOperation
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }

        public ResultOperation()
        {
            Errors = new List<string>();
        }
    }

    public interface IModel<T>
    {
        ResultOperation Save(T entity);

        ResultOperation Delete(T entity);

        List<T> GetAll(string searchCreateria);
    }
}
