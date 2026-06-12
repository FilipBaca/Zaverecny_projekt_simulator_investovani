using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace Zaverecny_projekt_trading_simulator
{
    internal class KryptoMeny
    {
        public string Jmeno { get; set; }
        public double PorizovaciCena { get; private set; }
        public double Volatility { get; set; }
        public string Ticker { get; set; }
        public double AktualniCena { get; set; }
        public double PocetVlastnenych { get; set; }
        public List<double> ListCen = new List<double>();
        public KryptoMeny()
        {

        }
        public KryptoMeny(double cenaZaKus, string jmeno, double volatility, string ticker)
        {
            this.PorizovaciCena = cenaZaKus;
            this.Jmeno = jmeno;
            this.Volatility = volatility;
            this.Ticker = ticker;
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
