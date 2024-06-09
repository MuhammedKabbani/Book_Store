using AutoMapper;
using DataAccessLayer.Contracts;
using EntityLayer.DTOs;
using EntityLayer.Exceptions;
using EntityLayer.LinkModels;
using EntityLayer.Models;
using EntityLayer.RequestFeatures;
using ServicesLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
        private readonly IBookLinks _bookLinks;
        public BookManager(IRepositoryManager repoManager, ILoggerService logger, IMapper mapper, IBookLinks bookLinks)
        {
            _repoManager = repoManager;
            _bookRepo = _repoManager.Book;
            _logger = logger;
            _mapper = mapper;
            _bookLinks = bookLinks;
        }

        public void CreateBook(Book book)
        {
            if (book is null)
                throw new ArgumentNullException(nameof(book));

            _bookRepo.Create(book);
            _repoManager.Save();
        }
        public async Task CreateBookAsync(Book book)
        {
            if (book is null)
                throw new ArgumentNullException(nameof(book));

            _bookRepo.Create(book);
            await _repoManager.SaveAsync();
        }
        public void DeleteBook(int id)
        {
            Book bookToDelete = GetBookById(id, false);

            _bookRepo.Delete(bookToDelete);
            _repoManager.Save();

        }
        public async Task DeleteBookAsync(int id)
        {
            Book bookToDelete =await GetBookByIdAsync(id, false);

            _bookRepo.Delete(bookToDelete);
            await _repoManager.SaveAsync();
        }
        public (LinkResponse, MetaData) GetAllBooks(DTOLinkParameters bookParameters,bool trackChanges)
        {
            var result = _bookRepo.GetAllBooks(bookParameters.BookRequestParameters, trackChanges);
            var resultCount = result.Count();
            _logger.LogInfo($"Book Count: {resultCount}");
            var mappedBooks = _mapper.Map<IEnumerable<DTOBook>>(result);
            var links = _bookLinks.TryGenerateLinks(mappedBooks, bookParameters.BookRequestParameters.Fields, bookParameters.HttpContext);
            return (links, result.MetaData);
        }
        public async Task<(LinkResponse, MetaData)> GetAllBooksAsync(DTOLinkParameters bookParameters,bool trackChanges)
        {
            if (!bookParameters.BookRequestParameters.ValidPriceRange)
                throw new BookPriceOutOfRangeBadRequestException();

            var result = await _bookRepo.GetAllBooksAsync(bookParameters.BookRequestParameters,trackChanges);
            var resultCount = result.Count();
            _logger.LogInfo($"Book Count: {resultCount}");
            var mappedBooks = _mapper.Map<IEnumerable<DTOBook>>(result);
            var links = _bookLinks.TryGenerateLinks(mappedBooks, bookParameters.BookRequestParameters.Fields, bookParameters.HttpContext);
            return (links, result.MetaData);
        }
        public async Task<IEnumerable<DTOBook>> GetAllBooksAsync(bool trackChanges)
        {

            var result = await _bookRepo.GetAllBooksAsync(trackChanges);
            var resultCount = result.Count();
            var bookDto = _mapper.Map<IEnumerable<DTOBook>>(result);
            _logger.LogInfo($"Book Count: {resultCount}");
            return bookDto;
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
        public async Task<Book> GetBookByIdAsync(int id, bool trackChanges)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            Book? book = await _bookRepo.GetBookByIdAsync(id, trackChanges);
            if (book is null)
                throw new BookNotFoundException(id);

            return book;

        }

        public void UpdateBook(int id, DTOBookUpdate bookdto, bool trackChanges)
        {
            if (bookdto is null)
                throw new ArgumentNullException(nameof(bookdto));

            Book bookToUpdate = GetBookById(id, trackChanges);

            // mapping
            bookToUpdate = _mapper.Map<Book>(bookdto);


            _bookRepo.Update(bookToUpdate);
            _repoManager.Save();
        }
        public async Task UpdateBookAsync(int id, DTOBookUpdate bookdto, bool trackChanges)
        {
            if (bookdto is null)
                throw new ArgumentNullException(nameof(bookdto));

            Book? bookToUpdate = await GetBookByIdAsync(id, trackChanges);

            // mapping
            bookToUpdate = _mapper.Map<Book>(bookdto);


            _bookRepo.Update(bookToUpdate);
            await _repoManager.SaveAsync();
        }
    }
}
