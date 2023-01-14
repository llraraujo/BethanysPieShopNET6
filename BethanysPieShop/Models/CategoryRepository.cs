namespace BethanysPieShop.Models
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly BethanysPieShopDbContext _bethanysPieShopDbcontext;

        public CategoryRepository(BethanysPieShopDbContext bethanysPieShopDbcontex)
        {
            _bethanysPieShopDbcontext = bethanysPieShopDbcontex;
        }

        public IEnumerable<Category> AllCategories
        {
            get
            {
                return _bethanysPieShopDbcontext.Categories.OrderBy(c => c.CategoryName);
            }
        }
    }
}
