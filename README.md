# Trading Simulator - Závěrečný Projekt

Simulátor obchodování na finančních trzích (Kryptoměny, Akcie, Komodity) pro herní konzoli v C#. Aplikace podporuje dynamické přepočítávání cen na základě náhodných událostí (eventů) a kompletní ukládání a načítání dat pomocí formátu JSON.

## 1. Zadání projektu
Cílem projektu je vytvořit simulátor burzy, kde hráč začíná s počátečním kapitálem 1000$. V každém kole může nakupovat a prodávat vybraná aktiva, přičemž ceny reagují na globální makroekonomické události. Cílem hry je maximalizovat svůj majetek během 30 herních kol.

## 2. Struktura aplikace a model tříd

### Přehled tříd (Domain Model)
- **Program**: Hlavní řídicí logika hry, správa menu, obsluha uživatelských vstupů a vykreslování UI (grafy, tabulky).
- **Ucet**: Reprezentuje konto hráče (Jméno, Peníze, Heslo) a drží kolekce vlastněných aktiv.
- **KryptoMeny / Akcie / Komodity**: Třídy reprezentující jednotlivé investiční instrumenty. Drží informace o aktuální ceně, volatilitě, počtu vlastněných kusů..
- **Event**: Reprezentuje náhodnou událost ovlivňující trh (slovník dopadů na jednotlivé sektory).

### Klíčové metody
- `Program.ZmenaCen(Ucet hrac, Event udalost)`: Přepočítává ceny všech aktiv na základě jejich základní volatility a dopadu aktuálního eventu.
- `Program.VyberEventu(...)`: Bezpečně losuje a odstraňuje odehrané eventy z z listu.
- `Program.Ulozeni / Nahrani`: Zajišťují serializaci a deserializaci uživatelských dat.

## 3. Práce se soubory 
Program využívá formát JSON
- **Registrace**: Vytvoří se nová instance třídy `Ucet` a uloží se jako soubor `{jmeno}.json`.
- **Přihlášení**: Program ověří existenci souboru a načte data. Následně proběhne verifikace hesla (max. 3 pokusy).
- **Autosave**: Stav hry (portfolio, zůstatek, ceny) se automaticky ukládá na disk na konci každého odehraného kola.

## 4. Ovládání aplikace
Aplikace se kompletně ovládá v konzoli:
- **Pohyb v menu instrumentů**: Pomocí šipek (`Doleva` / `Doprava`) a potvrzení klávesou `Enter`.
- **Zadávání textu/částek**: Pomocí `Console.ReadLine()` s integrovanou validací (ochrana proti zadání nečíselných hodnot, záporných čísel nebo částek převyšujících limit).
- **Potvrzení kola**: Klávesa `Enter`.
