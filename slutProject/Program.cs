
using System;
using System.IO;

namespace sänkaSkepp
{

    class Program
    {
        static int kartBredd = 100;
        static int kartHöjd = 100;

        //skapar kartor
        static string[,] spelarensKarta = new string[kartBredd, kartHöjd];
        static string[,] datornsKarta = new string[kartBredd, kartHöjd];
        static bool[,] spelarensSkott = new bool[kartBredd, kartHöjd];
        static bool[,] datornsSkott = new bool[kartBredd, kartHöjd];

        //slumpmaskin
        static Random slump = new Random();

        //skapar array för fillagring
        static string[] spelInformation = new string[4];

        //namn på fil för att används i flera funktioner
        static string filnamn = "centralbord.csv";



        static void Main(string[] args)
        {
            //kollar om filen finns annars skapas en fil med standrad inställningar
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
                spelInformation[2] = "6";
                spelInformation[3] = "4";


                // Lagra i filen
                File.WriteAllLines(filnamn, spelInformation);
                Console.WriteLine("Fil med spelinformation saknas, ny fil skapades");
            }

            int menyVal = 0;
            while (menyVal != 4)
            {
                //menyval
                Console.WriteLine("1. Spela sänka skepp");
                Console.WriteLine("2. Se senaste vinnare");
                Console.WriteLine("3. inställningar");
                Console.WriteLine("4. Avsluta");
                //ingen ogiltig inamtning max 4 min 1
                menyVal = ReadInt(4, 1);

                switch (menyVal)
                {
                    case 1:
                        //kör metoder för att spela spelet
                        SkapaKartorna();
                        PlaceraSkepp();
                        SpelaSänkaSkepp();
                        break;
                    case 2:
                        //visar sanaste vinnare
                        Console.WriteLine($"Senaste Vinnare Var {spelInformation[0]}");
                        break;
                    case 3:
                        //låtar användare välja spelinställningar
                        Spelinställning();
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
        /// Låter användare mata in spelplan bred och höjd och antalet båtar
        /// </summary>
        static void Spelinställning()
        {
            //felar upp array till int
            int antal = int.Parse(spelInformation[1]);
            int bredd = int.Parse(spelInformation[2]);
            int höjd = int.Parse(spelInformation[3]);

            //låtar användare mata in ny information
            Console.WriteLine("hur många båtar vill du ha med (Välj ett tal mellan 2-10)");
            antal = ReadInt(10, 2);

            Console.WriteLine("hur bred ska planen vara (Välj ett tal mellan 5-100)");
            bredd = ReadInt(100, 5);

            Console.WriteLine("hur hög ska planen vara (Välj ett tal mellan 4-100)");
            höjd = ReadInt(100, 4);

            //sparar information i array och sedan i fil
            spelInformation[1] = antal.ToString();
            spelInformation[2] = bredd.ToString();
            spelInformation[3] = höjd.ToString();
            File.WriteAllLines(filnamn, spelInformation);

        }

        /// <summary>
        /// Fyller spelarens och datorns kartor med tomma rutor
        /// </summary>
        static void SkapaKartorna()
        {
            //skapar kartor med den angedda värderna
            kartHöjd = int.Parse(spelInformation[3]);
            kartBredd = int.Parse(spelInformation[2]);

            for (int y = 0; y < kartHöjd; y++)
            {
                for (int x = 0; x < kartBredd; x++)
                {
                    //gör bas kartan med bara 0 or och anger träffar till false
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

            int antal = int.Parse(spelInformation[1]);
            //loopar igenom tills det inte finns fler skepp kvar
            for (int i = 0; i < antal; i++)
            {
                bool flagga = false;
                while (flagga == false)
                {
                    //låter användare ange x och y pos för skepp
                    Console.WriteLine($"Vart vill du ställa ut ditt skepp nr{i + 1}");
                    Console.WriteLine($"X-kordinat välj mellan 1-{kartBredd}");
                    Console.WriteLine("PS. kom ihåg att kordinaterana utgår från 4je kvadranten");
                    int x = ReadInt(kartBredd, 1);
                    Console.WriteLine($"Y-kordinat välj mellan 1-{kartHöjd}");
                    Console.WriteLine("PS. kom ihåg att kordinaterana utgår från 4je kvadranten");
                    int y = ReadInt(kartHöjd, 1);
                    //kolla om stället är ledigt
                    if (spelarensKarta[x, y] == "O")
                    {
                        spelarensKarta[x, y] = "X";
                        flagga = true;
                    }
                    else
                    {
                        Console.WriteLine("Finns redan ett skepp på samma ställe, försök igen");
                    }
                }

            }
            //loopar igenom tills det inte finns fler skepp kvar
            for (int i = 0; i < antal; i++)
            {
                bool flagga = false;
                while (flagga == false)
                {
                    //slupar pos för datorns skepp
                    int x = slump.Next(kartBredd);
                    int y = slump.Next(kartHöjd);
                    //kollar om pos är ledig för skeppet annars så slumpas en ny pos fram
                    if (datornsKarta[x, y] == "O")
                    {
                        datornsKarta[x, y] = "X";
                        flagga = true;
                    }
                }

            }
        }

        /// <summary>
        /// Spelets huvudloop
        /// </summary>
        static void SpelaSänkaSkepp()
        {
            //till någon vunnit körs denna kod
            bool harNågonVunnit = false;
            while (harNågonVunnit == false)
            {
                //tar bort gammla spelplan ritar ny spelplan och låtar användare skjuta valfri plats + datorn random plats
                Console.Clear();
                RitaSpelplanen();
                Console.WriteLine("Var vill du skjuta? (X)");
                int x = ReadInt(kartBredd, 1);
                Console.WriteLine("Var vill du skjuta? (Y)");
                int y = ReadInt(kartHöjd, 1);
                spelarensSkott[x - 1, y - 1] = true;
                datornsSkott[slump.Next(kartBredd), slump.Next(kartHöjd)] = true;

                //om spelare vunnit så informeras det och det lagras i fil och slutar matchen efter valfri knapp tryckt
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
                //om datorn vunnit så informeras det och det lagras i fil och slutar matchen efter valfri knapp tryckt
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
        /// Läser in inmatning från användaren
        /// </summary>
        /// <returns>heltal</returns>
        static int ReadInt(int maxInt, int minInt)
        {
            int heltal;
            //kollar så talet är en int och är mellan min och max kravet
            while (!int.TryParse(Console.ReadLine(), out heltal) || heltal < minInt || heltal > maxInt)
            {
                Console.WriteLine("Du skrev inte in ett heltal eller så var talet inte giltigt. Försök igen.");
            }
            return heltal;
        }
    }
}
