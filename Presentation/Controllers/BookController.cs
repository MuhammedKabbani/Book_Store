using EntityLayer.DTOs;
using EntityLayer.Exceptions;
using EntityLayer.Models;
using EntityLayer.RequestFeatures;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ActionFilters;
using ServicesLayer.Contracts;
using ServicesLayer.ValidationRules.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [ServiceFilter(typeof(LogFilterAttribute))]
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
        public async Task<IActionResult> GetBooksAsync([FromQuery] BookRequestParameters bookParameters)
        {
            var pagedResult = await _bookServices.GetAllBooksAsync(bookParameters, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.dTOBooks);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookByIdAsync([FromRoute(Name = "id")] int id)
        {
            return Ok(await _bookServices.GetBookByIdAsync(id, false));
        }
        [HttpPost]
        [ValidationFilter(ValidatorType = typeof(BookValidator))]
        public async Task<IActionResult> CreateBookAsync([FromBody] Book book)
        {
            await _bookServices.CreateBookAsync(book);
            return StatusCode(201, book); 
        }
        [HttpPut("{id:int}")]
        [ValidationFilter(ValidatorType = typeof(BookValidator))]
        public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")] int id,
    [FromBody] DTOBookUpdate book)
        {
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
