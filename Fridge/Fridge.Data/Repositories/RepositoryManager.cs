﻿using Fridge.Data.Context;
using Repositories.Data.Repositories;

namespace Fridge.Data.Repositories.Interfaces
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;
        private IFridgeRepository _fridgeRepository;
        private IProductRepository _productRepository;
        private IFridgeProductRepository _fridgeProductRepository;
        private IPictureRepository _pictureRepository;
        private IRenterRepository _userRepository;
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

        public IRenterRepository Renter
        {
            get
            {
                _userRepository ??= new RenterRepository(_repositoryContext);

                return _userRepository;
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
