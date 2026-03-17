namespace GNS.Contracts.Responses
{
    public record class LoginEmployeeResponse : LoginResponse
    {
        

        public string Role { get; set; } = string.Empty;
        
    }
}