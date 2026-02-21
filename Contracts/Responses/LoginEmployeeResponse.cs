namespace GNS.Contracts.Response
{
    public record class LoginEmployeeResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}