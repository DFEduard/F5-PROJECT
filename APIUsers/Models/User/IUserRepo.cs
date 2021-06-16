using System;
using System.Collections.Generic;
using static APIUsers.Models.User.Users;

namespace APIUsers.Models.User
{
    public interface IUserRepo
    {
        bool Create(Users user);
        List<Object> Get();
        Users Get(int id);
        Users Put(int id, UserEdit user);
        bool Delete(int id);
        Dictionary<string, object> UserErrorResponse();

    }
}