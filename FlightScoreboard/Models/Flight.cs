using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightScoreboard.Models
{
    public class Flight
    {
        public Bitmap MiniImage { get; set;}
        public string Company { get; set; }
        public Int64 Number { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Schedule { get; set; }
        public string Settlement { get; set; }
        public string Sector { get; set; }
        public string Status { get; set; }
        public Bitmap Image { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Data { get; set; }
        public string City { get; set; }
        public string Way { get; set; }
    }
}
