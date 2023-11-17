using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository CategoryRepo { get; private set; }
        public IProductRepository ProductRepo { get; private set; }
        public ICompanyRepository CompanyRepo { get; private set; }


        private ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            CategoryRepo = new CategoryRepository(_context);
            ProductRepo = new ProductRepository(_context);
            CompanyRepo = new CompanyRepository(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
