using EntityLayer.DTOs;
using EntityLayer.LinkModels;
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
        (LinkResponse linkResponse, MetaData metaData) GetAllBooks(DTOLinkParameters bookParameters, bool trackChanges);
        Task<(LinkResponse linkResponse, MetaData metaData)> GetAllBooksAsync(DTOLinkParameters bookParameters, bool trackChanges);
        Task<IEnumerable<DTOBook>> GetAllBooksAsync(bool trackChanges);
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
