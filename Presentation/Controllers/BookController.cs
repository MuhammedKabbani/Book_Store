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
        public async Task<IActionResult> GetBooksAsync()
        {
            return  Ok(await _bookServices.GetAllBooksAsync(false));
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookByIdAsync([FromRoute(Name = "id")] int id)
        {
            if (id <= 0)
                return BadRequest();

            return Ok(await _bookServices.GetBookByIdAsync(id, false));
            
        }
        [HttpPost]
        public async Task<IActionResult> CreateBookAsync([FromBody] Book book)
        {
            if (book == null)
                return BadRequest();


            await _bookServices.CreateBookAsync(book);

            return StatusCode(201, book); 
            
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")] int id,
    [FromBody] DTOBookUpdate book)
        {
            // check id
            if (book is null)
                return BadRequest(); // 400

            await _bookServices.UpdateBookAsync(id, book, false);

            return Ok(book);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBookAsync([FromRoute(Name = "id")] int id)
        {
            await _bookServices.DeleteBookAsync(id);

            return NoContent();
        }
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateOneBookAsync([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            // check entity
            var book = await _bookServices.GetBookByIdAsync(id, true);

            bookPatch.ApplyTo(book);
            await _bookServices.UpdateBookAsync(id, new DTOBookUpdate() { Id = book.Id,Title = book.Title,Price = book.Price}, true);

            return NoContent(); // 204
        }
    }
}
