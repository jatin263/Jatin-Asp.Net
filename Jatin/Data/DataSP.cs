using Microsoft.Data.SqlClient;
using System.Data;

namespace Jatin.Data
{
    public static class DataSP
    {
        public static IConfiguration Configuration { get; set; }
        private static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration.GetConnectionString("localDb").ToString();
        }
        public static DataTable GetAllDivision()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(GetConnectionString()))
                {
                    string sqlSP = "division_list";
                    SqlCommand cmd = new SqlCommand(sqlSP, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                conn?.Close();
            }
            return dt;

        }

        public static DataTable GetDistrict(int divisionID)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(GetConnectionString()))
                {
                    string sql = "district_list";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandType= CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter() {ParameterName= "@DivisionID",Value=divisionID,SqlDbType=SqlDbType.Int });
                    //cmd.Parameters.AddWithValue("@DivisionID", divisionID);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(dt);
                }
            }
            catch (Exception ex) { 
            
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public static DataTable GetBlock(string actionName,int districtID,int pageNum,int itemRow,string searchText)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(GetConnectionString()))
                {
                    string sql = "block_details";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@action", actionName);
                    cmd.Parameters.Add(new SqlParameter() { ParameterName = "@DistrictID", Value = districtID, SqlDbType = SqlDbType.Int });
                    cmd.Parameters.Add(new SqlParameter() { ParameterName = "@startNum", Value = pageNum, SqlDbType = SqlDbType.Int });
                    cmd.Parameters.Add(new SqlParameter() { ParameterName = "@numRow", Value = itemRow, SqlDbType = SqlDbType.Int });
                    cmd.Parameters.Add(new SqlParameter() { ParameterName = "@searchText", Value = searchText, SqlDbType = SqlDbType.VarChar });
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
    }
}
