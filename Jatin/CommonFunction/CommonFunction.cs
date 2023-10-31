using Newtonsoft.Json;
using System.Data;

namespace Jatin.CommonFunction
{
    public static class CommonFunction
    {
        public static string DataTableToJSON(DataTable dataTable)
        {
            string JSONString = "";
            JSONString = JsonConvert.SerializeObject(dataTable);
            return JSONString;
        }
    }
}
