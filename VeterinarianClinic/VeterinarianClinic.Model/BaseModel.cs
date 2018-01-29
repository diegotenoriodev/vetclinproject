using System.Collections.Generic;
using VeterinarianClinic.Repository;

namespace VeterinarianClinic.Model
{
    public abstract class BaseModel<T> : IModel<T>
    {
        protected IRepository Repository { get => RepositorySingleton.GetRepository(); }

        public abstract ResultOperation Delete(T entity);

        public abstract List<T> GetAll(string searchCreateria);

        public abstract ResultOperation Save(T entity);
    }
}
