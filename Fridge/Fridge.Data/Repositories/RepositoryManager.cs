using Fridge.Data.Context;
using Repositories.Data.Repositories;

namespace Fridge.Data.Repositories.Interfaces
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext repositoryContext;
        private IFridgeRepository fridgeRepository;
        private IProductRepository productRepository;
        private IFridgeProductRepository fridgeProductRepository;
        private IPictureRepository pictureRepository;
        private IUserRepository userRepository;
        private IModelRepository modelRepository;
        private IProducerRepository producerRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public IFridgeRepository Fridge
        {
            get
            {
                fridgeRepository ??= new FridgeRepository(repositoryContext);
                
                return fridgeRepository; 
            }
        }

        public IProductRepository Product
        {
            get
            {
                productRepository ??= new ProductRepository(repositoryContext);

                return productRepository;
            }
        }

        public IFridgeProductRepository FridgeProduct
        {
            get
            {
                fridgeProductRepository ??= new FridgeProductRepository(repositoryContext);

                return fridgeProductRepository;
            }
        }

        public IPictureRepository Picture
        {
            get
            {
                pictureRepository ??= new PictureRepository(repositoryContext);

                return pictureRepository;
            }
        }

        public IUserRepository User
        {
            get
            {
                userRepository ??= new UserRepository(repositoryContext);

                return userRepository;
            }
        }

        public IModelRepository Model
        {
            get
            {
                modelRepository ??= new ModelRepository(repositoryContext);

                return modelRepository;
            }
        }

        public IProducerRepository Producer
        { 
            get
            {
                producerRepository ??= new ProducerRepository(repositoryContext);

                return producerRepository;
            } 
        }

        public Task SaveAsync() => repositoryContext.SaveChangesAsync();
    }
}
