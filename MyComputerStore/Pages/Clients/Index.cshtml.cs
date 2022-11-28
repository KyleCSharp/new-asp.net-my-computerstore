using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyComputerStore.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public IndexModel(SqlConnection connection)
        {
            _connection = connection;
        }
        private readonly SqlConnection _connection;

        public List<ClientInfo> ListClients= new List<ClientInfo>();

        public void OnGet()
        {
            try // connecting to the data base
            {
                
                using (_connection)
                {
                    _connection.Open();
                    var sql = "SELECT * FROM clients";
                    using (SqlCommand command = new SqlCommand(sql, _connection)) // exucate command to server 
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                                clientInfo.created_at = reader.GetDateTime(5).ToString();

                                ListClients.Add(clientInfo); // addding to list
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception:" + ex.ToString()); // exception catch error
            }




        }
    }public class ClientInfo
    {
        public string id;
        public string name;
        public string email;
        public string phone;
        public string address;
        public string created_at;
    }
}
