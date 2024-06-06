using EntityLayer.DTOs;
using EntityLayer.LinkModels;
using EntityLayer.Models;
using ServicesLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Net.Http;
namespace ServicesLayer.Concrete
{
    public class BookLinks : IBookLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<DTOBook> _shaper;

        public BookLinks(LinkGenerator linkGenerator, IDataShaper<DTOBook> shaper)
        {
            _linkGenerator = linkGenerator;
            _shaper = shaper;
        }

        public LinkResponse TryGenerateLinks(IEnumerable<DTOBook> booksDto, string fields, HttpContext httpContext)
        {
            var shapedBooks = ShapeData(booksDto, fields);

            if (ShouldGenerateLinks(httpContext))
                return GetLinkedBooks(booksDto, fields, httpContext, shapedBooks);
            return GetShapedBooks(shapedBooks);
        }

        private LinkResponse GetLinkedBooks(IEnumerable<DTOBook> booksDto, string fields, HttpContext httpContext, List<Entity> shapedBooks)
        {
            var bookDtoList = booksDto.ToList();

            for (int i = 0; i < booksDto.Count(); i++)
            {
                List<Link> bookLinks = CreateForBook(httpContext, bookDtoList[i], fields);
                shapedBooks[i].Add("Links", bookLinks);
            }

            var bookCollectionWrapper = CreateForBooks(httpContext, shapedBooks);

            return new LinkResponse() { HasLinks = true, LinkedEntites = bookCollectionWrapper };
        }
        private LinkCollectionWrapper<Entity> CreateForBooks(HttpContext httpContext, List<Entity> shapedBooks)
        {
            var bookCollectionWrapper = new LinkCollectionWrapper<Entity>(shapedBooks);
            bookCollectionWrapper.Links.AddRange(new List<Link>()
            {
                new Link()
                {
                    Href = $"/api/{httpContext.GetRouteData().Values["controller"]?.ToString()?.ToLower()}",
                    Rel = "self",
                    Method = "GET"
                },
            });
            return bookCollectionWrapper;
        }
        private List<Link> CreateForBook(HttpContext httpContext, DTOBook dTOBook, string fields)
        {
            var links = new List<Link>() 
            { 
                new Link ()
                {
                    Href = $"/api/{httpContext.GetRouteData().Values["controller"]?.ToString()?.ToLower()}/{dTOBook.Id}",
                    Rel = "self",
                    Method = "GET"
                },
                new Link ()
                {
                    Href = $"/api/{httpContext.GetRouteData().Values["controller"]?.ToString()?.ToLower()}",
                    Rel = "self",
                    Method = "GET"
                },
                new Link()
                {
                    Href = $"/api/{httpContext.GetRouteData().Values["controller"]?.ToString()?.ToLower()}",
                    Rel = "create",
                    Method = "Post"
                },

            };
            return links;
        }

        private LinkResponse GetShapedBooks(List<Entity> shapedBooks)
        {
            return new LinkResponse() { SahpedEntities = shapedBooks };
        }

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType.
                SubTypeWithoutSuffix
                .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        private List<Entity> ShapeData(IEnumerable<DTOBook> booksDto, string fields)
        {
            return _shaper.ShapeData(booksDto, fields).Select(x => x.Entity).ToList();
        }
    }
}
