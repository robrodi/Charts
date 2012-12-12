using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace AdminData.Models
{
    public struct HopperCount
    {
        public DateTime Date;
        public int TotalCount;
        public int Sessions;
        public int FallbackHoppers;
        public decimal FallbackRatio;

        public override string ToString()
        {
            return string.Format("{0:yyyy-MM-ddTHH:mm:ss.0000000-08:00}\t{1}\t{2}\t{3}\t{4}", Date, TotalCount, Sessions, FallbackHoppers, FallbackRatio);
        }
    }

    public class HopperCountReader
    {
        string path;
        
        public HopperCountReader(string path)
        {
            this.path = path;
        }

        public HopperCount ReadLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) throw new ArgumentNullException("line");
            var chunks = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (chunks.Length != 5) throw new ArgumentOutOfRangeException("line", "Wrong number of entries on line.  Expected 5");

            return new HopperCount
            {
                Date = DateTime.Parse(chunks[0]),
                FallbackHoppers = int.Parse(chunks[3]),
                FallbackRatio = Decimal.Parse(chunks[4]),
                Sessions = int.Parse(chunks[2]),
                TotalCount = int.Parse(chunks[1])
            };
        }

        const int recordPeriodMinutes = 5;
        static readonly int recordsPerDay = Convert.ToInt32(TimeSpan.FromDays(1).TotalMinutes / recordPeriodMinutes);
        static readonly TimeSpan peak = TimeSpan.FromHours(20);
        static readonly double radiansPerPeriod = 2.0D * Math.PI / Convert.ToDouble(recordsPerDay);
        const int maxUsers = 200000;
        const int minUsers = 40000;
        const int range = maxUsers - minUsers;


        public static string JunkTSV(int days)
        {
            int count = recordsPerDay * days;
            
            var start = DateTime.Today.AddDays(-days).AddHours(DateTime.Now.Hour).AddMinutes((DateTime.Now.Minute / 5) * 5);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("date	total	sessions	fallback	ratio");
            var random = new Random(12345);
            var ratio = .1m;
            for (var i = 0; i < count; i++)
            {
                var offset = start - peak;
                var periodInDay = (offset.Hour * 60 + offset.Minute) / 5;
                var r = Convert.ToDouble(periodInDay) * radiansPerPeriod;
                var cosR = (Math.Cos(r) + 1D) / 2D; // map it to [0,1]
                var numUsers = Convert.ToInt32((range * cosR) + minUsers);

                var record = new HopperCount
                {
                    Date = start,
                    TotalCount =  numUsers,
                    FallbackRatio = ratio + (Decimal) (random.NextDouble() / 10D) - .05M                    
                };

                sb.AppendLine(record.ToString());
                start = start.AddMinutes(recordPeriodMinutes);
            }
            return sb.ToString();
        }
        public string RawTSV()
        {
            if (!File.Exists(path)) throw new FileNotFoundException("Invalid file specified");
            return File.ReadAllText(path);
        }

        public IEnumerable<HopperCount> GetHopperCounts()
        {
            if (!File.Exists(path)) throw new FileNotFoundException("Invalid file specified");
            return File.ReadAllLines(this.path).Skip(1).Select(l => ReadLine(l));
        }
    }
}