using Fridge.Data.Models;


namespace Fridge.Data.Repositories.Interfaces
{
    public interface IProducerRepository
    {
        Task<IEnumerable<Producer>> GetAllProducers();
    }
}
