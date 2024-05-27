﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public record DTOBookUpdate 
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public decimal Price { get; set; }
    }

}