using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=123456;";

            using var conn = new NpgsqlConnection(connectionString);

            conn.Open();

            ReadCategories(conn); //Print total categories in Categories Table
            DisplayCategories(conn); //Count total categories in Categories Table

            CreateCategory(200, "Car", "Toyoya car", conn); //Crate a new category

            UpdateCategories(20, "Toy", "This is a toy", "Car", conn); //Update Category by category_name ("Car")
 
            DeleteCategories("Toy" ,conn); //Delete Category by category_name


            conn.Close();
        }

        private static void DeleteCategories(
            string category_name,
            NpgsqlConnection conn
            )
        {
            using NpgsqlCommand cmd = new NpgsqlCommand(@"DELETE FROM categories
                                                                         WHERE category_name = @category_name;
                                                                ", conn);
            
            
            cmd.Parameters.AddWithValue("@category_name", category_name);

            var result = cmd.ExecuteNonQuery();
            Console.WriteLine($"Total rows deleted: {result}");
        }

        private static void UpdateCategories(
            int category_id,
            string category_name,
            string description,
            string condition,
            NpgsqlConnection conn
            )
        {
            using NpgsqlCommand cmd = new NpgsqlCommand(@"UPDATE categories
                                                                SET     category_id = @category_id,
                                                                        category_name = @category_name,
                                                                        description = @description
                                                                WHERE   category_name = @condition;"
                                                       , conn);

            cmd.Parameters.AddWithValue("@category_id", category_id);
            cmd.Parameters.AddWithValue("@category_name", category_name);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@condition", condition);

            var result = cmd.ExecuteNonQuery();
            Console.WriteLine($"Total rows updated: {result}");
        }

        private static void CreateCategory(
            int category_id, string category_name, string description,
            NpgsqlConnection conn
            )
        {
            using NpgsqlCommand cmd = new NpgsqlCommand(@"INSERT INTO categories(category_id, category_name, description)
                                                                        VALUES (
                                                                                @category_id,
                                                                                @category_name,
                                                                                @description
                                                                            );",conn
                );

            cmd.Parameters.AddWithValue("@category_id", category_id);
            cmd.Parameters.AddWithValue("@category_name", category_name);
            cmd.Parameters.AddWithValue("@description", description);

            var result = cmd.ExecuteNonQuery();
            Console.WriteLine($"Total rows created: {result}");
        }

        private static void DisplayCategories(NpgsqlConnection conn)
        {
            using NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(*) FROM categories", conn);
            var totalCategoriesRows = cmd.ExecuteScalar();
            Console.WriteLine($"Total rows read: {totalCategoriesRows}");
        }

        private static void ReadCategories(NpgsqlConnection conn)
        {
            using NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM categories;", conn);
            using NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"Category ID: {reader[0]}\t Category name: {reader.GetString(1)} \t Description: {reader.GetString(2)}");
            };
            reader.Close();
        }
    }
}
