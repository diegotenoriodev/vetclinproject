using System.Configuration;

namespace VeterinarianClinic.Repository
{
    public class RepositorySingleton
    {
        private RepositorySingleton() { }

        private static IRepository Repository;

        /// <summary>
        /// For sql connection, it requires a connection string called "VeterinarianClinic" in the
        /// configuration file
        /// </summary>
        /// <returns>Returns an implementation of IRepository</returns>
        public static IRepository GetRepository()
        {
            if (Repository == null)
            {
                Repository = new LINQ.RepositoryImplementation(ConfigurationManager.ConnectionStrings["VeterinarianClinic"].ConnectionString);
                //Repository = new XML.RepositoryImplementation("repository.xml");
            }

            return Repository;
        }
    }
}
