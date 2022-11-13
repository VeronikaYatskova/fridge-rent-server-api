using Fridge.Repository.Interfaces;
namespace Repositories.Repository.Interfaces
{
    public interface IRepositoryManager
    {
        IFridgeRepository Fridge { get; }
        IProductRepository Product { get; }
        IFridgeProductRepository FridgeProduct { get; }
        IPictureRepository Picture { get; }
        IUserRepository User { get; }
        IUserFridgeRepository UserFridge { get; }
        IOwnerRepository Owner { get; }
        IRentDocumentRepository RentDocument { get; }
        IModelRepository Model { get; }
        IProducerRepository Producer { get; }
        Task SaveAsync();
    }
}
