using EntityLayer.DTOs;
using EntityLayer.Models;
using EntityLayer.RequestFeatures;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ActionFilters;
using ServicesLayer.Contracts;
using ServicesLayer.ValidationRules.FluentValidation;
using System.Text.Json;

namespace PresentationLayer.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogFilterAttribute))]
    [ResponseCache(CacheProfileName = "5mins")]
    public class BookController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IBookService _bookServices;

        public BookController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
            _bookServices = _serviceManager.BookService;
        }

        [HttpHead]
        [HttpGet(Name = "GetBooksAsync")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetBooksAsync([FromQuery] BookRequestParameters bookParameters)
        {
            var linkParamters = new DTOLinkParameters()
            {
                BookRequestParameters = bookParameters,
                HttpContext = HttpContext
            };

            var result = await _bookServices.GetAllBooksAsync(linkParamters, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));

            return result.linkResponse.HasLinks ?
                    Ok(result.linkResponse.LinkedEntites) :
                    Ok(result.linkResponse.SahpedEntities);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookByIdAsync([FromRoute(Name = "id")] int id)
        {
            return Ok(await _bookServices.GetBookByIdAsync(id, false));
        }
        [HttpPost(Name = "CreateBookAsync")]
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
            await _bookServices.UpdateBookAsync(id, new DTOBookUpdate() { Id = book.Id, Title = book.Title, Price = book.Price }, true);

            return NoContent(); // 204
        }
        [HttpOptions]
        public IActionResult GetBooksOptions()
        {
            Response.Headers.Add("Allow", "GET, PUT, POST, PATCH, DELETE, HEAD, OPTIONS");
            return Ok();
        }
    }
}
