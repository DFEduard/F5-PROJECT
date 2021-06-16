using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace APIUsers.Models.User
{
    public class Users
    {
        [Required]
        public int userID { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string country { get; set; }


        public Object ReadOnly(bool excludeID=false)
        {
            if (excludeID)
            {
                return new {email = this.email, title = this.title, name = this.name, country = this.country };
            }

            return new { userID = this.userID, email = this.email, title = this.title, name = this.name, country = this.country };

        }

        public class UserEdit
        {
            public string email { get; set; }
            public string password { get; set; }
            public string title { get; set; }
            public string name { get; set; }
            public string country { get; set; }
        }

    }

}
