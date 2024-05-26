using DataAccessLayer.Contracts;
using EntityLayer.Models;
using ServicesLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.Concrete
{
    internal class BookManager : IBookService
    {
        private readonly IRepositoryManager _repoManager;
        private readonly IBookRepository _bookRepo;
        private readonly ILoggerService _logger;
        public BookManager(IRepositoryManager repoManager, ILoggerService logger)
        {
            _repoManager = repoManager;
            _bookRepo = _repoManager.Book;
            _logger = logger;
        }

        public void CreateBook(Book book)
        {
            if (book is null)
                throw new ArgumentNullException(nameof(book));

            _bookRepo.Create(book);
            _repoManager.Save();
        }

        public void DeleteBook(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            Book? bookToDelete = _bookRepo.GetBookById(id, false);
            if (bookToDelete is null)
            {
                string message = $"Book with id: {id} wasn't found.";
                _logger.LogInfo(message);
                throw new InvalidOperationException(message);
            }
            
            _bookRepo.Delete(bookToDelete);
            _repoManager.Save();

        }

        public IEnumerable<Book> GetAllBooks(bool trackChanges)
        {
            var result = _bookRepo.GetAll(trackChanges);
            var resultCount = result.Count();
            _logger.LogInfo($"Book Count: {resultCount}");
            return result;
        }

        public Book GetBookById(int id, bool trackChanges)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            return _bookRepo.GetBookById(id, trackChanges);

        }

        public void UpdateBook(int id, Book book, bool trackChanges)
        {
            Book? bookToUpdate = _bookRepo.GetBookById(id, trackChanges);
            if (bookToUpdate is null)
                throw new InvalidOperationException($"Book with id: {id} wasn't found.");

            if (book is null)
                throw new ArgumentNullException(nameof(book));

            // mapping
            bookToUpdate.Price = book.Price;
            bookToUpdate.Title = book.Title;

            _bookRepo.Update(bookToUpdate);
            _repoManager.Save();
        }
    }
}
