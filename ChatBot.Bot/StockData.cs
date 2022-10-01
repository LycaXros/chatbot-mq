using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Bot
{
    internal class StockData
    {
        public string Symbol { get; set; } = default!;
        public string Date { get; set; } = default!;
        public string Time { get; set; } = default!;
        public string Open { get; set; } = default!;
        public string High { get; set; } = default!;
        public string Low { get; set; } = default!;
        public string Close { get; set; } = default!;
        public string Volume { get; set; } = default!;
    }
}
