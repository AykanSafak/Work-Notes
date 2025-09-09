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
        public DataTable GetAll() 
        {
            //@ tamamen string olarak kaydet
            //initial catalog = hangi katalog

            //Sql server'a bağlanmamız lazım ancak zaten bağlı olup olmadığını kontrol ettirmemiz lazımdır.
            SqlConnection connection = new SqlConnection(@"server=(localdb)\MSSQLLocalDB;initial catalog= ETrade;integrated security=true");
            if (connection.State==ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlCommand command = new SqlCommand("Select * from Products",connection);

            SqlDataReader reader = command.ExecuteReader();
            
            DataTable dataTable=new DataTable();
            dataTable.Load(reader);
            reader.Close();
            connection.Close();
            return dataTable;
        }

    }
}
