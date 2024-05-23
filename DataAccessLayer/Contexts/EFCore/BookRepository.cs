using DataAccessLayer.Contracts;
using EntityLayer.Models;

namespace DataAccessLayer.Contexts.EFCore
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(EFRepositoryContext context) : base(context)
        {
            
        }

        public IQueryable<Book> GetBookById(int id, bool trackChanges) => FindBy(b => b.Id.Equals(id), trackChanges);
    }
}