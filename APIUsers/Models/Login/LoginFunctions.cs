using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepoDb;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APIUsers.Models.Login
{
    public class LoginFunctions : ILoginFunctions
    {
        private readonly string connectionString;


        public LoginFunctions(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool Authenticate(LoginModel loginModel)
        {
            
            DataTable dt = new DataTable();
            string query = "SELECT CASE WHEN EXISTS(SELECT email FROM Users WHERE email=@email and password=@password) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS result";


            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, conn))
            using (var sqlAdapter = new SqlDataAdapter(cmd))
            {

                cmd.Parameters.Add("@email", SqlDbType.VarChar);
                cmd.Parameters.Add("@password", SqlDbType.VarChar);
                cmd.Parameters["@email"].Value = loginModel.email;
                cmd.Parameters["@password"].Value = loginModel.password;
                sqlAdapter.Fill(dt);
            }


            if (dt.Rows.Count > 0)
            {
                return (bool)dt.Rows[0]["result"];
            }


            return false;
        }

    }
}
