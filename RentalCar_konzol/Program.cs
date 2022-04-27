using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RentalCar_konzol
{
    class Program
    {
        static List<Auto> autok = new List<Auto>();
        
        static void Main(string[] args)
        {
            
            Console.WriteLine("Autók beolvasása...");
            beolvasas();
            Console.WriteLine();
            Console.WriteLine("1. Listázza ki a legalacsonyabb kölcsönzési díjú autó rendszámát!");
            legalacsonyabb();
            Console.WriteLine();
            Console.WriteLine("2. Jelenítse meg az egyes autók kölcsönzésének bevételeit!");
            bevetelek();
            Console.WriteLine();
            Console.WriteLine("3. Írassa ki, melyik autót nem kölcsönözték ki pontosan 11 napra!");
            kolcsonzes11nap();
            Console.WriteLine();
            Console.WriteLine("Program vége!");
            Console.ReadLine();
        }

        private static void kolcsonzes11nap()
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Server = "localhost";
            sb.Database = "rentalcar";
            sb.UserID = "root";
            sb.Password = "";

            MySqlConnection connection = new MySqlConnection(sb.ConnectionString);

            try
            {
                connection.Open();
                MySqlCommand sql = connection.CreateCommand();
                sql.CommandText = "SELECT `rendszam` FROM `auto` WHERE `id` NOT IN (SELECT `id` FROM `kolcsonzes` JOIN `auto` USING(`id`) WHERE (`ig`-`tol`)=11);";

                using (MySqlDataReader dr = sql.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Console.WriteLine("Soha nem kölcsönözték 11 napra: {0}.", dr.GetString("rendszam"));
                    }
                    if (!dr.Read())
                    {
                        Console.WriteLine("Nincs ilyen autó.");
                    }
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        private static void bevetelek()
        {
            Console.WriteLine("A gépjárművek éves bevétele:");
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Server = "localhost";
            sb.Database = "rentalcar";
            sb.UserID = "root";
            sb.Password = "";

            MySqlConnection connection = new MySqlConnection(sb.ConnectionString);

            try
            {
                connection.Open();
                MySqlCommand sql = connection.CreateCommand();
                sql.CommandText = "SELECT `rendszam`, SUM(`ar`*(`ig`-`tol`)) as osszesen FROM `auto` JOIN `kolcsonzes` USING(`id`) GROUP BY `rendszam`; ";

                using(MySqlDataReader dr = sql.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Console.WriteLine("\t{0} bevétele: \t{1} Ft", dr.GetString("rendszam"), dr.GetInt32("osszesen"));
                    }
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        private static void legalacsonyabb()
        {
            int legalacsonyabb = autok.Min(a => a.Ar);
            Auto auto = autok.Find(a => a.Ar == legalacsonyabb);
            Console.WriteLine($"A legalacsonyabb kölcsönzési áru autó rendszáma: {auto.Rendszam} /{auto.Ar}/");
        }

        private static void beolvasas()
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Server = "localhost";
            sb.Database = "rentalcar";
            sb.UserID = "root";
            sb.Password = "";

            MySqlConnection connection = new MySqlConnection(sb.ConnectionString);
            
            try
            {
                connection.Open();
                MySqlCommand sql = connection.CreateCommand();

                sql.CommandText = "SELECT `id`, `rendszam`, `marka`, `model`, `ar` FROM `auto` WHERE 1;";

                using (MySqlDataReader dr = sql.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Auto uj = new Auto(dr.GetInt32("ar"), dr.GetInt32("id"), dr.GetString("marka"), dr.GetString("model"), dr.GetString("rendszam"));
                        autok.Add(uj);
                    }
                }

                sql.CommandText = "SELECT `id`, `tol`, `ig` FROM `kolcsonzes` WHERE YEAR(tol) = 2021 AND YEAR(ig) = 2021;";

                using (MySqlDataReader dr = sql.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Kolcsonzes uj = new Kolcsonzes(dr.GetDateTime("tol"), dr.GetDateTime("ig"));
                        int id = dr.GetInt32("id");
                        autok.Find(a => a.Id == id).Kolcsonzesek.Add(uj);
                    }
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                Environment.Exit(0); 
            }
        
        }
    }
}
