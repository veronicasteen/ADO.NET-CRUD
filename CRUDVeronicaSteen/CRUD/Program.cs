using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using VeronicaSteenInlämning;

namespace VeronicasInlämning
{
    public class Program
    {
        public static string connectionString = @"Data Source=;Initial Catalog="
        + "Integrated Security=true;TrustServerCertificate=true;";

        public static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("Välj mellan 1-6, tryck sedan 'ENTER'");
                Console.WriteLine();
                Console.WriteLine("1. Lägg till ny kund");
                Console.WriteLine("2. Ta bort kund");
                Console.WriteLine("3. Uppdatera kunds adress");
                Console.WriteLine("4. Visa ordervärde för land");
                Console.WriteLine("5. Lägg till ny order och ny kund");
                Console.WriteLine("6. Avsluta");

                int userInput = int.Parse(Console.ReadLine());
                Console.Clear();
                switch (userInput)
                {
                    case 1:
                        EditData.AddCustomer(connectionString);
                        Console.Clear();
                        Console.WriteLine("Kunden har lagts till i databasen.");
                        Console.WriteLine();
                        break;
                    
                    case 2:
                        Console.WriteLine("Hur vill du ta bort kunden? Välj mellan 1-2, tryck sedan 'ENTER'");
                        Console.WriteLine("1. Genom CustomerID.");

                        Console.WriteLine("2. Genom CompanyName");
                        int input = int.Parse(Console.ReadLine());
                        if (input == 1)
                        {
                            Console.WriteLine("Ange ID: ");
                            string companyId = Console.ReadLine();
                            DeleteData.DeleteByID(connectionString, companyId);
                            Console.Clear();
                            Console.WriteLine("Kunden har tagits bort");
                            Console.WriteLine();
                        }
                        else if (input == 2)
                        {
                            Console.WriteLine("Ange CompanyName: ");
                            string companyName = Console.ReadLine();
                            DeleteData.DeleteByName(connectionString, companyName);
                            Console.Clear();
                            Console.WriteLine("Kunden har tagits bort.");
                            Console.WriteLine();
                        }
                        break;
                    
                    case 3:
                        EditData.UpdateCustomerAddress(connectionString);
                        Console.Clear();
                        Console.WriteLine("Kundens adress har uppdaterats.");
                        Console.WriteLine();
                        break;
                    
                    case 4:
                        EditData.ShowCountrySales(connectionString);
                        break;
                    
                    case 5:
                        EditData.AddOrderForNewCustomer(connectionString);
                        Console.Clear();
                        Console.WriteLine("Kunden och ordern är skapad.");
                        Console.WriteLine();
                        break;

                    case 6:
                        Console.WriteLine("Hejdå!");
                        running = false;
                        break;
                    
                    default:
                        Console.WriteLine("Ogiltigt val");
                        break;
                }

            }

        }

    }
}
