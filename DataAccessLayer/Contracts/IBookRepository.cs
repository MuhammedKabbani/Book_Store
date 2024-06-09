using EntityLayer.Models;
using EntityLayer.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracts
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        Book? GetBookById(int id, bool trackChanges);
        Task<Book?> GetBookByIdAsync(int id, bool trackChanges);
        PagedList<Book> GetAllBooks(BookRequestParameters bookParametesr, bool trackChanges);
        Task<PagedList<Book>> GetAllBooksAsync(BookRequestParameters bookParametesr,bool trackChanges);
        Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges);

    }
}
