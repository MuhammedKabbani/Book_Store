using EntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace DataAccessLayer.Contexts.EFCore.Extensions
{
    public static class BookRepositoryExtension
    {
        public static IQueryable<Book> FilterBooksPrice(this IQueryable<Book> books,uint minPrice, uint maxPrice)
        {
            return books.Where(b => b.Price >= minPrice && b.Price <= maxPrice);
        }
        public static IQueryable<Book> Search(this IQueryable<Book>books,string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return books;

            var lowercaseSearch = searchTerm.Trim().ToLower();

            return books.Where(b => b.Title.ToLower().Contains(lowercaseSearch));
        }
        public static IQueryable<Book> Sort(this IQueryable<Book> books, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return books.OrderBy(x=>x.Id);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Book>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery?.ToString()))
                return books.OrderBy(x => x.Id);

            return books.OrderBy(orderQuery.ToString());
        }
    }
}
