using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeronicaSteenInlämning
{
    public class DeleteData
    {
        public static void DeleteByID(string connectionString, string customerId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string deleteOrderDetailsQuery = "DELETE FROM [Order Details] " +
                    "WHERE OrderID IN " +
                    "(SELECT OrderID FROM Orders " +
                    "WHERE CustomerID = @CustomerId)";

                using (SqlCommand deleteOrderDetailsCommand = new SqlCommand(deleteOrderDetailsQuery, connection))
                {
                    deleteOrderDetailsCommand.Parameters.AddWithValue("@CustomerId", customerId);
                    deleteOrderDetailsCommand.ExecuteNonQuery();
                }
                // Ta bort ordrar för den specifika kunden
                string deleteOrdersQuery = "DELETE FROM Orders WHERE CustomerID = @CustomerId";
                using (SqlCommand deleteOrdersCommand = new SqlCommand(deleteOrdersQuery, connection))
                {
                    deleteOrdersCommand.Parameters.AddWithValue("@CustomerId", customerId);
                    deleteOrdersCommand.ExecuteNonQuery();
                }

                // Ta bort kunden
                string deleteCustomerQuery = "DELETE FROM Customers WHERE CustomerID = @CustomerId";
                using (SqlCommand deleteCustomerCommand = new SqlCommand(deleteCustomerQuery, connection))
                {
                    deleteCustomerCommand.Parameters.AddWithValue("@CustomerId", customerId);
                    deleteCustomerCommand.ExecuteNonQuery();
                }
            }
        }
        public static void DeleteByName(string connectionString, string companyName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string deleteOrderDetailsQuery = "DELETE FROM [Order Details] " +
                    "WHERE OrderID IN " +
                    "(SELECT OrderID FROM Orders " +
                    "WHERE CustomerID = " + 
                    "(SELECT CustomerID FROM Customers WHERE CompanyName = @companyName))";

                using (SqlCommand deleteOrderDetailsCommand = new SqlCommand(deleteOrderDetailsQuery, connection))
                {
                    deleteOrderDetailsCommand.Parameters.AddWithValue("@CompanyName", companyName);
                    deleteOrderDetailsCommand.ExecuteNonQuery();
                }

                string deleteOrdersQuery = "DELETE FROM Orders" +
                    " WHERE CustomerID = " +
                    "(SELECT CustomerId " +
                    "FROM Customers" +
                    " WHERE CompanyName = @companyName)";

                using (SqlCommand deleteOrderCommand = new SqlCommand(deleteOrdersQuery, connection))
                {
                    deleteOrderCommand.Parameters.AddWithValue("@CompanyName", companyName);
                    deleteOrderCommand.ExecuteNonQuery();
                }

                string deleteCustomerNameCommand = "DELETE FROM Customers WHERE CompanyName = @companyName";

                using (SqlCommand command = new SqlCommand(deleteCustomerNameCommand, connection))
                {
                    command.Parameters.AddWithValue("@companyName", companyName);
                    command.ExecuteNonQuery();
                }

            }
        }

      
    }
}
