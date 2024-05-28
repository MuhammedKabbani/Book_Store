using DataAccessLayer.Contexts.EFCore;
using DataAccessLayer.Contracts;
using EntityLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(EFRepositoryContext context) : base(context)
        {

        }

        public Book? GetBookById(int id, bool trackChanges) => GetBy(b => b.Id.Equals(id), trackChanges).FirstOrDefault();
        public async Task<Book?> GetBookByIdAsync(int id, bool trackChanges)
        {
            return await GetBy(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }

        public IEnumerable<Book> GetAllBooks(bool trackChanges)
        {
            return GetAll(trackChanges).ToList();
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges)
        {
            return await GetAll(trackChanges).OrderBy(x=>x.Id).ToListAsync();
        }

    }
}