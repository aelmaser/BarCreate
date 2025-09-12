using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCreate.Models
{
    public class Barkod
    {
        public string BarkodNo { get; set; } = null!;
        public string StokNo { get; set; } = null!;
        public decimal KasaIciMiktar { get; set; }
        public decimal EksiltmeMiktar { get; set; }

        public StokKartBilgi Stok { get; set; } = null!;
    }
}
