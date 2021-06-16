using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepoDb;
using Microsoft.Data.SqlClient;
using APIUsers.Models.User;
using APIUsers.Models.Validations;
using static APIUsers.Models.User.Users;

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
            if(IsValidationTrue(user.email, user.password))
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

        public List<Object> Get()
        {
            List<Object> usersList = null;

            using (var conn = new SqlConnection(connectionString))
            {
                var users = conn.QueryAll<Users>().ToArray();
                if (users != null)
                {
                    usersList = new List<Object>();
                    foreach (var user in users)
                    {
                        usersList.Add(user.ReadOnly());
                    }

                    return usersList;
                }

                response[errorKey] = Utils.NotFoundResponse();
            }

            return usersList;
        }

        public Users Get(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                return conn.Query<Users>(u => u.userID == id).FirstOrDefault();
            }
        }

        public Users Put(int id, UserEdit editedUser)
        {

            using (var conn = new SqlConnection(connectionString))
            {
                var dbUser = conn.Query<Users>(u => u.userID == id).FirstOrDefault();

                if (dbUser != null)
                {
                    if (IsValidationTrue(editedUser.email, editedUser.password))
                    {
                        dbUser = UpdateWithEdit(dbUser, editedUser);

                        if (!string.IsNullOrEmpty(editedUser.password))
                        {
                            dbUser.password = Utils.Encrypt(editedUser.password);
                        }

                        conn.Update(dbUser);
                        return dbUser;
                    }

                    return null;
                }

                response[errorKey] = Utils.NotFoundResponse();
            }

            return null;
        }

        
        public bool Delete(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var dbUser = conn.Query<Users>(u => u.userID == id).FirstOrDefault();

                if (dbUser != null)
                {
                    conn.Delete(dbUser);
                    return true;
                }

                response[errorKey] = Utils.NotFoundResponse();
            }
                return false;
        }
        

        private bool IsValidationTrue(string email=null, string password=null)
        {
            Dictionary<string, object> regexErrors = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(email))
            {
                if (!UserValidation.EmailAddress(email))
                {
                    regexErrors.Add("email", UserConstants.emailRegexErrorMsg);
                }
            }
            
            if (!string.IsNullOrEmpty(password))
            {
                if (!UserValidation.Password(password))
                {
                    regexErrors.Add("password", UserConstants.passwordRegexErrorMsg);
                }
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

        private Users UpdateWithEdit(Users user, UserEdit editedUser)
        {
            foreach (var prop in editedUser.GetType().GetProperties())
            {
                string propName = prop.Name;
                var valueEdited = editedUser.GetType().GetProperty(prop.Name).GetValue(editedUser, null);
                var value = user.GetType().GetProperty(prop.Name).GetValue(user, null);
                if (valueEdited != null)
                {
                    
                    if (valueEdited.ToString() != value.ToString())
                    {
                        if(propName != "password")
                        {
                            user.GetType().GetProperty(prop.Name).SetValue(user, valueEdited);
                        }
                       
                    }
                }
            }
            return user;
        }
    }
}
