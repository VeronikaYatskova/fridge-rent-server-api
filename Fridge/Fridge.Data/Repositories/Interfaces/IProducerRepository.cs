using Fridge.Data.Models;


namespace Fridge.Data.Repositories.Interfaces
{
    public interface IProducerRepository
    {
        Task<Producer> GetProducerByIdAsync(Guid id);
        Task<IEnumerable<Producer>> GetAllProducers();
    }
}
