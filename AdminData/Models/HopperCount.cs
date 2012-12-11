using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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