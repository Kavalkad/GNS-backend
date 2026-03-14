namespace GNS.Contracts.Responses
{
    public class LoginUserResponse
    {
        public string UserName { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
       // public int ExpiredIn { get; set; }
        public string RefreshToken { get; set; } = null!;
    }
}