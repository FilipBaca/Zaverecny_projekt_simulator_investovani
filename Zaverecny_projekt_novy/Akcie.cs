using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
namespace Zaverecny_projekt_trading_simulator
{

    internal class Akcie
    {
        public string Ticker { get; set; }
        public string Jmeno { get; set; }
        public double PorizovaciCena { get; private set; }
        public Sektor Oblast { get; set; }
        public double AktualniCena { get; set; }
        public double Volatility { get; set; }
        public double PocetVlastnenych { get; set; }
        public List<double> ListCen = new List<double>();
        public Akcie() { }
        public Akcie(string jmeno, double cenaZaKus, Sektor oblast, string ticker, double volatility)
        {
            this.Jmeno = jmeno;
            this.PorizovaciCena = cenaZaKus;
            this.Oblast = oblast;
            this.Ticker = ticker;
            this.Volatility = volatility;
            this.AktualniCena = cenaZaKus;
            this.PocetVlastnenych = 0;
            this.ListCen.Add(cenaZaKus);
        }
        public void ZapisCen()
        {
            ListCen.Add(AktualniCena);
        }
    }
}
