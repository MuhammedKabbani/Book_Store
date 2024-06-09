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
    //[ApiVersion("2.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [Route("api/Book")]
    [ApiController]
    [ServiceFilter(typeof(LogFilterAttribute))]
    public class BookV2Controller : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IBookService _bookServices;

        public BookV2Controller(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
            _bookServices = _serviceManager.BookService;
        }

        [HttpHead]
        [HttpGet(Name = "GetBooksAsyncV2")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetBooksAsyncV2()
        {
            var books = await _bookServices.GetAllBooksAsync(false);

            return Ok(books);
        }
        
    }
}
