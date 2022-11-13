using Fridge.Models;
using Fridge.Models.DTOs;
using Fridge.Repository.Interfaces;

namespace Fridge.Repository.Repositories
{
    public class PictureRepository : RepositoryBase<ProductPicture>, IPictureRepository
    {
        public PictureRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void AddPicture(ProductPicture picture) =>
            Create(picture);
    }
}
