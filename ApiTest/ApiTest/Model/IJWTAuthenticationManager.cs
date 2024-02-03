namespace ApiTest.Model
{
    public interface IJWTAuthenticationManager
    {
        string? Authenticate(string username, string password);
    }

}

