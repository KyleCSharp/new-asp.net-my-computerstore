using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyComputerStore.Pages.Clients
{
    public class createModel : PageModel // model
    {
        public createModel(SqlConnection connection) 
        {
            _connection= connection;
        }
        private readonly SqlConnection _connection;

        public ClientInfo ClientInfo= new ClientInfo();

        public string errorMessage = "";

        public string successMessage = "";

        public void OnPost()
        {
            ClientInfo.name = Request.Form["name"];
            ClientInfo.email = Request.Form["email"];
            ClientInfo.phone = Request.Form["phone"];
            ClientInfo.address = Request.Form["address"];
             // '||' means 'or'.if any clinet info is left blank returns error string :)
            if (ClientInfo.name.Length==0 || ClientInfo.email.Length==0|| ClientInfo.phone.Length==0||ClientInfo.address.Length==0) 
            {
                errorMessage = "ALL THE FIELDS ARE REQUIRED";
                return;
            }


            // save new client into the database
            try
            {
                
                using (_connection)
                {
                    _connection.Open();
                    var sql = "INSERT INTO CLIENTS " +
                        "(name, email, phone, address) VALUES " +
                        "(@name, @email, @phone, @address);";
                    using (SqlCommand command= new SqlCommand (sql, _connection)) // exucte command in server
                    {
                        command.Parameters.AddWithValue("@name", ClientInfo.name);
                        command.Parameters.AddWithValue("@email", ClientInfo.email);
                        command.Parameters.AddWithValue("@phone", ClientInfo.phone);
                        command.Parameters.AddWithValue("@address", ClientInfo.address);

                        command.ExecuteNonQuery ();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            ClientInfo.name = ""; ClientInfo.email = "";
            ClientInfo.phone = ""; ClientInfo.address = "";
            successMessage = "NEW CLIENT ADDED! TIME TO GET TO WORK!";
            Response.Redirect("/Clients/Index");
        }
    }
}
