using EntityLayer.DTOs;
using EntityLayer.Exceptions;
using EntityLayer.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ServicesLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
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
        public IActionResult GetBookById([FromRoute(Name = "id")] int id)
        {
            if (id <= 0)
                return BadRequest();

            return Ok(_bookServices.GetBookById(id, false));
            
        }
        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            if (book == null)
                return BadRequest();


            _bookServices.CreateBook(book);

            return StatusCode(201, book); 
            
        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id,
    [FromBody] DTOBookUpdate book)
        {
            // check id
            if (book is null)
                return BadRequest(); // 400

            _bookServices.UpdateBook(id, book, false);

            return Ok(book);
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            _bookServices.DeleteBook(id);

            return NoContent();
        }
        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            // check entity
            var book = _bookServices.GetBookById(id, true);

            bookPatch.ApplyTo(book);
            _bookServices.UpdateBook(id, new DTOBookUpdate() { Id = book.Id,Title = book.Title,Price = book.Price}, true);

            return NoContent(); // 204
        }
    }
}
