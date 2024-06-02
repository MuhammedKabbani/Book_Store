using EntityLayer.DTOs;
using EntityLayer.Models;
using EntityLayer.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.Contracts
{
    public interface IBookService
    {
        (IEnumerable<ExpandoObject>,MetaData) GetAllBooks(BookRequestParameters bookParameters, bool trackChanges);
        Task<(IEnumerable<ExpandoObject>dTOBooks,MetaData metaData)> GetAllBooksAsync(BookRequestParameters bookParameters, bool trackChanges);
        Book GetBookById(int id, bool trackChanges);
        Task<Book> GetBookByIdAsync(int id, bool trackChanges);
        void CreateBook(Book book);
        Task CreateBookAsync(Book book);
        void UpdateBook(int id, DTOBookUpdate bookDto, bool trackChanges);
        Task UpdateBookAsync(int id, DTOBookUpdate bookDto, bool trackChanges);
        void DeleteBook(int id);
        Task DeleteBookAsync(int id);
    }
}
