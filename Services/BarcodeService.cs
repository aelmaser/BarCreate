using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BarCreate.Data;

namespace BarCreate.Services
{
    public static class BarcodeService
    {
        /// <summary>
        /// Gün prefix'i (yyMMdd) için mevcut en yüksek sırayı alır ve istenen adet kadar
        /// artan barkod numarası üretir. Örn: 2206080001, 2206080002...
        /// </summary>
        public static async Task<List<string>> GenerateDailySequentialAsync(AppDbContext db, int count)
        {
            if (count <= 0) return new List<string>();

            string prefix = DateTime.Now.ToString("yyMMdd");

            // O gün için mevcut en büyük barkodNo'yu bul (prefix ile başlayan)
            string? maxForToday = await db.Barkodlar
                .Where(b => b.BarkodNo.StartsWith(prefix))
                .Select(b => b.BarkodNo)
                .OrderByDescending(x => x)
                .FirstOrDefaultAsync();

            int lastSeq = 0;
            if (!string.IsNullOrWhiteSpace(maxForToday))
            {
                // yyMMdd (6) + seq(4)
                var seqPart = maxForToday.Substring(6);
                if (int.TryParse(seqPart, out var seq)) lastSeq = seq;
            }

            var list = new List<string>(count);
            for (int i = 1; i <= count; i++)
            {
                int next = lastSeq + i;
                list.Add($"{prefix}{next:D4}");
            }
            return list;
        }
    }
}
