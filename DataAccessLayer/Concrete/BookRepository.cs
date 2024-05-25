using DataAccessLayer.Contexts.EFCore;
using DataAccessLayer.Contracts;
using EntityLayer.Models;

namespace DataAccessLayer.Concrete
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(EFRepositoryContext context) : base(context)
        {

        }

        public Book? GetBookById(int id, bool trackChanges) => GetBy(b => b.Id.Equals(id), trackChanges).FirstOrDefault();
    }
}