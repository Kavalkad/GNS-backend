namespace GNS.Services.Interfaces
{
    public interface IHasher
    {
        string Generate(string word);
        bool Verify(string word, string hashedWord);
    }
}