using DataAccessLayer.Contexts.EFCore;
using DataAccessLayer.Contexts.EFCore.Extensions;
using DataAccessLayer.Contracts;
using EntityLayer.Models;
using EntityLayer.RequestFeatures;
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

        public PagedList<Book> GetAllBooks(BookRequestParameters bookParameters,bool trackChanges)
        {
            var books = GetAll(trackChanges)
                    .ToList();

            return PagedList<Book>.ToPagedList(books, bookParameters.PageNumber, bookParameters.PageSize);
        }

        public async Task<PagedList<Book>> GetAllBooksAsync(BookRequestParameters bookParameters, bool trackChanges)
        {
            var books = await GetAll(trackChanges)
                .FilterBooksPrice(bookParameters.MinPrice, bookParameters.MaxPrice)
                .Search(bookParameters.SearchTerm)
                .OrderBy(x=>x.Id)
                .ToListAsync();

            return PagedList<Book>.ToPagedList(books, bookParameters.PageNumber, bookParameters.PageSize);
        }

    }
}