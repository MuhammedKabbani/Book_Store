using DataAccessLayer.Contracts;
using EntityLayer.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicesLayer.Concrete;
using ServicesLayer.Contracts;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IBookService _bookServices;

        public BookController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
            _bookServices = _serviceManager.BookService;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            return Ok(_bookServices.GetAllBooks(false));
        }
        [HttpGet("{id:int}")]
        public IActionResult GetBookById([FromRoute(Name = "id")]int id)
        {
            if (id <= 0)
                return BadRequest();

            try
            {
                Book? book = _bookServices.GetBookById(id, false);
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
                _bookServices.CreateBook(book);

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
                var entity = _bookServices.GetBookById(id, true);

                if (entity is null)
                    return NotFound(); // 404

                // check id
                if (id != book.Id)
                    return BadRequest(); // 400

                entity.Title = book.Title;
                entity.Price = book.Price;

                _bookServices.UpdateBook(id,entity,true);

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
                var entity = _bookServices.GetBookById(id, false);


                if (entity is null)
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = $"Book with id:{id} could not found."
                    });  // 404

                _bookServices.DeleteBook(id);

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
                var entity = _bookServices.GetBookById(id, true);


                if (entity is null)
                    return NotFound(); // 404

                bookPatch.ApplyTo(entity);

                _bookServices.UpdateBook(id,entity,true);

                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
