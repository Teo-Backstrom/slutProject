using System;

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

        static void Main(string[] args)
        {
            SkapaKartorna();
            PlaceraSkepp();
            SpelaSänkaSkepp();
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
                int x = ReadInt();
                Console.WriteLine("Var vill du skjuta? (Y)");
                int y = ReadInt();
                spelarensSkott[x - 1, y - 1] = true;
                datornsSkott[slump.Next(kartBredd), slump.Next(kartHöjd)] = true;

                if (HarSpelarenVunnit())
                {
                    Console.Clear();
                    RitaSpelplanen();
                    Console.WriteLine("Spelaren vann");
                    Console.ReadKey();
                    harNågonVunnit = true;
                }
                else if (HarDatornVunnit())
                {
                    Console.Clear();
                    RitaSpelplanen();
                    Console.WriteLine("Datorn vann");
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
        static int ReadInt()
        {
            int heltal;
            while (int.TryParse(Console.ReadLine(), out heltal) == false)
            {
                Console.WriteLine("Du skrev inte in ett heltal. Försök igen.");
            }
            return heltal;
        }
    }
}
