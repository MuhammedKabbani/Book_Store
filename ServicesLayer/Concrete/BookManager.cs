using AutoMapper;
using DataAccessLayer.Contracts;
using EntityLayer.DTOs;
using EntityLayer.Exceptions;
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
        private readonly IMapper _mapper;
        public BookManager(IRepositoryManager repoManager, ILoggerService logger, IMapper mapper)
        {
            _repoManager = repoManager;
            _bookRepo = _repoManager.Book;
            _logger = logger;
            _mapper = mapper;
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
                throw new BookNotFoundException(id);
            
            _bookRepo.Delete(bookToDelete);
            _repoManager.Save();

        }

        public IEnumerable<DTOBook> GetAllBooks(bool trackChanges)
        {
            var result = _bookRepo.GetAll(trackChanges);
            var resultCount = result.Count();
            _logger.LogInfo($"Book Count: {resultCount}");
            return _mapper.Map<IEnumerable<DTOBook>>(result);
        }

        public Book GetBookById(int id, bool trackChanges)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            Book? book = _bookRepo.GetBookById(id, trackChanges);
            if (book is null)
                throw new BookNotFoundException(id);

            return book;

        }

        public void UpdateBook(int id, DTOBookUpdate bookdto, bool trackChanges)
        {
            Book? bookToUpdate = _bookRepo.GetBookById(id, trackChanges);
            if (bookToUpdate is null)
                throw new BookNotFoundException(id);

            if (bookdto is null)
                throw new ArgumentNullException(nameof(bookdto));

            // mapping
            bookToUpdate = _mapper.Map<Book>(bookdto);


            _bookRepo.Update(bookToUpdate);
            _repoManager.Save();
        }
    }
}
