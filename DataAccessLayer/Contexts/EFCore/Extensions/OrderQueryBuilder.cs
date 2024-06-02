using EntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contexts.EFCore.Extensions
{
    public static class OrderQueryBuilder
    {
        public static string CreateOrderQuery<T>(string orderByQueryString)
        {
            var orderParams = orderByQueryString.Trim().Split(',');

            var propertyInfo = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            StringBuilder orderQueryBuilder = new();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                string orderProp = param.Split(' ')[0];

                var propToSort = propertyInfo.FirstOrDefault(prop => prop.Name.Equals(orderProp, StringComparison.InvariantCultureIgnoreCase));

                if (propToSort is null)
                    continue;

                string direction = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{propToSort.Name} {direction}");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            return orderQuery;
        }
    }
}
