using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWatch.Model
{
    public partial class Epi
    {
        public string description { get; set; }
        public string id { get; set; }
    }

    public class Link
    {
        public string hoster { get; set; }
        public string part { get; set; }
        public string id { get; set; }

        public override string ToString()
        {
            return hoster;
        }
    }

    public class EpisodenInformationen
    {
        public string series { get; set; }
        public Epi epi { get; set; }
        public List<Link> links { get; set; }

        public List<Link> VLinks
        {
            get { return links.FindAll(p => p.hoster.Contains("Streamcloud") || p.hoster.Contains("Vivo")); }
        }

    }
}
