using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyComputerStore.Pages.Clients
{
    public class EditModel : PageModel
    {

        public EditModel(SqlConnection connection)
        {
            _connection = connection;
        }
        private readonly SqlConnection _connection;


        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                
                using (_connection)
                {
                    _connection.Open();
                    var sql = "SELECT * FROM clients WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(sql, _connection)) // exucate command to server 
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                

                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

            }
        }
        public void OnPost() 
        {

            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];
            if (clientInfo.id.Length== 0 ||
                clientInfo.name.Length == 0 || 
                clientInfo.email.Length == 0 ||
                clientInfo.phone.Length == 0 || 
                clientInfo.address.Length == 0) // '||' means 'or'
            {
                errorMessage = "ALL FIELDS ARE REQUIRED!";
                return;
            }//if any clinet info is left blank returns error string :)
            try
            {
                String connectionString = "Data Source=Localhost;Initial Catalog=mystore;Integrated Security=True";
                using (_connection)
                {
                    _connection.Open();
                    String sql = "UPDATE clients " + 
                        "SET name=@name, email=@email, phone=@phone, address=@address " +
                        "WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, _connection))
                    {
                        command.Parameters.AddWithValue("name",clientInfo.name);
                        command.Parameters.AddWithValue("email",clientInfo.email);
                        command.Parameters.AddWithValue("phone",clientInfo.phone);
                        command.Parameters.AddWithValue ("address",clientInfo.address);
                        command.Parameters.AddWithValue("id", clientInfo.id);
                        
                        
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
                return;
            }
            Response.Redirect("/Clients/Index");
        }

    }
}
