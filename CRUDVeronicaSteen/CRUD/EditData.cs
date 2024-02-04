using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;


namespace VeronicaSteenInlämning
{
    public class EditData
    {
        public static (string customerId, string name) GetCustomerInformation()
        {
            Console.WriteLine("Ange ID: ");
            string customerId = Console.ReadLine();
            Console.WriteLine("Ange företagets namn: ");
            string name = Console.ReadLine();

            return (customerId, name);
        }
        public static string AddCustomer(string connectionString)
        {
            (string customerId, string name) = GetCustomerInformation();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string addCustomerQuery = "INSERT INTO Customers (CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region," +
                    "PostalCode, Country, Phone, Fax) " +
                    "VALUES (@CustomerId, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, " +
                    "@Country, @Phone, @Fax)";

                using (SqlCommand command = new SqlCommand(addCustomerQuery, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerId);
                    command.Parameters.AddWithValue("@CompanyName", name);
                    command.Parameters.AddWithValue("@ContactName", "Veronica Steen");
                    command.Parameters.AddWithValue("@ContactTitle", "Owner");
                    command.Parameters.AddWithValue("@Address", "Solrosvägen 2");
                    command.Parameters.AddWithValue("@City", "Trollhättan");
                    command.Parameters.AddWithValue("@Region", "Västra Götaland");
                    command.Parameters.AddWithValue("@PostalCode", "46143");
                    command.Parameters.AddWithValue("@Country", "Sverige");
                    command.Parameters.AddWithValue("@Phone", "0520-12345");
                    command.Parameters.AddWithValue("@Fax", "(1)111-2");

                    command.ExecuteNonQuery();
                    return customerId;
                }

            }
        }
        public static void UpdateCustomerAddress(string connectionString)
        {
            Console.WriteLine("Ange CustomerID för den kund du vill ändra adress för: ");
            string customerId = Console.ReadLine();

            Console.WriteLine("Ange den nya addressen: ");
            string newAddress = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Customers SET Address = @newAddress " +
                    "WHERE CustomerID = @customerID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@newAddress", newAddress);
                    command.Parameters.AddWithValue("@CustomerID", customerId);
                    command.ExecuteNonQuery();
                }
            }

        }
        public static void ShowCountrySales(string connectionString)
        {
            Console.WriteLine("Välj ett land att visa ordervärde för: ");
            string country = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT E.FirstName + ' ' + E.LastName AS SalesPerson,
                SUM(od.Quantity * od.UnitPrice) AS TotalSales
                FROM Orders o
                JOIN Employees e ON o.EmployeeID = e.EmployeeID
                JOIN Customers c ON o.CustomerID = c.CustomerID
                JOIN[Order Details] od ON o.OrderID = od.OrderID
                WHERE C.Country = @Country
                GROUP BY E.FirstName, E.LastName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Country", country);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine($"{"Salesperson", -18} {"Totalsales", 16}");
                        while (reader.Read())
                        {
                            string salespersonName = reader["SalesPerson"].ToString();
                            decimal totalSales = (decimal)reader["TotalSales"];

                            Console.WriteLine($"{salespersonName,-18} {totalSales, 15}");
                            Console.WriteLine();
                        }
                    }
                }
            }

        }
        public static void AddOrderForNewCustomer(string connectionString)
        {
            string customerId = AddCustomer(connectionString);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertOrderQuery = "INSERT INTO Orders (CustomerID, OrderDate) " +
                "VALUES (@CustomerID, @OrderDate); SELECT SCOPE_IDENTITY();";

                using (SqlCommand insertOrderCommand = new SqlCommand(insertOrderQuery, connection))
                {
                    insertOrderCommand.Parameters.AddWithValue("@CustomerID", customerId);
                    insertOrderCommand.Parameters.AddWithValue("@OrderDate", DateTime.Now);

                    int orderId = Convert.ToInt32(insertOrderCommand.ExecuteScalar()); // Hämta OrderID
                    int productId = 55;
                    int quantity = 1;

                    // Lägg till produkten i den nya ordern
                    string insertOrderDetailsQuery = "INSERT INTO [Order Details] (OrderID, ProductID, Quantity) " +
                        "VALUES (@OrderId, @productId, @Quantity);";

                    using (SqlCommand insertOrderDetailsCommand = new SqlCommand(insertOrderDetailsQuery, connection))
                    {
                        insertOrderDetailsCommand.Parameters.AddWithValue("@OrderId", orderId);
                        insertOrderDetailsCommand.Parameters.AddWithValue("@ProductId", productId);
                        insertOrderDetailsCommand.Parameters.AddWithValue("@Quantity", quantity);
                        insertOrderDetailsCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    } 
}

