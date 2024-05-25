using DataAccessLayer.Contracts;
using EntityLayer.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IRepositoryManager _repoManager;
        private readonly IBookRepository _bookRepo;
        public BookController(IRepositoryManager repoManager)
        {
            _repoManager = repoManager;
            _bookRepo = _repoManager.Book;
        }
        [HttpGet]
        public IActionResult GetBooks()
        {
            return Ok(_bookRepo.FindAll(false));
        }
        [HttpGet("{id:int}")]
        public IActionResult GetBookById([FromRoute(Name = "id")]int id)
        {
            if (id <= 0)
                return BadRequest();

            try
            {
                Book? book = _bookRepo.GetBookById(id, false).SingleOrDefault();
                if (book is null)
                    return NotFound();

                return Ok(book);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult CreateBook([FromBody]Book book)
        {
            if (book == null)
                return BadRequest();

            try
            {
                _bookRepo.Create(book);
                _repoManager.Save();

                return StatusCode(201, book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id,
    [FromBody] Book book)
        {
            try
            {
                // check book?
                var entity = _bookRepo.FindBy(b => b.Id.Equals(id),true)
                    .SingleOrDefault();

                if (entity is null)
                    return NotFound(); // 404

                // check id
                if (id != book.Id)
                    return BadRequest(); // 400

                entity.Title = book.Title;
                entity.Price = book.Price;

                _repoManager.Save();

                return Ok(book);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var entity = _bookRepo.FindBy(b => b.Id.Equals(id),false)
                 .SingleOrDefault();


                if (entity is null)
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = $"Book with id:{id} could not found."
                    });  // 404

                _bookRepo.Delete(entity);
                _repoManager.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            try
            {
                // check entity
                var entity = _bookRepo.FindBy(b => b.Id.Equals(id),true)
                    .SingleOrDefault();

                if (entity is null)
                    return NotFound(); // 404

                bookPatch.ApplyTo(entity);

                _repoManager.Save();

                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
