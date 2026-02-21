using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class Hasher : IHasher
    {
        public string Generate(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public bool Verify(string password, string hashedPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);

    }
}