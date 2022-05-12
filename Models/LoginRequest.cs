namespace Webhallen.Models
{
    public readonly struct LoginRequest
    {
        public LoginRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; }

        public string Password { get; }
    }
}

