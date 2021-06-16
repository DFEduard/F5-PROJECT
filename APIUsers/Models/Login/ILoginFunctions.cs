namespace APIUsers.Models.Login
{
    public interface ILoginFunctions
    {
        bool Authenticate(LoginModel loginModel);
    }
}