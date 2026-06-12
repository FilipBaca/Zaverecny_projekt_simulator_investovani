using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
namespace Zaverecny_projekt_trading_simulator
{
    
    internal class Event
    {
        public string Text { get; set; }
        public Dictionary<Sektor, double> Rozsah { get; set; }
        public bool UzSeStal { get; set;  }
        public TypEventu DruhEventu { get; set; }
        
        public Event(string text, TypEventu druhEventu, Dictionary<Sektor, double> rozsah)
        {
            this.Text = text;
            this.UzSeStal = false;
            this.DruhEventu = druhEventu;
            this.Rozsah = rozsah;
        }
    }
}
