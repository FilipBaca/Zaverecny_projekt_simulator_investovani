using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.ComponentModel.Design.Serialization;


namespace Zaverecny_projekt_trading_simulator
{

    public enum Sektor
    {
        tech,
        defense,
        energetika,
        zdravotnictví,
        finančnictví,
        průmyslový,
        nemovitosti,
        kvanta,
        krypto,
        fosilni,
        draheKovy

    }
    public enum TypEventu
    {
        Maly,
        velky
    }
    internal class Program
    {
        public static List<double> listCen = new List<double>();
        public static string AktualniNazevGrafu = "Žádné aktivum";
        static void Main(string[] args)
        {
            Ucet hrac = null;
            int i = 0;
            string cesta = "";
            List<Event> listEventu = NaplneniEventu();
            int pocetKol = 0;
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.White;
            string jmeno = "";
            ConsoleKey confirm = ConsoleKey.UpArrow;
            int cislo = 0;
            do
            {
                i = 0;
                
                do
                {
                    VypisTabulka($"Přihlásit se -> 1ĐRegistrovat se -> 2");
                    Console.WriteLine();
                    if (!int.TryParse(Console.ReadLine(), out cislo))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        VypisTabulka("To není číslo!ĐOdklikněte to a pokračujte");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else if(cislo != 2 && cislo != 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        VypisTabulka("Napište číslo 1 nebo číslo 2!ĐOdklikněte to a pokračujte");
                        Console.WriteLine();
                        Console.ReadKey();
                        Console.Clear();
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                } while (cislo != 2 && cislo != 1);
                switch (cislo)
                {
                    case 1:
                        do
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.White;
                            VypisTabulka("Napište jméno vašeho účtu");
                            Console.WriteLine();
                            cesta = Console.ReadLine().ToLower();
                            if(File.Exists(cesta + ".json") == false)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                VypisTabulka("Tento účet neexistuje! -> ENter pro odkliknutí.");
                                Console.ReadLine();
                            }
                        } while (!File.Exists(cesta + ".json"));
                        Ucet prozatimUcet = Nahrani(cesta);
                        do
                        {


                            VypisTabulka($"Zadejte heslo pro účet: {prozatimUcet.Jmeno}");
                            string heslo = Console.ReadLine();
                            if (heslo == prozatimUcet.Heslo)
                            {
                                hrac = prozatimUcet;
                                break;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                VypisTabulka($"{heslo} je nesprávné heslo!ĐMáte: {3 - i} pokusů!");
                                Console.ReadKey();
                                Console.ForegroundColor = ConsoleColor.White;
                                i++;
                            }
                            if (i == 3)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                VypisTabulka($"Mockrát jste zadali heslo špatně!");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        } while (true);

                        break;
                    case 2:
                        VypisTabulka("Zadejte jméno nového hráče. Max. 12 znaků.");
                        Console.WriteLine();
                        jmeno = Jmeno();
                        Console.WriteLine();
                        jmeno = Potvrzeni(jmeno);
                        hrac = new Ucet(jmeno);
                        bool spravneHeslo = false;

                        do
                        {
                            spravneHeslo = false;
                            string heslo = "";
                            VypisTabulka("Napište heslo");
                            Console.WriteLine();
                            heslo = Console.ReadLine();
                            VypisTabulka("Napište heslo znovu.");
                            Console.WriteLine();
                            if(Console.ReadLine() == heslo)
                            {
                                spravneHeslo = true;
                                hrac.Heslo = heslo;
                            }
                            else
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Red;
                                VypisTabulka("ŠpatněĐ->libovolná klávesa pro opakování hesla.");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.ReadKey();
                                Console.Clear();
                            }
                        } while (spravneHeslo == false);
                        // Naplním list Kryptoměn
                        hrac.ListKrypta.Add(new KryptoMeny(65000, "Bitcoin", 0.05, "BTC"));
                        hrac.ListKrypta.Add(new KryptoMeny(35000, "Ethereum", 0.08, "ETH"));
                        hrac.ListKrypta.Add(new KryptoMeny(0.15, "Dodge", 0.05, "DODGE"));

                        // Naplním list Akcií
                        hrac.ListAkcie.Add(new Akcie("Apple", 180, Sektor.tech, "AAPL", 0.03));
                        hrac.ListAkcie.Add(new Akcie("Lockheed Martin", 450, Sektor.defense, "LMT", 0.02));
                        hrac.ListAkcie.Add(new Akcie("Shell ", 70, Sektor.energetika, "SHEL", 0.035));
                        hrac.ListAkcie.Add(new Akcie("Pfitzer", 30, Sektor.zdravotnictví, "PFE", 0.05));
                        hrac.ListAkcie.Add(new Akcie("Nvidia", 700, Sektor.tech, "NVDA", 0.08));
                        hrac.ListAkcie.Add(new Akcie("IonQ", 5000, Sektor.kvanta, "IONQ", 0.10));
                        hrac.ListAkcie.Add(new Akcie("Blackrock", 1000, Sektor.finančnictví, "BLK", 0.10));
                        hrac.ListAkcie.Add(new Akcie("Visa", 350, Sektor.finančnictví, "IONQ", 0.05));
                        hrac.ListAkcie.Add(new Akcie("Caterpillar", 340, Sektor.průmyslový, "CAT", 0.07));
                        hrac.ListAkcie.Add(new Akcie("CBRE group", 91, Sektor.nemovitosti, "CBRE", 0.03));

                        // Naplním List komodit
                        hrac.ListKomodity.Add(new Komodity(2300, "zlato", 0.01));
                        hrac.ListKomodity.Add(new Komodity(80, "ropa", 0.04));
                        hrac.ListKomodity.Add(new Komodity(30, "stříbro", 0.025));
                        break;
                    default:
                        break;
                }
            } while (i == 3);

           


            
            Console.Clear();
            //tento do while cyklus se nespustí a nevím proč, když hoveruju myší nad breakpointem, který není vyplněný červenou barvou ale je prázdný tak to napíše to co je na fotce co posílám


            string[] investice = { "Kryptoměny", "Akcie", "Komodity" };
            string[] typNakupu = { "Nákup", "Pass", "Prodej" };

            //Naplním list Kryptoměn
            pocetKol = hrac.PocetKol;

            do
            {

                bool podminka = false;
                string konkrAktiv = "";
                string vyber = "";
                string nakup = "";
                Console.Clear();
                string vypis = "               Đ               Đ               Đ               Đ";
                string[] vypisTabulka = vypis.Split('Đ');
                vypisTabulka = VypisTabulkaVraceni(vypis);
                Console.Clear();
                vyber = VyberAktivaObchodu(vypisTabulka, investice, hrac);
                konkrAktiv = VyberKonkretni(vyber, hrac);

                AktualniNazevGrafu = konkrAktiv;
                foreach (KryptoMeny kryptomena in hrac.ListKrypta)
                {
                    if (kryptomena.Ticker.ToLower() == konkrAktiv.ToLower() || kryptomena.Jmeno.ToLower() == konkrAktiv.ToLower()) listCen = kryptomena.ListCen;
                }
                foreach (Akcie kryptomena in hrac.ListAkcie)
                {
                    if (kryptomena.Ticker.ToLower() == konkrAktiv.ToLower() || kryptomena.Jmeno.ToLower() == konkrAktiv.ToLower()) listCen = kryptomena.ListCen;
                }
                foreach (Komodity kryptomena in hrac.ListKomodity)
                {
                    if (kryptomena.Jmeno.ToLower() == konkrAktiv.ToLower()) listCen = kryptomena.ListCen;

                }

                nakup = VyberAktivaObchodu(vypisTabulka, typNakupu, hrac);



                Console.Clear();
                switch (nakup)
                {
                    case "Nákup":
                        do
                        {


                            switch (vyber)
                            {

                                case "Kryptoměny":
                                    podminka = Nakup(hrac, konkrAktiv);
                                    break;
                                case "Akcie":
                                    podminka = NakupAkcie(hrac, konkrAktiv);
                                    break;
                                case "Komodity":
                                    podminka = NakupKomodity(hrac, konkrAktiv);
                                    break;
                                default:
                                    break;
                            }
                        } while (!podminka);
                        break;

                    case "Prodej":
                        switch (vyber)
                        {

                            case "Kryptoměny":
                                Prodej(konkrAktiv, "Kryptoměny", hrac);
                                break;
                            case "Akcie":
                                Prodej(konkrAktiv, "Akcie", hrac);
                                break;
                            case "Komodity":
                                Prodej(konkrAktiv, "Komodity", hrac);
                                break;
                            default:
                                break;
                        }
                        break;
                    case "Konec":
                        break;
                    default:
                        break;
                }
                Event aktualniUdalost = VyberEventu(pocetKol, listEventu);

                do
                {
                    Console.CursorVisible = false;
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    hrac.VypisPortfolia();
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Red;
                    VypisTabulka(aktualniUdalost.Text);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    VypisTabulka("Enter pro confirm");
                    confirm = Console.ReadKey(true).Key;
                } while (confirm != ConsoleKey.Enter);

                Console.Clear();
                Console.SetCursorPosition(0, 0);
                pocetKol++;
                hrac.PocetKol++;
                Ulozeni(hrac);
                ZmenaCen(hrac, aktualniUdalost);
            } while (pocetKol < 30);
            Ulozeni(hrac);

        }
        static string Potvrzeni(string jmeno)
        {
            Console.CursorVisible = true;
            Console.Clear();
            string text = "";
            ConsoleKey klavesa;
            do
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                VypisTabulka($"Potvrďte jméno: {jmeno}ĐAno->EnterĐNe->Esc");
                Console.WriteLine();
                klavesa = Console.ReadKey(true).Key;
                if (klavesa == ConsoleKey.Escape)
                {
                    Console.Clear();
                    VypisTabulka("Napište nové jméno: ");
                    Console.WriteLine();
                    jmeno = Jmeno();
                }
            } while (klavesa != ConsoleKey.Enter);
            text = jmeno;
            return text;
        }
        public static string Jmeno()
        {


            string jmeno = "";
            do
            {
                jmeno = Console.ReadLine();
                if (jmeno.Length == 0)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    VypisTabulka("Jméno musí mít délku!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                }
                else if (jmeno.Length > 12)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    VypisTabulka("Max. délka je: 12!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                }

            } while (jmeno.Length == 0 || jmeno.Length > 12);
            return jmeno;
        }
        public static void VypisTabulka(string text)
        {
            string[] textTabulka = text.Split('Đ');
            int nejdelsi = 0;
            for (int i = 0; i < textTabulka.Length; i++)
            {
                if (nejdelsi < textTabulka[i].Length) nejdelsi = textTabulka[i].Length;
            }
            for (int i = 0; i < nejdelsi + 1; i++)
            {
                Console.Write('=');
            }
            Console.WriteLine();

            for (int i = 0; i < textTabulka.Length; i++)
            {
                Console.WriteLine(textTabulka[i].PadRight(nejdelsi) + " |");
            }
            for (int i = 0; i < nejdelsi + 1; i++)
            {
                Console.Write('=');
            }
        }
        public static string[] VypisTabulkaVraceni(string text)
        {
            string[] textTabulka = text.Split('Đ');
            int nejdelsi = 0;
            string[] tabulka = new string[textTabulka.Length + 2];
            for (int i = 0; i < textTabulka.Length; i++)
            {
                if (nejdelsi < textTabulka[i].Length) nejdelsi = textTabulka[i].Length;
            }
            for (int i = 0; i < nejdelsi + 1; i++)
            {
                Console.Write('=');
                tabulka[0] += '=';
            }
            Console.WriteLine();

            for (int i = 0; i < textTabulka.Length; i++)
            {
                Console.WriteLine(textTabulka[i].PadRight(nejdelsi) + " |");
                tabulka[i + 1] = textTabulka[i].PadRight(nejdelsi) + " |";
            }
            for (int i = 0; i < nejdelsi + 1; i++)
            {
                tabulka[tabulka.Length - 1] += '=';
                Console.Write('=');
            }
            return tabulka;
        }
        public static string VyberAktivaObchodu(string[] vypisTabulka, string[] investice, Ucet hrac)
        {
            Console.CursorVisible = false;

            Console.SetCursorPosition(0, 0);
            Console.Clear();
            string vraceni = "";
            int index = 0;
            ConsoleKey klavesa = ConsoleKey.Enter;

            do
            {
                Console.Clear();

                Console.SetCursorPosition(0, 15);
                hrac.VypisCen();

                vypisTabulka[3] = investice[index].PadRight(vypisTabulka[0].Length) + '|';
                for (int i = 0; i < vypisTabulka.Length; i++)
                {
                    if (i == 3)
                    {

                        switch (investice[index])
                        {
                            case "Kryptoměny":
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case "Akcie":
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            case "Komodity":
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case "Konec":
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case "Prodej":
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case "Nákup":
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;

                            default:
                                break;
                        }
                    }
                    Console.SetCursorPosition(0, i);
                    Console.WriteLine(vypisTabulka[i]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                klavesa = Console.ReadKey(true).Key;
                switch (klavesa)
                {
                    case ConsoleKey.LeftArrow:
                        index--;
                        break;

                    case ConsoleKey.RightArrow:
                        index++;
                        break;

                    case ConsoleKey.Enter:
                        vraceni = investice[index];
                        break;
                    default:
                        break;
                }
                switch (index)
                {
                    case > 2:
                        index = 0;
                        break;
                    case < 0:
                        index = 2;
                        break;
                    default:
                        break;
                }
            } while (klavesa != ConsoleKey.Enter);
            return vraceni;
        }
        public static string VyberKonkretni(string typAktiva, Ucet hrac)
        {
            bool podminka = false;
            bool potvrzeni = false;
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = true;
            string vyber = "";
            ConsoleKey klavesa = ConsoleKey.A;
            do
            {
                podminka = false;
                klavesa = ConsoleKey.A;
                switch (typAktiva)
                {
                    case "Kryptoměny":
                        Console.Clear();
                        Console.SetCursorPosition(0, 10);
                        hrac.VypisCen();
                        Console.SetCursorPosition(0, 0);
                        VypisTabulka("[TICKER] kryptoměny: ");

                        Console.WriteLine();

                        vyber = Console.ReadLine();
                        for (int i = 0; i < hrac.ListKrypta.Count; i++)
                        {
                            if (vyber.ToLower() == hrac.ListKrypta[i].Ticker.ToLower() || vyber.ToLower() == hrac.ListKrypta[i].Jmeno.ToLower())
                            {
                                podminka = true;
                                break;
                            }

                        }
                        if (podminka == false)
                        {
                            Console.SetCursorPosition(0, 0);
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            VypisTabulka($"To není platný vstup!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }


                        break;
                    case "Komodity":
                        Console.Clear();
                        Console.SetCursorPosition(0, 10);
                        hrac.VypisCen();
                        Console.SetCursorPosition(0, 0);
                        VypisTabulka("Název komodity: ");
                        Console.WriteLine();
                        vyber = Console.ReadLine();
                        for (int i = 0; i < hrac.ListKomodity.Count; i++)
                        {
                            if (vyber.ToLower() == hrac.ListKomodity[i].Jmeno.ToLower())
                            {
                                podminka = true;
                                break;
                            }

                        }
                        if (podminka == false)
                        {
                            Console.SetCursorPosition(0, 0);
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            VypisTabulka($"To není platný vstup!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }



                        break;
                    case "Akcie":
                        Console.Clear();
                        Console.SetCursorPosition(0, 10);
                        hrac.VypisCen();
                        Console.SetCursorPosition(0, 0);
                        VypisTabulka("[TICKER] akcie: ");
                        Console.WriteLine();
                        vyber = Console.ReadLine();
                        for (int i = 0; i < hrac.ListAkcie.Count; i++)
                        {
                            if (vyber.ToLower() == hrac.ListAkcie[i].Ticker.ToLower() || vyber.ToLower() == hrac.ListAkcie[i].Jmeno.ToLower())
                            {
                                podminka = true;
                                break;
                            }


                        }
                        if (podminka == false)
                        {
                            Console.SetCursorPosition(0, 0);
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            VypisTabulka($"To není platný vstup!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }


                        break;

                    default:
                        break;
                }

                if (podminka == false)
                {
                    do
                    {
                        Console.CursorVisible = false;
                        Console.SetCursorPosition(0, 0);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        VypisTabulka($"To není platný vstup!ĐENTER -> Pokračování");
                        klavesa = Console.ReadKey().Key;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.CursorVisible = true;
                    } while (klavesa != ConsoleKey.Enter);
                }
                else
                {

                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.CursorVisible = false;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    VypisTabulka($"Potvrďte: {vyber}");
                    Console.ForegroundColor = ConsoleColor.White;
                    klavesa = Console.ReadKey().Key;
                    if (klavesa == ConsoleKey.Enter)
                    {
                        potvrzeni = true;
                    }
                    else
                    {
                        potvrzeni = false;
                    }
                }
            } while (!podminka || !potvrzeni);
            return vyber;
        }
        public static double OvereniNakup(Ucet hrac)
        {
            double limit = hrac.Penize;
            double num = double.MinValue;
            bool jeNum = true;
            bool jeMaly = false;
            bool mocVelky = false;
            bool potvrzeni = false;
            ConsoleKey klavesa = ConsoleKey.A;
            ConsoleKey porvKey = ConsoleKey.P;
            do
            {
                Console.CursorVisible = true;
                jeNum = true;
                jeMaly = false;
                mocVelky = false;
                VypisTabulka("Kolik peněz chcete investovat?");
                Console.WriteLine();
                if (!double.TryParse(Console.ReadLine(), out num))
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    VypisTabulka($"To není číslo!");
                    Console.WriteLine();
                    jeNum = false;

                }
                else if (num != double.MinValue && num > limit)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    VypisTabulka($"{num}$ je příliš!ĐMáte pouze: {hrac.Penize}$ĐEnter -> pokračování");
                    Console.WriteLine();
                    mocVelky = true;
                }
                else if (num < 0 && num != double.MinValue)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    VypisTabulka($"Záporné číslo!");
                    Console.WriteLine();
                    jeMaly = true;
                }
                Console.CursorVisible = false;

                if (jeMaly == false && jeNum == true && mocVelky == false)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.CursorVisible = false;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    VypisTabulka($"Potvrďte částku: {num} -> EnterĐLibovolná klávesa -> zpět");
                    porvKey = Console.ReadKey().Key;
                    if (porvKey == ConsoleKey.Enter)
                    {
                        potvrzeni = true;
                    }
                    else
                    {
                        potvrzeni = false;
                    }
                }
                else
                {
                    do
                    {

                        VypisTabulka("ENTER -> pokračování");

                        klavesa = Console.ReadKey().Key;

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Clear();
                        Console.SetCursorPosition(0, 0);
                    } while (klavesa != ConsoleKey.Enter);
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.CursorVisible = true;
            } while (!potvrzeni);

            return num;
        }
        public static bool Nakup(Ucet hrac, string kryptoTick)
        {
            bool podminka = false;

            Console.WriteLine();
            foreach (KryptoMeny krypto in hrac.ListKrypta)
            {
                if (krypto.Ticker.ToLower() == kryptoTick.ToLower() || krypto.Jmeno.ToLower() == kryptoTick.ToLower())
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    double pocetPenez = OvereniNakup(hrac);
                    hrac.Nakup(pocetPenez, kryptoTick);
                    Console.SetCursorPosition(0, 0);
                    VypisTabulka($"Hráč: {hrac.Jmeno} investoval: {pocetPenez} do kryptoměny [{kryptoTick}]$");
                    podminka = true;

                    break;
                }
            }
            if (podminka == false)
            {
                ConsoleKey klavesa = ConsoleKey.UpArrow;
                do
                {

                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    VypisTabulka($"Tato kryptoměna neexistuje!ĐEnter pro pokračování");
                    klavesa = Console.ReadKey(true).Key;
                    Console.ForegroundColor = ConsoleColor.White;
                } while (klavesa != ConsoleKey.Enter);

            }
            return podminka;
        }
        public static bool NakupKomodity(Ucet hrac, string komoditaNazev)
        {
            bool podminka = false;

            Console.WriteLine();
            foreach (Komodity komodita in hrac.ListKomodity)
            {
                if (komodita.Jmeno.ToLower() == komoditaNazev.ToLower())
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    double pocetPenez = OvereniNakup(hrac);
                    hrac.NakupKomodity(pocetPenez, komoditaNazev);
                    VypisTabulka($"Hráč: {hrac.Jmeno} investoval: {pocetPenez} do komodity: {komoditaNazev} {pocetPenez}$");
                    podminka = true;
                    break;

                }
            }
            if (podminka == false)
            {
                ConsoleKey klavesa = ConsoleKey.UpArrow;
                do
                {

                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    VypisTabulka($"Tato komodita neexistuje neexistuje!ĐEnter pro pokračování");
                    klavesa = Console.ReadKey(true).Key;
                    Console.ForegroundColor = ConsoleColor.White;
                } while (klavesa != ConsoleKey.Enter);

            }


            return podminka;
        }
        public static bool NakupAkcie(Ucet hrac, string akcieNazev)
        {
            bool podminka = false;

            Console.WriteLine();
            foreach (Akcie akcie in hrac.ListAkcie)
            {
                if (akcie.Ticker.ToLower() == akcieNazev.ToLower() || akcie.Jmeno.ToLower() == akcieNazev.ToLower())
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);

                    double pocetPenez = OvereniNakup(hrac);
                    hrac.NakupAkcie(pocetPenez, akcieNazev);
                    VypisTabulka($"Hráč: {hrac.Jmeno} investoval: {pocetPenez} do akcie: [{akcie.Ticker}] {pocetPenez}$");
                    podminka = true;
                    break;

                }
            }
            if (podminka == false)
            {
                ConsoleKey klavesa = ConsoleKey.UpArrow;
                do
                {

                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    VypisTabulka($"Tato akcie neexistuje!ĐEnter pro pokračování");
                    klavesa = Console.ReadKey(true).Key;
                    Console.ForegroundColor = ConsoleColor.White;
                } while (klavesa != ConsoleKey.Enter);

            }
            return podminka;
        }
        public static List<Event> NaplneniEventu()
        {

            List<Event> list = new List<Event>();
            Dictionary<Sektor, double> rozsah = new Dictionary<Sektor, double>();

            //VelkeEventy
            list.Add(new Event("Vypukla mezinárodní válka! USA vylodilo vojáky v Evropě!", TypEventu.velky, new Dictionary<Sektor, double> { { Sektor.defense, 0.25 }, { Sektor.energetika, 0.15 }, { Sektor.tech, -0.20 } }));
            list.Add(new Event("Objevil se nový, neznámí typ viru! WHO bije na poplach!", TypEventu.velky, new Dictionary<Sektor, double> { { Sektor.defense, 0.25 }, { Sektor.energetika, 0.15 }, { Sektor.tech, -0.20 } }));
            list.Add(new Event("První sériově vyrobený motor vyroben! Éra fosilních paliv je minulosti!", TypEventu.velky, new Dictionary<Sektor, double> { { Sektor.tech, 0.15 }, { Sektor.kvanta, 0.05 }, { Sektor.energetika, -0.40 } }));
            list.Add(new Event("Obří výpadek evropských centrálních bank zatím bez jasného vyníka. Důvěra v centralizované měny klesá! ", TypEventu.velky, new Dictionary<Sektor, double> { { Sektor.draheKovy, 0.12 }, { Sektor.krypto, 0.25 }, { Sektor.finančnictví, -0.24 } }));
            list.Add(new Event("Donald Trump na mírové návštěvě Číny a Ruské federace. ", TypEventu.velky, new Dictionary<Sektor, double> { { Sektor.defense, -0.25 }, { Sektor.energetika, 0.15 }, { Sektor.tech, 0.15 } }));
            list.Add(new Event("Podivný signál zachycen z druhé nejbližší hvězdy od Země! ", TypEventu.velky, new Dictionary<Sektor, double> { { Sektor.defense, 0.25 }, { Sektor.tech, 0.20 } }));
            list.Add(new Event("AI bublina praská ve švech! AI byla příliš nadhodnocená a negeruje slibované zisky. Investoři panicky prodávají.", TypEventu.velky, new Dictionary<Sektor, double> { { Sektor.tech, -0.50 }, { Sektor.průmyslový, +0.25 }, { Sektor.nemovitosti, 0.25 } }));
            list.Add(new Event("Kvantový počítač překonal šifrování americké armády.", TypEventu.velky, new Dictionary<Sektor, double> { { Sektor.defense, 0.25 }, { Sektor.kvanta, 0.20 }, { Sektor.tech, -0.18 }, { Sektor.finančnictví, -0.20 } }));
            list.Add(new Event("Blackout jaký svět neviděl! Celá Evropa 72 dní bez proudu", TypEventu.velky, new Dictionary<Sektor, double> { { Sektor.energetika, -0.25 }, { Sektor.krypto, -0.35 }, { Sektor.tech, -0.20 } }));


            //Maly
            list.Add(new Event("Apple omylem ukázal design nového iPhonu. Fanoušci šílí.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.tech, 0.03 } }));
            list.Add(new Event("Nvidia představila grafické karty nové generace. Nestíhají se vyrábět.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.tech, 0.05 } }));
            list.Add(new Event("Evropská unie hrozí technologickým gigantům obří pokutou za monopol.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.tech, -0.04 } }));
            list.Add(new Event("Zemětřesení na Taiwanu dočasně omezilo výrobu mikročipů.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.tech, -0.05 } }));
            list.Add(new Event("Velká sociální síť zažila výpadek serverů na 12 hodin.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.tech, -0.03 } }));
            list.Add(new Event("Softwarová aktualizace způsobila pád operačních systémů na letištích.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.tech, -0.04 } }));
            list.Add(new Event("Analytici předpovídají Applu rekordní prodeje v tomto kvartálu.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.tech, 0.04 } }));

            // Defense
            list.Add(new Event("Americká vláda schválila nový balík vojenské pomoci pro spojence.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.defense, 0.05 } }));
            list.Add(new Event("Lockheed Martin úspěšně otestoval novou hypersonickou střelu.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.defense, 0.04 } }));
            list.Add(new Event("Zbrojařské firmy hlásí nedostatek střelného prachu pro výrobu.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.defense, -0.02 } }));
            list.Add(new Event("Mírové protesty před fabrikou na drony mírně zdržely dodávky.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.defense, -0.01 } }));
            list.Add(new Event("Ministerstvo obrany podepsalo obří kontrakt na nákup stíhaček.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.defense, 0.06 } }));
            list.Add(new Event("Závada na softwaru vojenského satelitu stála firmu miliony.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.defense, -0.03 } }));

            // Energetika
            list.Add(new Event("Norsko objevilo nové ložisko zemního plynu v Severním moři.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.energetika, 0.04 } }));
            list.Add(new Event("Požár na ropné plošině v Mexickém zálivu zastavil těžbu.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.energetika, -0.05 } }));
            list.Add(new Event("Solární elektrárny hlásí rekordní produkci díky slunečnému létu.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.energetika, 0.02 } }));
            list.Add(new Event("Ekologičtí aktivisté zablokovali uhelný důl v Německu.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.energetika, -0.02 } }));
            list.Add(new Event("Cena uranu roste, zájem o jadernou energii celosvětově stoupá.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.energetika, 0.04 } }));
            list.Add(new Event("Zastaralá rozvodná síť v Texasu kolabuje pod náporem veder.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.energetika, -0.04 } }));

            // Zdravotnictví
            list.Add(new Event("Pfizer zahájil finální fázi testování vakcíny proti rýmě.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.zdravotnictví, 0.04 } }));
            list.Add(new Event("Lék na hubnutí odhalil nebezpečné vedlejší účinky. Stahuje se z trhu.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.zdravotnictví, -0.06 } }));
            list.Add(new Event("Vláda schválila vyšší dotace na nákup nemocničních přístrojů.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.zdravotnictví, 0.03 } }));
            list.Add(new Event("Patent na klíčový lék proti rakovině vypršel, konkurence ho kopíruje.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.zdravotnictví, -0.04 } }));
            list.Add(new Event("Chřipková sezóna začala letos dříve, lékárny hlásí vyprodané zásoby.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.zdravotnictví, 0.03 } }));
            list.Add(new Event("Skandál s kontaminovanými antibiotiky poškodil reputaci laboratoří.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.zdravotnictví, -0.05 } }));

            // Finančnictví
            list.Add(new Event("Centrální banka mírně snížila základní úrokovou sazbu.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.finančnictví, 0.03 } }));
            list.Add(new Event("Bankovní magnát byl zatčen za praní špinavých peněz.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.finančnictví, -0.05 } }));
            list.Add(new Event("JPMorgan hlásí rekordní zisky z poplatků za obchodování.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.finančnictví, 0.04 } }));
            list.Add(new Event("Nová regulace omezuje maximální poplatky za vedení účtů.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.finančnictví, -0.02 } }));
            list.Add(new Event("Mladá generace masivně opouští tradiční banky a přechází na fintech.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.finančnictví, -0.03 } }));
            list.Add(new Event("Ratingová agentura zvýšila hodnocení stability největších bank.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.finančnictví, 0.03 } }));

            // Průmysl
            list.Add(new Event("Automobilky hlásí obnovení plných dodávek oceli a hliníku.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.průmyslový, 0.03 } }));
            list.Add(new Event("Stávka zaměstnanců v loděnicích úplně paralyzovala export.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.průmyslový, -0.05 } }));
            list.Add(new Event("Nová továrna na elektromobily ve střední Evropě zahájila provoz.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.průmyslový, 0.04 } }));
            list.Add(new Event("Cena lithia padá, výroba baterií je výrazně levnější.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.průmyslový, 0.03 } }));
            list.Add(new Event("Zpřísnění emisních povolenek drtí těžký průmysl.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.průmyslový, -0.04 } }));

            // Nemovitosti
            list.Add(new Event("Stavební úřady zjednodušily proces schvalování nových bytových domů.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.nemovitosti, 0.04 } }));
            list.Add(new Event("Ceny stavebních materiálů (dřevo, cement) vzrostly o 15 %.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.nemovitosti, -0.04 } }));
            list.Add(new Event("Zájem o luxusní kancelářské prostory v centrech měst prudce klesá.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.nemovitosti, -0.03 } }));
            list.Add(new Event("Mladé rodiny se stěhují na venkov, poptávka po bytech stagnuje.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.nemovitosti, -0.02 } }));
            list.Add(new Event("Vláda schválila příspěvek na bydlení pro mladé lidi.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.nemovitosti, 0.04 } }));

            // Kvanta
            list.Add(new Event("IonQ oznámil vytvoření kvantového procesoru s 50 čistými qubity.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.kvanta, 0.06 } }));
            list.Add(new Event("Kvantové šifrování bylo poprvé nasazeno v komerčním provozu.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.kvanta, 0.04 } }));
            list.Add(new Event("Chlazení kvantového počítače selhalo, drahý výzkum byl zničen.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.kvanta, -0.05 } }));
            list.Add(new Event("Zahraniční tajná služba údajně prolomila běžné šifry pomocí kvanta.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.kvanta, 0.05 } }));
            list.Add(new Event("Investoři začínají být skeptičtí k rychlému nasazení kvantových PC.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.kvanta, -0.03 } }));

            // Internet / Šumy
            list.Add(new Event("Elon Musk změnil logo na svém profilu na obrázek psa. Doge coin letí.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.tech, 0.05 } }));
            list.Add(new Event("Populární influencer streamoval hru a omylem přitom doporučil nákup stříbra.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.energetika, 0.03 } }));
            list.Add(new Event("Na TikToku se šíří trend 'Boycott Wall Street', teenageři hromadně shortují.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.finančnictví, -0.03 } }));
            list.Add(new Event("Slavný investor prohlásil, že hotovost jsou odpadky a zlato je jistota.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.energetika, 0.04 } }));
            list.Add(new Event("Ekonomická celebrita na Twitteru předpověděla na zítra konec světa. Trhy mírně klesly.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.finančnictví, -0.02 } }));
            list.Add(new Event("V diskusním fóru Reddit se zorganizovala skupina 2 milionů lidí a pumpují akcie.", TypEventu.Maly, new Dictionary<Sektor, double> { { Sektor.tech, 0.06 } }));

            for (int i = 0; i < list.Count - 1; i++)
            {
                int nahoda = Random.Shared.Next(i, list.Count);
                Event udalost = list[nahoda];
                list[nahoda] = list[i];
                list[i] = udalost;

            }
            return list;


        }
        public static Event VyberEventu(int pocetKol, List<Event> listEventu)
        {
            int pocitadlo = 0;
            Event vraceni = null;
            if (pocetKol % 6 == 0)
            {
                foreach (Event udalost in listEventu)
                {
                    if (udalost.DruhEventu == TypEventu.velky)
                    {
                        vraceni = udalost;

                        break;
                    }
                }
                listEventu.Remove(vraceni);
            }
            else
            {
                foreach (Event udalost in listEventu)
                {
                    if (udalost.DruhEventu == TypEventu.Maly)
                    {
                        vraceni = udalost;
                        break;
                    }
                }
                listEventu.Remove(vraceni);
            }
            return vraceni;
        }
        public static void ZmenaCen(Ucet hrac, Event udalost)
        {
            foreach (KryptoMeny krypto in hrac.ListKrypta)
            {
                double eventRozsah = 0;

                if (udalost.Rozsah.ContainsKey(Sektor.krypto))
                {
                    eventRozsah = udalost.Rozsah[Sektor.krypto];
                }
                double zmenaCeny = (Random.Shared.NextDouble() * 2 - 1) * krypto.Volatility;
                krypto.AktualniCena *= zmenaCeny + 1 + eventRozsah;
                if (krypto.AktualniCena < 0.01) krypto.AktualniCena = 0.01;
                krypto.ZapisCen();
            }
            foreach (Akcie a in hrac.ListAkcie)
            {
                double eventRozsah = 0;

                if (udalost.Rozsah.ContainsKey(a.Oblast))
                {
                    eventRozsah = udalost.Rozsah[a.Oblast];
                }
                double zmenaCeny = (Random.Shared.NextDouble() * 2 - 1) * a.Volatility;
                a.AktualniCena *= zmenaCeny + 1 + eventRozsah;
                if (a.AktualniCena < 0.01) a.AktualniCena = 0.01;
                a.ZapisCen();
            }
            foreach (Komodity k in hrac.ListKomodity)
            {
                double eventRozsah = 0;

                if (udalost.Rozsah.ContainsKey(Sektor.draheKovy))
                {
                    eventRozsah = udalost.Rozsah[Sektor.draheKovy];
                }
                double zmenaCeny = (Random.Shared.NextDouble() * 2 - 1) * k.Volatility;
                k.AktualniCena *= zmenaCeny + 1 + eventRozsah;
                if (k.AktualniCena < 0.01) k.AktualniCena = 0.01;
                k.ZapisCen();
            }

        }
        public static void Short(string nazevTicker, string typAktiva)
        {

        }
        public static void Prodej(string nazevTicker, string typAktiva, Ucet hrac)
        {
            double castka = 0;
            if (typAktiva == "Kryptoměny")
            {
                foreach (KryptoMeny krypto in hrac.ListKrypta)
                {
                    if (krypto.Ticker.ToLower() == nazevTicker.ToLower() || nazevTicker.ToLower() == krypto.Jmeno.ToLower() && krypto.PocetVlastnenych != 0)
                    {
                        castka = OvereniCisloProdej("Kryptoměnty", krypto.AktualniCena, krypto.PocetVlastnenych);
                        hrac.Penize += castka;
                        krypto.PocetVlastnenych -= castka / krypto.AktualniCena;
                    }

                }
            }
            else if (typAktiva == "Akcie")
            {
                foreach (Akcie akcie in hrac.ListAkcie)
                {
                    if ((akcie.Ticker.ToLower() == nazevTicker.ToLower() || akcie.Jmeno.ToLower() == nazevTicker.ToLower()) && akcie.PocetVlastnenych != 0)
                    {
                        castka = OvereniCisloProdej("Akcie", akcie.AktualniCena, akcie.PocetVlastnenych);
                        hrac.Penize += castka;
                        akcie.PocetVlastnenych -= castka / akcie.AktualniCena;
                    }

                }
            }
            else if (typAktiva == "Komodity")
            {
                foreach (Komodity komodity in hrac.ListKomodity)
                {
                    if (komodity.Jmeno.ToLower() == nazevTicker.ToLower() && komodity.PocetVlastnenych != 0)
                    {
                        castka = OvereniCisloProdej("Komodity", komodity.AktualniCena, komodity.PocetVlastnenych);
                        hrac.Penize += castka;
                        komodity.PocetVlastnenych -= castka / komodity.AktualniCena;
                    }

                }
            }

        }
        public static double OvereniCisloProdej(string jmenoAktiva, double aktualniCena, double pocetVlastnenych)
        {
            double maxHodnota = aktualniCena * pocetVlastnenych;
            double spravnaCena = 0;
            ConsoleKey klavesa = ConsoleKey.C;

            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                VypisTabulka($"Napište, za kolik dolarů chcete prodat {jmenoAktiva} (Max: {maxHodnota:F2}$): ");
                Console.WriteLine();

                if (!double.TryParse(Console.ReadLine(), out spravnaCena))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    VypisTabulka("To není číslo! Stiskněte Enter pro opakování.");
                    Console.ReadKey();
                    continue;
                }

                if (spravnaCena > maxHodnota)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    VypisTabulka("Číslo je moc velké! Nemáte tolik aktiv. Stiskněte Enter.");
                    Console.ReadKey();
                }
                else if (spravnaCena <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    VypisTabulka("Číslo musí být větší než 0! Stiskněte Enter.");
                    Console.ReadKey();
                }
                else
                {
                    // Tady to teprve počká na skutečné potvrzení od hráče
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    VypisTabulka($"Potvrďte prodej za {spravnaCena}$ -> ENTER ");
                    klavesa = Console.ReadKey(true).Key;


                }
            } while (klavesa != ConsoleKey.Enter);
            return spravnaCena;
        }
        public static void Ulozeni(Ucet hrac)
        {
            string cesta = $"{hrac.Jmeno.ToLower()}.json";
            var moznosti = new JsonSerializerOptions { WriteIndented = true };
            string jsonText = JsonSerializer.Serialize(hrac, moznosti);
            File.WriteAllText(cesta, jsonText);
        }
        public static Ucet Nahrani(string cesta)
        {
            cesta += ".json";
            if (File.Exists(cesta) == true)
            {
                string jsonText = File.ReadAllText(cesta);

                Ucet nactenyHrac = JsonSerializer.Deserialize<Ucet>(jsonText);
                return nactenyHrac;
            }
            return null;
        }


    }
}
