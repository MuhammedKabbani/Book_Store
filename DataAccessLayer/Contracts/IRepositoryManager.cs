﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracts
{
    public interface IRepositoryManager
    {
        IBookRepository Book { get; }
        void Save();
        Task SaveAsync();
    }
}
