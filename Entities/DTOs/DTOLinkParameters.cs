using EntityLayer.RequestFeatures;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public record DTOLinkParameters
    {
        public BookRequestParameters BookRequestParameters { get; set; }
        public HttpContext HttpContext { get; set; }

    }
}
