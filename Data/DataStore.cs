using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarCreate.Models;

namespace BarCreate.Data
{
    public static class DataStore
    {
        // Uygulama süresi boyunca veriler burada (DB yok)
        public static readonly List<StokKartBilgi> StokKartlari = new()
        {
            new StokKartBilgi { StokNo = "A", KasaIciMiktar = 50,   EksiltmeMiktar = 10  },
            new StokKartBilgi { StokNo = "B", KasaIciMiktar = 20,   EksiltmeMiktar = 5   },
            new StokKartBilgi { StokNo = "C", KasaIciMiktar = 1200, EksiltmeMiktar = 300 }
        };

        public static readonly List<Barkod> Barkodlar = new();

        // Yardımcılar
        public static void ClearBarkod() => Barkodlar.Clear();

        public static Barkod? FindMaxToday()
        {
            var prefix = System.DateTime.Now.ToString("yyMMdd");
            return Barkodlar
                .Where(b => b.BarkodNo.StartsWith(prefix))
                .OrderByDescending(b => b.BarkodNo)
                .FirstOrDefault();
        }
    }
}
