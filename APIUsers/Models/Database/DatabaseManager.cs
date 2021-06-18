using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APIUsers.Models.Database
{
    public class DatabaseManager : IDatabaseManager
    {
        private readonly string connectionString;
        

        public DatabaseManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Check if user credentials are valid 
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        /// <returns>Current user id</returns>
        public int ValidUserCredentials(string email, string password)
        {
            // query parameters
            string paramEmail = "@email";
            // db query
            string query = $"SELECT userID, password FROM Users WHERE email={paramEmail}";
            // create parameter list
            SqlParameter[] paramList = {
                new SqlParameter(paramEmail, SqlDbType.VarChar, 50) { Value = email },
            };
            // get db result into table
            DataTable dataTable = QueryDb(query, paramList);

            if (dataTable.Rows.Count > 0)
            {
                string dbPassword = (string)dataTable.Rows[0]["password"];

                try
                {
                    if (Utils.Decrypt(dbPassword) == password)
                    {
                        return (int)dataTable.Rows[0]["UserID"];
                    }
                }
                catch (FormatException)
                {
                    if(dbPassword == password)
                    {
                        return (int)dataTable.Rows[0]["UserID"];
                    }
                }
                
            }

            return -1;
        }

        /// <summary>
        /// Register a refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <param name="userID">Current user id</param>
        /// <returns></returns>
        public bool RegisterRefreshToken(string refreshToken, int userID)
        {

            string paramRefreshToken = "@refreshToken";
            string paramUserID = "@userID";

            string query = $"INSERT INTO Token VALUES ({paramRefreshToken}, {paramUserID})";

            SqlParameter[] paramList = {
                new SqlParameter(paramRefreshToken, SqlDbType.VarChar, 100) { Value = refreshToken },
                new SqlParameter(paramUserID, SqlDbType.Int) { Value = userID },
            };

            DataTable dataTable = QueryDb(query, paramList);

            return true;
        }

        /// <summary>
        /// Check if a refresh token is exists into db and delete it
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns>Current user email</returns>
        public string CheckRefreshToken(string refreshToken)
        {
            // query parameter
            string paramRefreshToken = "@refreshToken";
            // db query
            string query = $"SELECT TOP 1 Users.email as email FROM Users INNER JOIN Token ON Users.userID = Token.UserID AND Token.refresh={paramRefreshToken}";
            // create parameter list
            SqlParameter[] paramList =
            {
                new SqlParameter(paramRefreshToken, SqlDbType.VarChar, 100) { Value = refreshToken }
            };

            DataTable dataTable = QueryDb(query, paramList);

            if (dataTable.Rows.Count > 0)
            {
                query = $"DELETE FROM Token WHERE refresh={paramRefreshToken}";
                // create parameter list
                SqlParameter[] paramListDel =
                {
                    new SqlParameter(paramRefreshToken, SqlDbType.VarChar, 100) { Value = refreshToken }
                };

                QueryDb(query, paramListDel);

                // return true if credentials matches entry in db else false 
                return (string)dataTable.Rows[0]["email"];
            };

            return null;
        }

        /// <summary>
        /// Query database
        /// </summary>
        /// <param name="query">query string</param>
        /// <param name="sqlParams">list of parameters</param>
        /// <returns>Datatable object</returns>
        public DataTable QueryDb(string query, SqlParameter[] sqlParams=null)
        {
            DataTable dataTable = new DataTable();
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, conn))
            using (var sqlAdapter = new SqlDataAdapter(cmd))
            {

                foreach (SqlParameter param in sqlParams)
                {
                    if (param != null)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                sqlAdapter.Fill(dataTable);
            }


            return dataTable;
        }

    }
}
