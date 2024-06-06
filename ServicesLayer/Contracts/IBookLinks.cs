using EntityLayer.DTOs;
using EntityLayer.LinkModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.Contracts
{
    public interface IBookLinks
    {
        LinkResponse TryGenerateLinks(IEnumerable<DTOBook> booksDto, string fields, HttpContext httpContext); 
    }
}
