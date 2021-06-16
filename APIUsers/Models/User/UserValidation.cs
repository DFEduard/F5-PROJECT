using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace APIUsers.Models.User
{
    public static class UserValidation
    {
        public static bool EmailAddress(string email)
        {
            Regex regex = new Regex(UserConstants.emailRegext);
            Match match = regex.Match(email);
            return match.Success;
        }

        public static bool Password(string password)
        {
            Regex regex = new Regex(UserConstants.passwordRegext);
            Match match = regex.Match(password);
            return match.Success;
        }
    }
}
