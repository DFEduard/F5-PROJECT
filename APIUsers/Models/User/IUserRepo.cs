using System.Collections.Generic;

namespace APIUsers.Models.User
{
    public interface IUserRepo
    {
        bool Create(Users user);
        Users[] Get();
        Users Get(int id);
        Dictionary<string, object> UserErrorResponse();

    }
}