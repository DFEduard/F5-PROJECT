using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepoDb;
using Microsoft.Data.SqlClient;
using APIUsers.Models.User;
using APIUsers.Models.Validations;

namespace APIUsers.Models.User
{
    public class UserRepo : IUserRepo
    {
        private readonly string connectionString;
        private readonly Dictionary<string, object> response;
        private const string errorKey = "error";

        public UserRepo(string connectionString)
        {
            this.connectionString = connectionString;
            response = new Dictionary<string, object>();
            response.Add(errorKey, "");
        }

        public bool Create(Users user)
        {
            if(IsValidationTrue(user))
            {
                user.password = Utils.Encrypt(user.password);

                using (var conn = new SqlConnection(connectionString))
                {
                   
                    int userID = conn.Query<Users>(u => u.email == user.email).Select(u => u.userID).FirstOrDefault();

                    if(userID > 0)
                    {
                        response[errorKey] = UserConstants.emailExistsErrorMsg;
                        return false;
                    }


                    conn.ExecuteNonQuery("INSERT INTO Users (email, password, name, title, country) VALUES (@email, @password, @name, @title, @country)",
                    new { user.email, user.password, user.name, user.title, user.country });
                    return true;

                }
                
            }

            return false;
        }

        public Users[] Get()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                return conn.QueryAll<Users>().ToArray();
            }
        }

        public Users Get(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                return conn.Query<Users>(u => u.userID == id).FirstOrDefault();
            }
        }

        private bool IsValidationTrue(Users userModel)
        {
            Dictionary<string, object> regexErrors = new Dictionary<string, object>();

            if (!UserValidation.EmailAddress(userModel.email))
            {
                regexErrors.Add("email", UserConstants.emailRegexErrorMsg);
                
            }

            if (!UserValidation.Password(userModel.password))
            {
                regexErrors.Add("password", UserConstants.passwordRegexErrorMsg);
                
            }

            if(regexErrors.Count() != 0)
            {
                response[errorKey] = regexErrors;
                return false;
            }

            return true;
        }

        public Dictionary<string, object> UserErrorResponse()
        {
            return response;
        }
    }
}
