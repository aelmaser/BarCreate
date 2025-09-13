using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
using BarCreate.Data;

namespace BarCreate.Services
{
    public static class BarcodeService
    {
        // yyMMdd + 4 hane sıra ör. 2509120001
        public static List<string> GenerateDailySequential(int count)
        {
            var res = new List<string>(count);
            if (count <= 0) return res; 

            string prefix = DateTime.Now.ToString("yyMMdd");
            int lastSeq = 0;

            //if bloğu barkodlar her program çalıştırılma başında temizlenmeseydi en son üretilmiş barkodu takip ederek yeni barkod üretme üzerine yazılmıştır.
            var maxToday = DataStore.FindMaxToday(); 
            if (maxToday != null)
            {
                var seqPart = maxToday.BarkodNo.Substring(6);
                int.TryParse(seqPart, out lastSeq);
            }

            for (int i = 1; i <= count; i++) //kaç adet etiket oluşacaksa o kadar sayıda prefix + indis 4 haneli versiyonu şeklinde barkodu oluşturuyor.
                res.Add($"{prefix}{(lastSeq + i):D4}");

            return res;
        }
    }
}
