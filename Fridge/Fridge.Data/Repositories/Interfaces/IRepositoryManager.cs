namespace Fridge.Data.Repositories.Interfaces
{
    public interface IRepositoryManager
    {
        IFridgeRepository Fridge { get; }
        IProductRepository Product { get; }
        IFridgeProductRepository FridgeProduct { get; }
        IPictureRepository Picture { get; }
        IUserRepository User { get; }
        IModelRepository Model { get; }
        IProducerRepository Producer { get; }
        Task SaveAsync();
    }
}
