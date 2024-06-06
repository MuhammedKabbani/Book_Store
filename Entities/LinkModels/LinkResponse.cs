using EntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.LinkModels
{
    public class LinkResponse
    {
        public bool HasLinks { get; set; }
        public List<Entity> SahpedEntities{ get; set; }
        public LinkCollectionWrapper<Entity> LinkedEntites { get; set; }
        public LinkResponse() 
        {
            SahpedEntities = new List<Entity>();
            LinkedEntites = new LinkCollectionWrapper<Entity>();
        }

    }
}
