using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracking
{
    class DataServices
    {
        private static string connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\{0};Integrated Security=True";

        public static DataTable GetTable(string filename, string select, string tablename)
        {
            System.Data.SqlClient.SqlConnection cn = new SqlConnection(string.Format(connectionString, filename));
            SqlDataAdapter da = new SqlDataAdapter(select, cn);
            DataTable dt = new DataTable(tablename);
            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dt;
        }

        public static int SaveTable(string filename, string select, DataTable data)
        {
            SqlConnection cn = new SqlConnection(string.Format(connectionString, filename));
            SqlDataAdapter da = new SqlDataAdapter(select, cn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            int rowsSaved = -1;
            try
            {
                rowsSaved = da.Update(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return rowsSaved;
        }

        public static int SaveRows(string filename, string select, DataRow[] dr)
        {
            SqlConnection cn = new SqlConnection(string.Format(connectionString, filename));
            SqlDataAdapter da = new SqlDataAdapter(select, cn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            int rowsSaved = -1;
            try
            {
                rowsSaved = da.Update(dr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return rowsSaved;
        }
    }
}
