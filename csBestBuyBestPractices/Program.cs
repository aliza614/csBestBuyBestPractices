using System;
using System.Data;
using System.IO;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace csBestBuyBestPractices
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connString = config.GetConnectionString("DefaultConnection");

            IDbConnection conn = new MySqlConnection(connString);
            var repo = new DapperDepartmentRepository(conn);

            Console.WriteLine("Type a new Department name");
            var newDepartment=Console.ReadLine();
            repo.InsertDepartment(newDepartment);

            var departments = repo.GetAllDepartments();

            foreach(var dept in departments)
            {
                Console.WriteLine(dept.Name);
            }

            //products part starts here
           var productRepo=new DapperProductRepository(conn);

            Console.WriteLine("Type a new Product name:");
            var name=Console.ReadLine();
            Console.WriteLine("Type a new Product price: ");
            var price = double.Parse(Console.ReadLine());
            Console.WriteLine("Type a new Product categoryID (1-10): ");
            var categoryID=int.Parse(Console.ReadLine());

            productRepo.CreateProduct(name, price, categoryID);

            var products=productRepo.GetAllProducts();

            foreach (var product in products)
            {
                Console.WriteLine(product.toString());
            }

            //delete a product

            
            Console.WriteLine("Type a Product ID to delete:");
            var productIDDelete = int.Parse(Console.ReadLine());
            
            productRepo.DeleteProduct(productIDDelete);
            var productsDelete = productRepo.GetAllProducts();
            foreach(var product in productsDelete)
            {
                Console.WriteLine(product.toString());
            }



        }

    }
}
