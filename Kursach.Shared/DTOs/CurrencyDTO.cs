using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach.Shared.DTOs
{
    public class CurrencyDTO
    {
        public int CurrencyCodeA { get; set; }
        public int CurrencyCodeB { get; set; }
        public long UnixTime { get; set; }
        public float RateSell { get; set; }
        public float RateBuy { get; set; }
        public float RateCross { get; set; }
    }
}
