
using System.Threading.Tasks;

namespace ModelGraph.Core
{
    public interface IRepository
    {
        string Name { get; }
        string FullName { get; }
        bool HasNoStorage { get; }

        void New(Chef chef);
        Task<bool> OpenAsync(Chef chef);
        Task<bool> SaveAsync(Chef chef);
        Task<bool> ReloadAsync(Chef chef);
        void SaveAS(Chef chef);
    }
}
