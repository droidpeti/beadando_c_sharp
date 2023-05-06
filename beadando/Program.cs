namespace beadando
{
    //cím;megjelenés(év);hossz(perc);gyártó;műfjakok(#-el elválasztva);szereplők(#-el elválasztva);forgalmazó;
    public struct film
    {
        public string cim;
        public int megjelenes;
        public int hossz;
        public string gyarto;
        public string[] mufajok;
        public string[] szereplok;
        public string forgalmazo;
    }
    internal class Program
    {
        static void keret(byte x, byte y, byte hossz, int magassag)
        {
            //üdvözlő keret kiirató
            Console.Clear();
            if (x > Console.WindowWidth)
                x = (byte)(Console.WindowWidth - hossz);

            if (y > Console.WindowHeight)
                y = (byte)(Console.WindowHeight - magassag);

            if (Console.WindowWidth < x + hossz)
                hossz = (byte)(Console.WindowWidth - x);

            if (Console.WindowHeight < y + magassag)
                magassag = (byte)(Console.WindowHeight - y);


            for (byte i = 0; i < magassag; i++)
            {
                for (byte j = 0; j < hossz; j++)
                {
                    Console.SetCursorPosition(x + j, y + i);
                    if (i != 0 && i != magassag - 1 && j != 0 && j != hossz - 1)
                        Console.Write(" ");
                    else if (i == 0 && j == 0)
                        Console.Write("╔");
                    else if (i == 0 && j == hossz - 1)
                        Console.Write("╗");
                    else if (i == magassag - 1 && j == 0)
                        Console.Write("╚");
                    else if (i == magassag - 1 && j == hossz - 1)
                        Console.Write("╝");
                    else if ((i == 0 || i == magassag - 1) && (j != 0 && j != hossz - 1))
                        Console.Write("═");
                    else
                        Console.Write("║");
                }
            }
            
            //╝ ╗ ║ ╚ ╔ ═
        }
        static void szovegkiiratas(byte x, byte y, byte hossz, byte magassag, string szoveg)
        {
            Console.SetCursorPosition((x + hossz / 2) - szoveg.Length / 2 + 1, magassag / 2 + y);
            Console.Write(szoveg);
        }

        static film[] beolvaso()
        {
            //A Filmek beolvasása fájlból, sajnos 100-nál nagyobb listával nem fog működni :(
            film[] filmek = new film[100];

            FileStream fs = new FileStream("../../../../filmek.txt", FileMode.Open);
            StreamReader r = new StreamReader(fs);
            string sor;
            string[] feldarabolt;
            int db = 0;
            while(!r.EndOfStream)
            {
                sor = r.ReadLine();
                feldarabolt = sor.Split(";");
                //cím;megjelenés(év);hossz(perc);gyártó;műfjakok(#-el elválasztva);szereplők(#-el elválasztva);forgalmazó
                filmek[db].cim = feldarabolt[0];
                filmek[db].megjelenes = Convert.ToInt32(feldarabolt[1]);
                filmek[db].hossz = Convert.ToInt32(feldarabolt[2]);
                filmek[db].gyarto = feldarabolt[3];
                filmek[db].mufajok = feldarabolt[4].Split("#");
                filmek[db].szereplok = feldarabolt[5].Split("#");
                filmek[db++].forgalmazo = feldarabolt[6];
            }
            r.Close();
            fs.Close();
            Array.Resize(ref filmek, db);

            return filmek;
        }

        static void filmlistakiirato(film film, string vegszoveg)
        {
            Console.Clear();
            //A beolvasott filmek listája kiiratása
            byte sor = 0;
            Console.BackgroundColor = ConsoleColor.Magenta;
            keret(0,0, 100, 13+film.mufajok.Length/5+film.szereplok.Length/3);
            Console.SetCursorPosition(2, ++sor);
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write($"{film.cim}");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(2, ++sor);
            Console.WriteLine($"Megjelenés: {film.megjelenes} ");
            Console.SetCursorPosition(2, ++sor);
            Console.WriteLine($"Hossza: {film.hossz} perc");
            Console.SetCursorPosition(2, ++sor);
            Console.WriteLine($"Készítette: {film.gyarto} ");
            ++sor;
            Console.SetCursorPosition(2, ++sor);
            Console.Write("Műfajok: ");
            Console.SetCursorPosition(2, ++sor);
            for (byte j = 0; j < film.mufajok.Length; j++)
                {
                Console.Write(film.mufajok[j] + ", ");
                if (j % 5 == 0 && j != 0) 
                    {
                        Console.SetCursorPosition(2, ++sor);
                    }
                }
            ++sor;
            Console.SetCursorPosition(2, ++sor);
            Console.Write("Szereplők: ");
            Console.SetCursorPosition(2, ++sor);
            for (byte j = 0; j < film.szereplok.Length; j++)
                {
                    Console.Write(film.szereplok[j] + ", ");
                if (j % 3 == 0 && j != 0)
                {
                    Console.SetCursorPosition(2, ++sor);
                }
            }
            Console.SetCursorPosition(2, ++sor);
            Console.WriteLine($"Kiadta: {film.forgalmazo}\n\n");

            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(vegszoveg);
            Console.ReadKey();
        }

        static int filmvalaszto(film[] filmek)
        {
            Console.Clear();
            Console.WriteLine("Válaszd ki, hogy melyik filmnek az adatait szeretnéd megnézni: ");
            for(byte i = 0; i < filmek.Length; i++)
            {
                Console.WriteLine($"{i+1}. {filmek[i].cim}\n");
            }
            Console.WriteLine("Add meg a film sorsámát!");
            return hibakezelt_int()-1;
        }
        static byte menu()
        {
            //Menü kezelése, ellenőrzéssel
            bool ok = false;
            byte menupont = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("Válassz egy menüpontot!");
                Console.WriteLine("1: Filmek kilistázása minden adattal\n2: Új film feltöltése\n3: Egy film adatai megváltoztatása\n4: Keresés különböző szempontok alapján\n5: Kilépés");
                Console.Write("Add meg a kiválasztott menüpontot: ");
                ok = Byte.TryParse(Console.ReadLine(), out menupont);
                if (menupont < 1 || menupont > 5 || !ok)
                {
                    ok = false;
                    Console.WriteLine("Helyes számot adj meg!!! (Nyomd meg az ENTERt, hogy megadhasd...)");
                    Console.ReadLine();
                }
            } while (!ok);
            return menupont;
        }

        static film filmfeltoltes(film filmadatok)
        {
            //Új film feltöltése
            Console.Clear();
            Console.WriteLine("Add meg a film címét!");
            filmadatok.cim = hibakezelt_string();

            Console.WriteLine("Add meg a film megjelenési évét!");
            filmadatok.megjelenes = hibakezelt_int();

            Console.WriteLine("Add meg a film hosszát percben!");
            filmadatok.hossz = hibakezelt_int();

            Console.WriteLine("Add meg a film gyártóját!");
            filmadatok.gyarto = hibakezelt_string();

            Console.WriteLine("Add meg a film műfajait ,-vel elválasztva!");
            filmadatok.mufajok = hibakezelt_string().Split(",");

            Console.WriteLine("Add meg a film szereplőit ,-vel elválasztva!");
            filmadatok.szereplok = hibakezelt_string().Split(",");

            Console.WriteLine("Add meg a film forgalmazóját!");
            filmadatok.forgalmazo = hibakezelt_string();

            FileStream fs = new FileStream("../../../../filmek.txt", FileMode.Append);
            StreamWriter w = new StreamWriter(fs);
            w.Write($"{filmadatok.cim};{filmadatok.megjelenes};{filmadatok.hossz};{filmadatok.gyarto};");
            for (byte i = 0; i < filmadatok.mufajok.Length; i++)
            {
                w.Write($"{filmadatok.mufajok[i]}");
                if (i != filmadatok.mufajok.Length - 1)
                {
                    w.Write("#");
                }
            }
            w.Write(";");
            for (byte i = 0; i < filmadatok.szereplok.Length; i++)
            {
                w.Write($"{filmadatok.szereplok[i]}");
                if (i != filmadatok.szereplok.Length - 1)
                {
                    w.Write("#");
                }
            }
            w.Write(";");
            w.WriteLine($"{filmadatok.forgalmazo}");

            w.Close();
            fs.Close();
            Console.WriteLine("Sikeres feltöltés!!!");
            Console.WriteLine("Nyomd meg az ETERT, hogy visszamenj a menübe!");
            Console.ReadLine();

            return filmadatok;
        }

        //string ellenőrző (ne legyen üres!!!)
        static string hibakezelt_string()
        {
            string szoveg = "";
             while (szoveg.Length < 1)
            {
                Console.Write("Adj meg egy szöveget: ");
                szoveg = Console.ReadLine();
            }
            return szoveg;
        }

        static int hibakezelt_int()
        {
            //int ellenőrző
            bool ok = false;
            int szam = 0;
            do
            {
                Console.Write("Adj meg egy egész számot: ");
                ok = int.TryParse(Console.ReadLine(), out szam);
                if (!ok)
                {
                    Console.WriteLine("Kérlek egész számot adj meg!!! (ENTER...)");
                    Console.ReadLine();
                }
            } while (!ok);
            return szam;
        }

        static string hibakezelt_kategoria()
        {
            string szoveg = "";
            string[] kategoriak = {"cim", "megjelenes", "hossz", "gyarto", "mufajok", "szereplok", "forgalmazo" };
            foreach(string kategoria in kategoriak)
            {
                Console.Write(kategoria + " ");
            }
            Console.WriteLine();
            //itt valami nem jó mert mindig újrakéri
            while (!kategoriak.Contains(szoveg))
            {
                Console.Write("Adj meg egy kategóriát: ");
                szoveg = Console.ReadLine().ToLower();
            }
            return szoveg;
        }

        static string[] hibakezelt_string_tomb()
        {
            Console.WriteLine("Max 255 elemet adhatsz meg!");
            string[] tomb = new string[255];
            bool megegy = true;
            byte db = 0;
            while(megegy)
            {
                tomb[db++] = hibakezelt_string();
                Console.Write("Szeretnél mégegy elemet megadni? (i/n)");
                if(!(Console.ReadLine() == "i"))
                {
                    megegy = false;
                }
            }
            Array.Resize(ref tomb, db);
            return tomb;
        }

        static void filmadat_valtoztatas(ref film[] filmek)
        {
            int filmindex = filmvalaszto(filmek);
            filmlistakiirato(filmek[filmindex], "Add meg melyik adatát szeretnéd megváltoztatni: (Enter...)");

            string megvaltoztatando = hibakezelt_kategoria();
            Console.Clear();

            switch (megvaltoztatando)
            {
                case "cim":
                    filmek[filmindex].cim = hibakezelt_string();
                    break;
                case "megjelenes":
                    filmek[filmindex].megjelenes = hibakezelt_int();
                    break;
                case "hossz":
                    filmek[filmindex].hossz = hibakezelt_int();
                    break;
                case "gyarto":
                    filmek[filmindex].gyarto = hibakezelt_string(); ;
                    break;
                case "mufajok":
                    filmek[filmindex].mufajok = hibakezelt_string_tomb();
                    break;
                case "szereplok":
                    filmek[filmindex].szereplok = hibakezelt_string_tomb();
                    break;
                case "forgalmazo":
                    filmek[filmindex].forgalmazo = hibakezelt_string(); ;
                    break;
            }
                    
                
            FileStream fs = new FileStream("../../../../filmek.txt", FileMode.Create);
            StreamWriter w = new StreamWriter(fs);
            foreach(film film in filmek)
            {
                w.Write($"{film.cim};{film.megjelenes};{film.hossz};{film.gyarto};");
                for(byte i = 0; i < film.mufajok.Length; i++)
                {
                    w.Write($"{film.mufajok[i]}");
                    if(!(i == film.mufajok.Length - 1))
                    {
                        w.Write('#');
                    }
                }
                w.Write(";");
                for (byte i = 0; i < film.szereplok.Length; i++)
                {
                    w.Write($"{film.szereplok[i]}");
                    if (!(i == film.szereplok.Length - 1))
                    {
                        w.Write('#');
                    }
                }
                w.Write(";");
                w.Write($"{film.forgalmazo}\n");
            }

            w.Close();
            fs.Close();
            Console.WriteLine("Sikeres megváltoztatás! (Enter...)");
            Console.ReadLine();
            
            //cím;megjelenés(év);hossz(perc);gyártó;műfjakok(#-el elválasztva);szereplők(#-el elválasztva);forgalmazó;
        }
        static void Main(string[] args)
        {
            //üdvözlő keret
            Console.BackgroundColor = ConsoleColor.Blue;
            string szoveg = "Üdv a Filmes beadandó programomban!";
            string szoveg2 = "Nyomd meg az ENTERT a folytatáshoz..."; 
            keret((byte)(Console.WindowWidth / 4), (byte)(Console.WindowHeight / 4), (byte)(Console.WindowWidth/2), (byte)(Console.WindowHeight/2));
            szovegkiiratas((byte)(Console.WindowWidth / 4), (byte)(Console.WindowHeight / 4), (byte)(Console.WindowWidth / 2 - szoveg.Length/4), (byte)(Console.WindowHeight / 2), szoveg);
            szovegkiiratas((byte)(Console.WindowWidth / 4), (byte)(Console.WindowHeight / 4+2), (byte)(Console.WindowWidth / 2 - szoveg2.Length / 4), (byte)(Console.WindowHeight / 2), szoveg2);
            Console.ReadLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();



            //filmek beolvasása
            film[] filmek = beolvaso();

            //A Fő program
            bool fut = true;
            while (fut)
            {
                
                byte menupont = menu();
                switch (menupont)
                {
                    case 1:
                        filmlistakiirato(filmek[filmvalaszto(filmek)], "Nyomd meg az Entert a kilépéshez...");
                        break;
                    case 2:
                        Array.Resize(ref filmek, filmek.Length+1);
                        filmek[filmek.Length - 1] = filmfeltoltes(filmek[filmek.Length - 1]);
                        break;

                    case 3:
                        filmadat_valtoztatas(ref filmek);
                        //Filmek adatai változtatása, még nincs meg
                        break;

                    case 4:
                        //Filmek keresése, még nincs meg
                        break;

                    case 5:
                        //Kilépés
                        break;
                }
                Console.Clear();
                Console.Write("\nSzeretnél megint választani? (i/n)");
                if ((Console.ReadLine().ToLower()) != "i")
                    fut = false;
            }

            //elköszönés
            string szoveg3 = "Itt a program vége";
            Console.BackgroundColor= ConsoleColor.DarkYellow;
            keret((byte)(Console.WindowWidth / 4), (byte)(Console.WindowHeight / 4), (byte)(Console.WindowWidth / 2), (byte)(Console.WindowHeight / 2));
            szovegkiiratas((byte)(Console.WindowWidth / 4), (byte)(Console.WindowHeight / 4), (byte)(Console.WindowWidth / 2 - szoveg3.Length / 4), (byte)(Console.WindowHeight / 2), szoveg3);
            szovegkiiratas((byte)(Console.WindowWidth / 4), (byte)(Console.WindowHeight / 4 + 2), (byte)(Console.WindowWidth / 2 - szoveg2.Length / 4), (byte)(Console.WindowHeight / 2), szoveg2);
            Console.ReadLine();   
            //tombkiiratas(x, y, hossz, magassag, tomb);
            //szovegkiiratas(x, y, hossz, magassag, szoveg);
        }
    }
}