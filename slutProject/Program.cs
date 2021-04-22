using System;
using System.IO;

namespace sänkaSkepp
{
    class Program
    {
        static int kartBredd = 6;
        static int kartHöjd = 4;
        static string[,] spelarensKarta = new string[kartBredd, kartHöjd];
        static string[,] datornsKarta = new string[kartBredd, kartHöjd];
        static bool[,] spelarensSkott = new bool[kartBredd, kartHöjd];
        static bool[,] datornsSkott = new bool[kartBredd, kartHöjd];

        static Random slump = new Random();

        static string[] spelInformation = new string[4];

        static string filnamn = "centralbord.csv";



        static void Main(string[] args)
        {


            if (File.Exists(filnamn))
            {
                // Läs in alla rader, dvs bordsbokningar
                spelInformation = File.ReadAllLines(filnamn);
                Console.WriteLine("spelinformation lästes in från fil");
            }
            else
            {
                // Skapa en tom bordslista i arrayen
                // Lagra detta i filen
                // 0. förra vinnare, 1. antalet skepp, 2. x-led, 3.y-led
                spelInformation[0] = "Ingen vinnare än";
                spelInformation[1] = "0";
                spelInformation[2] = "0";
                spelInformation[3] = "0";


                // Lagra i filen
                File.WriteAllLines(filnamn, spelInformation);
                Console.WriteLine("Fil med spelinformation saknas, ny fil skapades");
            }
            int menyVal = 0;
            while (menyVal != 4)
            {
                Console.WriteLine("1. Spela sänka skepp");
                Console.WriteLine("2. Se senaste vinnare");
                Console.WriteLine("3. inställningar");
                Console.WriteLine("4. Avsluta");
                menyVal = ReadInt(4);

                switch (menyVal)
                {
                    case 1:
                        SkapaKartorna();
                        PlaceraSkepp();
                        SpelaSänkaSkepp();
                        break;
                    case 2:
                        Console.WriteLine($"Senaste Vinnare Var {spelInformation[0]} ");
                        break;
                    case 3:

                        break;
                    case 4:
                        Console.WriteLine("Hejdå");
                        break;

                    default:

                        break;
                }
            }

        }

        /// <summary>
        /// Fyller spelarens och datorns kartor med tomma rutor
        /// </summary>
        static void SkapaKartorna()
        {
            for (int y = 0; y < kartHöjd; y++)
            {
                for (int x = 0; x < kartBredd; x++)
                {
                    spelarensKarta[x, y] = "O";
                    datornsKarta[x, y] = "O";
                    spelarensSkott[x, y] = false;
                    datornsSkott[x, y] = false;
                }
            }
        }

        /// <summary>
        /// Placerar X på några av spelarens och datorns platser
        /// </summary>
        static void PlaceraSkepp()
        {
            spelarensKarta[1, 3] = "X";
            spelarensKarta[2, 0] = "X";
            datornsKarta[3, 2] = "X";
            datornsKarta[1, 3] = "X";
        }

        /// <summary>
        /// Spelets huvudloop
        /// </summary>
        static void SpelaSänkaSkepp()
        {
            bool harNågonVunnit = false;
            while (harNågonVunnit == false)
            {
                Console.Clear();
                RitaSpelplanen();
                Console.WriteLine("Var vill du skjuta? (X)");
                int x = ReadInt(6);
                Console.WriteLine("Var vill du skjuta? (Y)");
                int y = ReadInt(4);
                spelarensSkott[x - 1, y - 1] = true;
                datornsSkott[slump.Next(kartBredd), slump.Next(kartHöjd)] = true;

                if (HarSpelarenVunnit())
                {
                    Console.Clear();
                    RitaSpelplanen();
                    Console.WriteLine("Spelaren vann");
                    spelInformation[0] = "Spelaren";
                    File.WriteAllLines(filnamn, spelInformation);
                    Console.ReadKey();
                    harNågonVunnit = true;
                }
                else if (HarDatornVunnit())
                {
                    Console.Clear();
                    RitaSpelplanen();
                    Console.WriteLine("Datorn vann");
                    spelInformation[0] = "Datorn";
                    File.WriteAllLines(filnamn, spelInformation);
                    Console.ReadKey();
                    harNågonVunnit = true;

                }
            }
        }


        /// <summary>
        /// Ritar ut spelarens och datorns kartor
        /// </summary>
        static void RitaSpelplanen()
        {
            Console.WriteLine("Spelarens karta");
            for (int y = 0; y < kartHöjd; y++)
            {
                for (int x = 0; x < kartBredd; x++)
                {
                    if (datornsSkott[x, y] == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write(spelarensKarta[x, y]);

                    // Återställ färgen till grå
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.WriteLine();
            }

            Console.WriteLine();

            // Rita datorns karta
            Console.WriteLine("Datorns karta");
            for (int y = 0; y < kartHöjd; y++)
            {
                for (int x = 0; x < kartBredd; x++)
                {
                    // Endast rutor som spelaren har skjutit på syns
                    if (spelarensSkott[x, y] == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(datornsKarta[x, y]);
                    }
                    else
                    {
                        Console.Write("-");
                    }


                    // Återställ färgen till grå
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Kollat om spelaren har vunnit
        /// </summary>
        /// <returns></returns>
        static bool HarSpelarenVunnit()
        {
            for (int y = 0; y < kartHöjd; y++)
            {
                for (int x = 0; x < kartBredd; x++)
                {
                    if (datornsKarta[x, y] == "X" && spelarensSkott[x, y] == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Kollar om datorn har vunnit
        /// </summary>
        /// <returns></returns>
        static bool HarDatornVunnit()
        {
            for (int y = 0; y < kartHöjd; y++)
            {
                for (int x = 0; x < kartBredd; x++)
                {
                    if (spelarensKarta[x, y] == "X" && datornsSkott[x, y] == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Läser in ett heltal från användaren
        /// </summary>
        /// <returns>Användarens heltal</returns>
        static int ReadInt(int maxInt)
        {
            int heltal;
            while (!int.TryParse(Console.ReadLine(), out heltal) || heltal < 1 || heltal > maxInt)
            {
                Console.WriteLine("Du skrev inte in ett heltal eller så var talet inte giltigt. Försök igen.");
            }
            return heltal;
        }
    }
}
