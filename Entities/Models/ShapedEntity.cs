﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Models
{
    public class ShapedEntity
    {
        public ShapedEntity()
        {
            Entity = new Entity();
        }

        public int Id { get; set; }
        public Entity Entity { get; set; }

    }
}
