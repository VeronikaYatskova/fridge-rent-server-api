namespace Fridge.Data.Repositories.Interfaces
{
    public interface IRepositoryManager
    {
        IFridgeRepository Fridge { get; }
        IProductRepository Product { get; }
        IFridgeProductRepository FridgeProduct { get; }
        IPictureRepository Picture { get; }
        IRenterRepository Renter { get; }
        IRenterFridgeRepository RenterFridge { get; }
        IOwnerRepository Owner { get; }
        IRentDocumentRepository RentDocument { get; }
        IModelRepository Model { get; }
        IProducerRepository Producer { get; }
        Task SaveAsync();
    }
}
