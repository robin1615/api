using ProductAPI.Models;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Xml.Linq;
using System.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;

namespace ProductAPI.Controllers
{
    [Route("api/product")]
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("post-data")]
        public ResponseModel InsertProduct([FromBody] ProductModel products)
        {                   

            //int product_id = 0;
            //var constring = ConfigurationManager.ConnectionStrings["Connection_Local"].ConnectionString.ToString();
            string constring = _configuration.GetConnectionString("Connection_Local");
            ResponseModel resposne = new ResponseModel();
            using (SqlConnection connection = new SqlConnection(constring))
            {
               
                try
                {
                    SqlCommand command = new SqlCommand("pcreateorupdate", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@product_id", products.Product_id);
                    command.Parameters.AddWithValue("@product_name", products.Product_name);
                    command.Parameters.AddWithValue("@product_category", products.Product_category);
                    command.Parameters.AddWithValue("@product_freshness", products.Product_freshness);
                    command.Parameters.AddWithValue("@price", products.Price);
                    command.Parameters.AddWithValue("@comment", products.Comment);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    resposne.response = 1;
                    resposne.message = "Success";

                } catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    resposne.response = 0;
                    resposne.message = "Something went wrong";
                }
          
            }
            return resposne;
        }





        [HttpGet("getbyid/{product_id}")]
        public ResponseModel GetproductById(int product_id)
        {
            //string constring = ConfigurationManager.ConnectionStrings["Connection_Local"].ConnectionString.ToString();
            string constring = _configuration.GetConnectionString("Connection_Local");
            ResponseModel resposne = new ResponseModel();
            List<ProductModel> product_list = new List<ProductModel>();
            using (SqlConnection Connection = new SqlConnection(constring))
            {
                try
                {
                    SqlCommand command = Connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "pViewById";
                    command.Parameters.AddWithValue("@product_id", product_id);
                    SqlDataAdapter sqlda = new SqlDataAdapter(command);
                    DataTable dtproducts = new DataTable();
                    Connection.Open();
                    sqlda.Fill(dtproducts);
                    Connection.Close();

                    foreach (DataRow dr in dtproducts.Rows)
                    {
                        product_list.Add(new ProductModel
                        {
                            Product_id = Convert.ToInt32(dr["Product_id"]),
                            Product_name = dr["Product_name"].ToString(),
                            Price = (float)(dr["Price"]),
                            Product_category = dr["Product_Category"].ToString(),
                            Product_freshness = dr["Product_freshness"].ToString(),
                            Comment = dr["Comment"].ToString()

                        }
                        );
                    }

                    resposne.response = 1;
                    resposne.message = "Success";
                    resposne.data = product_list;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                    resposne.response = 0;
                    resposne.message = "Something went wrong";
                }

            }
            return resposne;
        }

            [HttpGet("getprodcuts")]
          
            public ResponseModel GetAllProducts()
            {
            //string constring = ConfigurationManager.ConnectionStrings["Connection_Local"].ConnectionString.ToString();
            string constring = _configuration.GetConnectionString("Connection_Local");
            List<ProductModel> product_list = new List<ProductModel>();
            ResponseModel resposne = new ResponseModel();
            using (SqlConnection Connection = new SqlConnection(constring))
                {
                try
                {
                    SqlCommand command = Connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "pviewAll";
                    SqlDataAdapter sqlda = new SqlDataAdapter(command);
                    DataTable dtproducts = new DataTable();
                    Connection.Open();
                    sqlda.Fill(dtproducts);
                    Connection.Close();

                    foreach (DataRow dr in dtproducts.Rows)
                    {
                        product_list.Add(new ProductModel
                        {
                            Product_id = Convert.ToInt32(dr["Product_id"]),
                            Product_name = dr["Product_name"].ToString(),
                            Price = (float)(dr["Price"]),
                            Product_category = dr["Product_Category"].ToString(),
                            Product_freshness = dr["Product_freshness"].ToString(),
                            Comment = dr["Comment"].ToString()
                        });

                    }
                    resposne.response = 1;
                    resposne.message = "Success";
                    resposne.data = product_list;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                    resposne.response = 0;
                    resposne.message = "Something went wrong";
                }
                }

            return resposne;

            }



        [HttpPut("deleteproduct/{product_id}")]

        public ResponseModel DeleteProduct(int product_id)
        {
       
            ResponseModel resposne = new ResponseModel();
            //string constring = ConfigurationManager.ConnectionStrings["Connection_Local"].ToString();
            string constring = _configuration.GetConnectionString("Connection_Local");
            using (SqlConnection connection = new SqlConnection(constring))
            {
                try
                {
                    SqlCommand command = new SqlCommand("pDeleteById", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@product_id", product_id);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    resposne.response = 1;
                    resposne.message = "Success";
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                    resposne.response = 0;
                    resposne.message = "Something went wrong";
                }
            }
            return resposne;
        }
    }
}
    /*public class Get_Data
    {

        public List<ProductModel> GetAllProducts()
        {
            string constring = ConfigurationManager.ConnectionStrings["Connection_Local"].ToString();
            List<ProductModel> product_list = new List<ProductModel>();
            using (SqlConnection Connection = new SqlConnection(constring))
            {
                SqlCommand command = Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "pviewAll";
                SqlDataAdapter sqlda = new SqlDataAdapter(command);
                DataTable dtproducts = new DataTable();
                Connection.Open();
                sqlda.Fill(dtproducts);
                Connection.Close();

                foreach (DataRow dr in dtproducts.Rows)
                {
                    product_list.Add(new ProductModel
                    {
                        Product_id = Convert.ToInt32(dr["Product_id"]),
                        Product_name = dr["Product_name"].ToString(),
                        Price = (float)(dr["Price"]),
                        Product_category = dr["Product_Category"].ToString(),
                        Product_freshness = dr["Product_freshness"].ToString(),
                        Comment = dr["Comment"].ToString()
                    });

                }
            }

            return product_list;

        }

        public bool InsertProduct(ProductModel products)
        {
            int id = 0;
            string constring = ConfigurationManager.ConnectionStrings["Connection_Local"].ToString();
            using (SqlConnection connection = new SqlConnection(constring))
            {
                SqlCommand command = new SqlCommand("pcreateorupdate", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@product_name", products.Product_name);
                command.Parameters.AddWithValue("@product_category", products.Product_category);
                command.Parameters.AddWithValue("@product_freshness", products.Product_freshness);
                command.Parameters.AddWithValue("@price", products.Price);
                command.Parameters.AddWithValue("@comment", products.Comment);
                connection.Open();
                id = command.ExecuteNonQuery();
                connection.Close();
                return id > 0;
            }
        }

        public List<ProductModel> GetproductById(int product_id)
        {
            string constring = ConfigurationManager.ConnectionStrings["Connection_Local"].ToString();
            List<ProductModel> product_list = new List<ProductModel>();
            using (SqlConnection Connection = new SqlConnection(constring))
            {
                SqlCommand command = Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "pViewById";
                command.Parameters.AddWithValue("@product_id", product_id);
                SqlDataAdapter sqlda = new SqlDataAdapter(command);
                DataTable dtproducts = new DataTable();
                Connection.Open();
                sqlda.Fill(dtproducts);
                Connection.Close();


                foreach (DataRow dr in dtproducts.Rows)
                {
                    product_list.Add(new ProductModel
                    {
                        Product_id = Convert.ToInt32(dr["Product_id"]),
                        Product_name = dr["Product_name"].ToString(),
                        Price = (float)(dr["Price"]),
                        Product_category = dr["Product_Category"].ToString(),
                        Product_freshness = dr["Product_freshness"].ToString(),
                        Comment = dr["Comment"].ToString()

                    });
                    

                }

                return product_list;
            }

             bool UpdateProduct(ProductModel products)
            {
                int i = 0;
                string constring = ConfigurationManager.ConnectionStrings["Connection_Local"].ToString();
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    SqlCommand command = new SqlCommand("pcreateorupdate", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@product_id", products.Product_id);
                    command.Parameters.AddWithValue("@product_name", products.Product_name);
                    command.Parameters.AddWithValue("@product_category", products.Product_category);
                    command.Parameters.AddWithValue("@product_freshness", products.Product_freshness);
                    command.Parameters.AddWithValue("@price", products.Price);
                    command.Parameters.AddWithValue("@comment", products.Comment);
                    connection.Open();
                    i = command.ExecuteNonQuery();
                    connection.Close();
                    return i > 0;
                }
            }

        }

        public string DeleteProduct(int product_id)
        {
            string result = "";
            string constring = ConfigurationManager.ConnectionStrings["Connection_Local"].ToString();
            using (SqlConnection connection = new SqlConnection(constring))
            {
                SqlCommand command = new SqlCommand("pDeleteById", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@product_id", product_id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

                return result;


        }
    }
}*/







