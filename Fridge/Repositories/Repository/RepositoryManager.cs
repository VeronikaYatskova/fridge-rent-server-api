using Fridge.Repository.Interfaces;
using Fridge.Repository.Repositories;
using Repositories.Repository.Interfaces;
using Repositories.Repository.Repositories;

namespace Fridge.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;
        private IFridgeRepository _fridgeRepository;
        private IProductRepository _productRepository;
        private IFridgeProductRepository _fridgeProductRepository;
        private IPictureRepository _pictureRepository;
        private IUserRepository _userRepository;
        private IUserFridgeRepository _userFridgeRepository;
        private IOwnerRepository _ownerRepository;
        private IRentDocumentRepository _rentDocumentRepository;
        private IModelRepository _modelRepository;
        private IProducerRepository _producerRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IFridgeRepository Fridge
        {
            get
            {
                _fridgeRepository ??= new FridgeRepository(_repositoryContext);
                
                return _fridgeRepository; 
            }
        }

        public IProductRepository Product
        {
            get
            {
                _productRepository ??= new ProductRepository(_repositoryContext);

                return _productRepository;
            }
        }

        public IFridgeProductRepository FridgeProduct
        {
            get
            {
                _fridgeProductRepository ??= new FridgeProductRepository(_repositoryContext);

                return _fridgeProductRepository;
            }
        }

        public IPictureRepository Picture
        {
            get
            {
                _pictureRepository ??= new PictureRepository(_repositoryContext);

                return _pictureRepository;
            }
        }

        public IUserRepository User
        {
            get
            {
                _userRepository ??= new UserRepository(_repositoryContext);

                return _userRepository;
            }
        }

        public IUserFridgeRepository UserFridge
        {
            get
            {
                _userFridgeRepository ??= new UserFridgeRepository(_repositoryContext);
                
                return _userFridgeRepository;
            }
        }

        public IOwnerRepository Owner
        {
            get
            {
                _ownerRepository ??= new OwnerRepository(_repositoryContext);

                return _ownerRepository;
            }
        }

        public IRentDocumentRepository RentDocument
        {
            get
            {
                _rentDocumentRepository ??= new RentDocumentRepository(_repositoryContext);

                return _rentDocumentRepository;
            }
        }

        public IModelRepository Model
        {
            get
            {
                _modelRepository ??= new ModelRepository(_repositoryContext);

                return _modelRepository;
            }
        }

        public IProducerRepository Producer
        { 
            get
            {
                _producerRepository ??= new ProducerRepository(_repositoryContext);

                return _producerRepository;
            } 
        }

        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
    }
}
