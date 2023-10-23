using Jatin.Models;
using Microsoft.Data.SqlClient;

namespace Jatin.Data
{
    public class ApplicationDB
    {
        private static IWebHostEnvironment Environment;
        private static IConfiguration Configuration { get; set; }
        private static string getConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration.GetConnectionString("localDb").ToString();

        }

        public static User getUser(string username)
        {
            User user = null;
            if (username == null)
            {
                user = null;
            }
            else
            {
                user = new User();
                SqlConnection conn = null;
                try
                {
                    using (conn = new SqlConnection(getConnectionString()))
                    {
                        conn.Open();
                        string Username = username.ToString();
                        string sql = "Select * from Users where Username = @username";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@username", Username);
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            user.Id = new Guid(dr["ID"].ToString());
                            user.Username = dr["Username"].ToString();
                            user.Name = dr["Name"].ToString();
                            user.Password = dr["Password"].ToString();
                            user.Profile_Path = dr["Profile_Path"].ToString();
                        }
                    }
                }
                catch
                {

                }
                finally
                {
                    conn.Close();
                }

            }
            return user;
        }

        public static bool registerUser(UserView user)
        {
            bool res = false;
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(getConnectionString()))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        Guid uId = Guid.NewGuid();
                        string sql = "Insert into Users(ID,Name,Username,Password) values(@ID,@Name,@Username,@Password)";
                        SqlCommand cmd = new SqlCommand(sql, conn); 
                        cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@ID", uId.ToString());
                        cmd.Parameters.AddWithValue("@Name", user.Name);
                        cmd.Parameters.AddWithValue("@Username", user.Username);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                         
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            res = true;
                        }

                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                res = false;
            }
            finally
            {
                conn.Close();
            }
            return res;
        }


        public static bool registerUser(UserView user,string filePath)
        {
            bool res = false;
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(getConnectionString()))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        Guid uId = Guid.NewGuid();
                        string sql = "Insert into Users(ID,Name,Username,Password,Profile_Path) values(@ID,@Name,@Username,@Password,@ProfilePath)";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@ID", uId.ToString());
                        cmd.Parameters.AddWithValue("@Name", user.Name);
                        cmd.Parameters.AddWithValue("@Username", user.Username);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.Parameters.AddWithValue("@ProfilePath", filePath);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            res = true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                res = false;
            }
            finally
            {
                conn.Close();
            }
            return res;
        }



        public static List<ReminderTaskView> getReminderTasks(Guid g)
        {
            List<ReminderTaskView> reminderTasks = null;
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(getConnectionString()))
                {
                    conn.Open();
                    if(conn.State == System.Data.ConnectionState.Open)
                    {
                        string sql = "Select * from ReminderTask where Uid = @uid";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@uid", g.ToString());
                        SqlDataReader dr = cmd.ExecuteReader();
                        reminderTasks = new List<ReminderTaskView>();
                        while (dr.Read())
                        {
                            ReminderTaskView r = new ReminderTaskView();
                            r.Id = Convert.ToInt32(dr["Id"]);
                            r.Name = dr["Name"].ToString();
                            r.Description = dr["Description"].ToString();
                            r.ActiveStatus = Convert.ToInt32(dr["ActiveStatus"]);
                            r.dateAt = DateOnly.FromDateTime(DateTime.Parse(dr["DateAt"].ToString())).ToString();
                            reminderTasks.Add(r);
                        }
                    }
                }
            }
            catch {
                reminderTasks = null;
            }
            finally
            {
                conn.Close();
            }
            return reminderTasks;
        }

        public static bool setReminderTask(NewReminderTask r,Guid id)
        {
            bool res = false;
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(getConnectionString()))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        string sql = "Insert into ReminderTask (Name,UId,Description,ActiveStatus,DateAt) values (@name,@uid,@description,1,@dateAt)";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@name",r.Name);
                        cmd.Parameters.AddWithValue("@uid",id.ToString());
                        cmd.Parameters.AddWithValue("@description",r.Description);
                        string d = String.Format("{0:yyyy-MM-dd}", r.DateAt);
                        cmd.Parameters.AddWithValue("@dateAt", d.ToString());
                        int ress = 0;
                        try
                        {
                           ress = cmd.ExecuteNonQuery();
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine( ex.Message);
                         }
                        
                        if (ress > 0)
                        {
                            res = true;
                        }
                        else
                        {
                            var s = " jj ";
                        }
                    }
                }
            }
            catch
            {
                res = false;
            }
            finally
            {
                conn.Close();
            }
            return res;
        }

        public static bool deleteReminder(int id,string uid)
        {
            bool res = false;
            SqlConnection conn = null;
            try
            {
                using(conn = new SqlConnection(getConnectionString()))
                {
                    conn.Open();
                    if(conn.State==System.Data.ConnectionState.Open)
                    {
                        string sql = "Delete from ReminderTask where id=@id and UId=@uid";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@id", id.ToString());
                        cmd.Parameters.AddWithValue("@uid", uid.ToString());
                        try
                        {
                            cmd.ExecuteNonQuery();
                            res = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }   
                }
            }
            catch
            {
                res = false;
            }
            finally
            {
                conn.Close();
            }
            return res;
        }

        public static bool disableTask(int id, string uid)
        {
            bool res = false;
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(getConnectionString()))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        string sql = "Update ReminderTask set ActiveStatus = 0 where id=@id and UId=@uid";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@id", id.ToString());
                        cmd.Parameters.AddWithValue("@uid", uid.ToString());
                        try
                        {
                            cmd.ExecuteNonQuery();
                            res = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch
            {
                res = false;
            }
            finally
            {
                conn.Close();
            }
            return res;
        }

        public static bool enableTask(int id, string uid)
        {
            bool res = false;
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(getConnectionString()))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        string sql = "Update ReminderTask set ActiveStatus = 1 where id=@id and UId=@uid";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@id", id.ToString());
                        cmd.Parameters.AddWithValue("@uid", uid.ToString());
                        try
                        {
                            cmd.ExecuteNonQuery();
                            res = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch
            {
                res = false;
            }
            finally
            {
                conn.Close();
            }
            return res;
        }

        public static bool updateReminderTask(UpdateReminder r, Guid id)
        {
            bool res = false;
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(getConnectionString()))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        string sql = "Update ReminderTask SET Name=@name,Description=@description,DateAt=@dateAt where id=@Id and UId=@uid";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@name", r.Name);
                        cmd.Parameters.AddWithValue("@description", r.Description);
                        string d = String.Format("{0:yyyy-MM-dd}", r.DateAt);
                        cmd.Parameters.AddWithValue("@dateAt", d.ToString());
                        cmd.Parameters.AddWithValue("@Id", r.Id.ToString());
                        cmd.Parameters.AddWithValue("@uid", id.ToString());
                        int ress = 0;
                        try
                        {
                            ress = cmd.ExecuteNonQuery();
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        if (ress > 0)
                        {
                            res = true;
                        }
                        else
                        {
                            var s = " jj ";
                        }
                    }
                }
            }
            catch
            {
                res = false;
            }
            finally
            {
                conn.Close();
            }
            return res;
        }

        public static PhoneBookUser getPhoneBookUser(string email)
        {
            PhoneBookUser phoneBookUser = new PhoneBookUser();
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(getConnectionString()))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        string procedureCommand = "OperationUser";
                        SqlCommand cmd = new SqlCommand(procedureCommand, conn);
                        cmd.Parameters.AddWithValue("@action", "getData");
                        cmd.Parameters.AddWithValue("@email", email);
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        while (reader.Read())
                        {
                            phoneBookUser.Id = reader["id"].ToString();
                            phoneBookUser.Email = reader["email"].ToString();
                            phoneBookUser.PhoneNumber = reader["phone"].ToString();
                            phoneBookUser.Password = reader["password"].ToString();
                        }
                    }
                }
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
            return phoneBookUser;
        }

        public static int checkEmail(string email)
        {
            int count = 5;
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(getConnectionString()))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        string procedureCommand = "OperationUser";
                        SqlCommand cmd = new SqlCommand(procedureCommand, conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@action", "check");
                        var r = cmd.ExecuteScalar();
                        count = Convert.ToInt32(r);
                    }

                }
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);  
            }
            finally
            {
                conn.Close();
            }
            return count;
        }

        public static bool addUser(PhoneBookGetUser u)
        {
            Guid newUserID = Guid.NewGuid() ;
            SqlConnection sqlConnection = null;
            using(sqlConnection = new SqlConnection(getConnectionString()))
            { 
                sqlConnection.Open();
                if(sqlConnection.State==System.Data.ConnectionState.Open)
                {
                    string sqlCommand = "OperationUser";
                    SqlCommand cmd = new SqlCommand(sqlCommand, sqlConnection);
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "insert");
                    cmd.Parameters.AddWithValue("@id", newUserID.ToString());
                    cmd.Parameters.AddWithValue("@name", u.Name);
                    cmd.Parameters.AddWithValue("@email", u.Email);
                    cmd.Parameters.AddWithValue("@phone", u.PhoneNumber);
                    cmd.Parameters.AddWithValue("@password", u.Password);
                    int res = 0;
                    try
                    {
                        res = cmd.ExecuteNonQuery();

                    }
                    catch
                    {
                        res = 0;
                    }
                    if (res > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
             }
            return false;
        }


    }
}
