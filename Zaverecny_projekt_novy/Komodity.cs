using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
namespace Zaverecny_projekt_trading_simulator
{
    internal class Komodity
    {
        public double PorizovaciCena { get; private set; }
        public string Jmeno { get; set; }
        public double Volatility { get; private set; }
        public double AktualniCena { get; set; }
        public double PocetVlastnenych { get; set; }
        public List<double> ListCen = new List<double>();
        public Komodity() { }
        public Komodity(double cenaZaKus, string jmeno, double volatility)
        {
            this.PorizovaciCena = cenaZaKus;
            this.Jmeno = jmeno;
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
