﻿namespace beadando
{
    //cím;megjelenés(év);hossz(perc);gyártó;műfjakok(#-el elválasztva);szereplők(#-el elválasztva);forgalmazó
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

        static void Main(string[] args)
        {
            string szoveg = "Üdv a Filmes beadandó programomban!";
            string szoveg2 = "Nyomd meg az Entert a folytatáshoz...";
            Console.ReadKey();
            keret((byte)(Console.WindowWidth / 4), (byte)(Console.WindowHeight / 4), (byte)(Console.WindowWidth/2), (byte)(Console.WindowHeight/2));
            szovegkiiratas((byte)(Console.WindowWidth / 4), (byte)(Console.WindowHeight / 4), (byte)(Console.WindowWidth / 2 - szoveg.Length/4), (byte)(Console.WindowHeight / 2), szoveg);
            szovegkiiratas((byte)(Console.WindowWidth / 4), (byte)(Console.WindowHeight / 4+2), (byte)(Console.WindowWidth / 2 - szoveg.Length / 4), (byte)(Console.WindowHeight / 2), szoveg2);
            Console.ReadLine();
            Console.BackgroundColor= ConsoleColor.Black;
            Console.Clear();
            film[] filmek = beolvaso();
            for (byte i = 0; i< filmek.Length; i++)
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
                    if (j%5 == 0 && j != 0)
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
            
            //tombkiiratas(x, y, hossz, magassag, tomb);
            //szovegkiiratas(x, y, hossz, magassag, szoveg);
        }
    }
}