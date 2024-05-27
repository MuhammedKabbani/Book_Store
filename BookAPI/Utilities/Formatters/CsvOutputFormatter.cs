using EntityLayer.DTOs;
using EntityLayer.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Text;

namespace BookAPI.Utilities.Formatters
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        protected override bool CanWriteType(Type? type)
        {
            if (typeof(DTOBook).IsAssignableFrom(type) || typeof(IEnumerable<DTOBook>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }
        private static void FormatCsv(StringBuilder buffer, DTOBook book)
        {
            buffer.AppendLine($"{book.Id}, {book.Title}, {book.Price}");
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var responce = context.HttpContext.Response;
            var buffer = new StringBuilder();
            if(context.Object is IEnumerable<DTOBook> bookList)
            {
                foreach (var book in bookList)
                {
                    FormatCsv(buffer, book);
                }
            }
            else
            {
                FormatCsv(buffer, (DTOBook)context.Object);
            }
            await responce.WriteAsync(buffer.ToString());
        }
    }
}
