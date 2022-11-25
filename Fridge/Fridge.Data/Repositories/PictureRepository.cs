using Fridge.Data.Context;
using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;

namespace Fridge.Data.Repositories
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
