using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;

namespace csBestBuyBestPractices
{
    public class DapperProductRepository : IProductRepository
    {
        private readonly IDbConnection _connection;

        public DapperProductRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public void CreateProduct(string name, double price, int categoryID)
        {
            _connection.Execute("INSERT INTO products (name,price, categoryID) values(@productName, @productPrice, @productCategoryID)",
                new { productName = name, productPrice = price, productCategoryID = categoryID });
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _connection.Query<Product>("SELECT * FROM products;");
        }
        public IEnumerable<int> GetProductID(string name, double price, int categoryID)
        {
            return _connection.Query<int>("SELECT distinct productID FROM products WHERE name=@productName, price=@productPrice, categoryID=@categoryID;", new { productName = name, productPrice = price, productCategoryID = categoryID });
            

        }

        public void DeleteProduct(string name, double price, int categoryID)
        {
            IEnumerable<int> productIDs=GetProductID(name, price, categoryID);
            foreach (var productID in productIDs)
                DeleteProduct(productID);
        }
        public void DeleteProduct(int productID)
        {
                _connection.Execute("DELETE from sales where productID=@myProductID;" +
                                    "DELETE FROM reviews WHERE productID=@myProductID;" +
                                    "DELETE FROM products WHERE productID=@myProductID;"
                                    , new { myProductID = productID+"" });


        }
    }
}
