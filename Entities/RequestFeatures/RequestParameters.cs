﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.RequestFeatures
{
    public abstract class RequestParameters
    {
		const int maxPageSize = 50;
        public int PageNumber { get; set; }
		private int _pageSize;
		public int PageSize
		{
			get { return _pageSize; }
			set { _pageSize = value > maxPageSize ? maxPageSize : value; }
		}
        public string? OrderBy { get; set; }
		public string? Fields { get; set; }

    }

}
