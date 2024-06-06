using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.LinkModels
{
    public class LinkResourceBase
    {
        public LinkResourceBase()
        {
            Links = new List<Link>();
        }
        public List<Link> Links { get; set; }
    }
}
