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
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
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
            film[] filmek = new film[100];

            FileStream fs = new FileStream("filmek.txt", FileMode.Open);
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

        static void filmlistakiirato(film[] filmek)
        {
            for (byte i = 0; i < filmek.Length; i++)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.Write($"{filmek[i].cim}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine($"\nMegjelenés: {filmek[i].megjelenes} ");
                Console.WriteLine($"Hossza: {filmek[i].hossz} perc");
                Console.WriteLine($"Készítette: {filmek[i].gyarto} ");
                Console.WriteLine("\nMűfajok: ");
                for (byte j = 0; j < filmek[i].mufajok.Length; j++)
                {
                    Console.Write(filmek[i].mufajok[j] + ", ");
                    if (j % 5 == 0 && j != 0)
                    {
                        Console.WriteLine();
                    }
                }
                Console.WriteLine("\n\nSzereplők: ");
                for (byte j = 0; j < filmek[i].szereplok.Length; j++)
                {
                    Console.Write(filmek[i].szereplok[j] + ", ");
                    if (j % 3 == 0 && j != 0)
                    {
                        Console.WriteLine();
                    }
                }

                Console.WriteLine($"\n\nKiadta: {filmek[i].forgalmazo}\n\n");
            }
            Console.ReadKey();
        }
        static byte menu()
        {
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

            FileStream fs = new FileStream("filmek.txt", FileMode.Append);
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
        static void Main(string[] args)
        {
            string szoveg = "Üdv a Filmes beadandó programomban!";
            string szoveg2 = "Nyomd meg az ENTERT a folytatáshoz...";
            Console.ReadKey();
            keret((byte)(Console.WindowWidth / 4), (byte)(Console.WindowHeight / 4), (byte)(Console.WindowWidth/2), (byte)(Console.WindowHeight/2));
            szovegkiiratas((byte)(Console.WindowWidth / 4), (byte)(Console.WindowHeight / 4), (byte)(Console.WindowWidth / 2 - szoveg.Length/4), (byte)(Console.WindowHeight / 2), szoveg);
            szovegkiiratas((byte)(Console.WindowWidth / 4), (byte)(Console.WindowHeight / 4+2), (byte)(Console.WindowWidth / 2 - szoveg.Length / 4), (byte)(Console.WindowHeight / 2), szoveg2);
            Console.ReadLine();
            Console.BackgroundColor= ConsoleColor.Black;
            Console.Clear();
            film[] filmek = beolvaso();
            bool fut = true;
            while (fut)
            {
                
                byte menupont = menu();
                switch (menupont)
                {
                    case 1:
                        filmlistakiirato(filmek);
                        break;
                    case 2:
                        Array.Resize(ref filmek, filmek.Length+1);
                        filmek[filmek.Length - 1] = filmfeltoltes(filmek[filmek.Length - 1]);
                        break;

                    case 3:
                        break;

                    case 4:
                        break;

                    case 5:

                        break;
                }
                Console.Clear();
                Console.Write("\nSzeretnél megint választani? (i/n)");
                if ((Console.ReadLine().ToLower()) != "i")
                    fut = false;
            }
            Console.WriteLine("Program vége, köszi hogy használtad, a kilépéshez nyomd meg az ENTERT! ;)");
            Console.ReadLine();   
            //tombkiiratas(x, y, hossz, magassag, tomb);
            //szovegkiiratas(x, y, hossz, magassag, szoveg);
        }
    }
}