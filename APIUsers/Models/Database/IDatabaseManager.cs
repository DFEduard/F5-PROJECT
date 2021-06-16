namespace APIUsers.Models.Database
{
    public interface IDatabaseManager
    {
        int ValidUserCredentials(string email, string password);
        string CheckRefreshToken(string refreshToken);
        bool RegisterRefreshToken(string refreshToken, int userID);
    }
}