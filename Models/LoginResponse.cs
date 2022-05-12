namespace Webhallen.Models
{
    public readonly struct LoginResponse
    {
        public readonly string LastVisit;

        public readonly string WebhallenAuth;

        public LoginResponse(string lastVisit, string webhallenAuth)
        {
            LastVisit = lastVisit;
            WebhallenAuth = webhallenAuth;
        }
    }
}
