using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
namespace Zaverecny_projekt_trading_simulator
{
    internal class Ucet
    {
        public string Jmeno { get; set; }
        public string Heslo { get; set; }
        public double Penize { get; set; }
        public List<KryptoMeny> ListKrypta { get; set; }
        public List<Akcie> ListAkcie { get; set; }
        public List<Komodity> ListKomodity { get; set; }
        public int PocetKol { get; set; }
        public Ucet(string jmeno)
        {
            this.Jmeno = jmeno;
            this.Penize = 1000;
            this.ListKrypta = new List<KryptoMeny>();
            this.ListKomodity = new List<Komodity>();
            this.ListAkcie = new List<Akcie>();
            this.PocetKol = 0;
        }
        public void VypisAtribudu()
        {
            string vypis = $"Jméno".PadRight(10) + $"| {Jmeno}Đ" +
                   $"Majetek".PadRight(10) + $"| {Penize}$Đ" +
                   $"Kryptoměny".PadRight(10) + $"| {ListKrypta.Count}Đ" +
                   $"Komodity".PadRight(10) + $"| {ListKomodity.Count}Đ" +
                   $"Akcie".PadRight(10) + $"| {ListAkcie.Count}";
            Program.VypisTabulka(vypis);



        }
        public void VypisPortfolia()
        {

            string Vypis = $"Hráč: {Jmeno}ĐPeníze: {Penize}";
            Vypis += "Kryptoměny: Đ";
            for (int i = 0; i < ListKrypta.Count(); i++)
            {
                Vypis += $"{ListKrypta[i].Jmeno}({ListKrypta[i].Ticker})({ListKrypta[i].AktualniCena:F2}) : {(ListKrypta[i].PocetVlastnenych * ListKrypta[i].AktualniCena):F2}ĐĐ";
            }
            Vypis += "Akcie: Đ";
            for (int i = 0; i < ListAkcie.Count(); i++)
            {
                Vypis += $"{ListAkcie[i].Jmeno}({ListAkcie[i].Ticker})({ListAkcie[i].AktualniCena:F2}) : {(ListAkcie[i].PocetVlastnenych * ListAkcie[i].AktualniCena):F2}ĐĐ";
            }
            Vypis += "Komodity: Đ";
            for (int i = 0; i < ListKomodity.Count(); i++)
            {
                Vypis += $"{ListKomodity[i].Jmeno}({ListKomodity[i].AktualniCena:F2}) : {(ListKomodity[i].PocetVlastnenych * ListKomodity[i].AktualniCena):F2}ĐĐ";
            }
            Vypis += $"Zbývá: {30-PocetKol}";
            Console.Clear();
            Program.VypisTabulka(Vypis);

        }
        public void VypisCen()
        {
            string Vypis = $"";
            Vypis += "Kryptoměny: Đ";
            for (int i = 0; i < ListKrypta.Count(); i++)
            {
                Vypis += $"{ListKrypta[i].Jmeno}({ListKrypta[i].Ticker})<->({ListKrypta[i].AktualniCena:F2})Đ";
                Console.ForegroundColor = ConsoleColor.White;
            }
            Vypis += "Akcie: Đ";
            for (int i = 0; i < ListAkcie.Count(); i++)
            {
                Vypis += $"{ListAkcie[i].Jmeno}({ListAkcie[i].Ticker})<->({ListAkcie[i].AktualniCena:F2})Đ";
                Console.ForegroundColor = ConsoleColor.White;

            }
            Vypis += "Komodity: Đ";
            for (int i = 0; i < ListKomodity.Count(); i++)
            {
                Vypis += $"{ListKomodity[i].Jmeno}<->({ListKomodity[i].AktualniCena:F2})Đ";
                Console.ForegroundColor = ConsoleColor.White;

            }
            Program.VypisTabulka(Vypis);
        }
        public void Nakup(double pocet, string ticker)
        {
            double pocetKrypta = 0;
            for (int i = 0; i < ListKrypta.Count(); i++)
            {
                if (ListKrypta[i].Ticker.ToLower() == ticker.ToLower() || ListKrypta[i].Jmeno.ToLower() == ticker.ToLower())
                {
                    pocetKrypta = pocet / ListKrypta[i].AktualniCena;
                    ListKrypta[i].PocetVlastnenych += pocetKrypta;
                }
            }
            Penize -= pocet;

        }
        public void NakupKomodity(double pocet, string nazev)
        {
            for (int i = 0; i < ListKomodity.Count(); i++)
            {
                if (ListKomodity[i].Jmeno.ToLower() == nazev.ToLower())
                {
                    ListKomodity[i].PocetVlastnenych += pocet / ListKomodity[i].AktualniCena;
                }
            }
            Penize -= pocet;

        }
        public void NakupAkcie(double pocet, string nazev)
        {
            for (int i = 0; i < ListAkcie.Count(); i++)
            {
                if (ListAkcie[i].Ticker.ToLower() == nazev.ToLower() || ListAkcie[i].Jmeno.ToLower() == nazev.ToLower())
                {
                    ListAkcie[i].PocetVlastnenych += pocet / ListAkcie[i].AktualniCena;
                }
            }
            Penize -= pocet;
        }
    }
}
