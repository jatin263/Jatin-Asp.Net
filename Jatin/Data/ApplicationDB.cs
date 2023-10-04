using Microsoft.Data.SqlClient;
namespace Jatin.Data
{
    public class ApplicationDB
    {
        private static IConfiguration Configuration { get; set; }
        public static string getConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration.GetConnectionString("jatinDb").ToString();

        }
    }
}
