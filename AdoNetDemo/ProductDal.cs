using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetDemo
{
    internal class ProductDal
    {   
        //DataTable daha çok enerji ister/yük bindirir.
        //@ tamamen string olarak kaydet
        //initial catalog = hangi katalog
        SqlConnection _connection = new SqlConnection(@"server=(localdb)\MSSQLLocalDB;initial catalog= ETrade;integrated security=true");

        
        
        public List<Product> GetAll() 
        {
            
            

            //Sql server'a bağlanmamız lazım ancak zaten bağlı olup olmadığını kontrol ettirmemiz lazımdır.
          
            if (_connection.State==ConnectionState.Closed)
            {
                _connection.Open();
            }
            SqlCommand command = new SqlCommand("Select * from Products",_connection);

            SqlDataReader reader = command.ExecuteReader();
            
            List<Product> products = new List<Product>();

            while (reader.Read())
            {
                Product prodcut = new Product
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = Convert.ToString(reader["Name"]),
                    StockAmount = Convert.ToInt32(reader["StockAmount"]),
                    UnitPrice = Convert.ToDecimal(reader["UnitPrice"])
                };
                products.Add(prodcut);
                
            }
            reader.Close();
            _connection.Close();
            return products;
        }

        //Ekleme Operasyonu
        public void Add(Product product)
        {
            //Sql string ile yazmak burada saldırılara açık olmayı kolaylaştırır.
            ConnectionControl();
            SqlCommand command = new SqlCommand(
                "Insert into Products values(@name,@unitPrice,@stockAmount)", _connection);
            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@unitPrice", product.UnitPrice);
            command.Parameters.AddWithValue("@stockAmount", product.StockAmount);
            command.ExecuteNonQuery();

            _connection.Close();
        }

        //Güncelleme Operasyonu
        public void Update(Product product)
        {
            //Where cmd olarak eklenmezse tüm productslara zarar verebilir.
            ConnectionControl();
            SqlCommand command = new SqlCommand(
                "Update Products set Name= @name, UnitPrice= @unitPrice, StockAmount= @stockAmount where Id= @id", _connection);
            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@unitPrice", product.UnitPrice);
            command.Parameters.AddWithValue("@stockAmount", product.StockAmount);
            command.Parameters.AddWithValue("@id", product.Id);
            command.ExecuteNonQuery();

            _connection.Close();
        }

        public DataTable GetAll2()
        {
            //@ tamamen string olarak kaydet
            //initial catalog = hangi katalog

            //Sql server'a bağlanmamız lazım ancak zaten bağlı olup olmadığını kontrol ettirmemiz lazımdır.
            
            ConnectionControl();
            SqlCommand command = new SqlCommand("Select * from Products", _connection);

            SqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            reader.Close();
            _connection.Close();
            return dataTable;
        }

        private void ConnectionControl()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }
    }
}
