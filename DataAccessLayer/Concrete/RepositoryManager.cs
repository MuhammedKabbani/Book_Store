using DataAccessLayer.Contexts.EFCore;
using DataAccessLayer.Contracts;

namespace DataAccessLayer.Concrete
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly EFRepositoryContext _context;
        private readonly Lazy<IBookRepository> _bookRepositoryLazy;
        public RepositoryManager(EFRepositoryContext context)
        {
            _context = context;
            _bookRepositoryLazy = new Lazy<IBookRepository>(() => new BookRepository(_context));
        }

        // This may cause a dependency, these can be added to IoC and be injected to constructor.
        public IBookRepository Book => _bookRepositoryLazy.Value;

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
