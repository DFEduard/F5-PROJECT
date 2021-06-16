using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIUsers.Models.User
{
    public static class UserConstants
    {
        public const string emailRegext = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        public const string passwordRegext = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

        public const string emailRegexErrorMsg = "Invalid email";
        public const string passwordRegexErrorMsg = "Password must contain minimum eight characters, at least one letter, one number and one special character";
        public const string emailExistsErrorMsg = "User with the same email already exists.";
    }
}
