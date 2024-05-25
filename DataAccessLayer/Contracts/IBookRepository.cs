using EntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracts
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        Book? GetBookById(int id, bool trackChanges);
    }
}
