using EntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.Contracts
{
    public interface IBookService
    {
        IEnumerable<Book> GetAllBooks(bool trackChanges);
        Book GetBookById(int id, bool trackChanges);
        void CreateBook(Book book);
        void UpdateBook(int id, Book book, bool trackChanges);
        void DeleteBook(int id);
    }
}
