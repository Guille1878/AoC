using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public static class SqlService
    {
        public static SqlConnection _sqlConnection;

        //public static void Init()
        //{
        //    _sqlConnection = new SqlConnection("Server=FUESTECHECOMPUT\\WILLE;Integrated Security=true;Initial Catalog=Lab;");
        //    _sqlConnection.Open();
        //}

        const string connectionString = "Server=FUESTECHECOMPUT\\WILLE;Integrated Security=true;Initial Catalog=Lab;";


        public static void AddRow(string text, int len)
        {
            using var conn = new SqlConnection(connectionString);

            using var command = new SqlCommand($"INSERT INTO [dbo].[Way] ([WayType], [Lenght]) VALUES ('{text}', {len})", conn);

            conn.Open();
            command.ExecuteNonQuery();

            //using var conn = new SqlConnection(connectionString);
            //using var command = new SqlCommand("dbo.Way_Add", conn)
            //{
            //    CommandType = CommandType.StoredProcedure,
            //};
            //command.Parameters.AddWithValue("input", text);
            //command.Parameters.AddWithValue("lenght", len);
            //conn.Open();
            //await command.ExecuteNonQueryAsync();

        }

        public static bool WayExists(string text, int len)
        {
            try
            {

            
            using var conn = new SqlConnection(connectionString);

            using var command = new SqlCommand($"Select WayType From dbo.Way Where lenght = {len} And WayType = '{text}'", conn);

            conn.Open();
            var reader = command.ExecuteReader();

            return reader.HasRows;

            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
    }
}
