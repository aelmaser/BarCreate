using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCreate.Models
{
    public class StokKartBilgi
    {
        public string StokNo { get; set; } = null!;
        public int KasaIciMiktar { get; set; }
        public int EksiltmeMiktar { get; set; }

        public ICollection<Barkod> Barkodlar { get; set; } = new List<Barkod>();
    }
}
