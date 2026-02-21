namespace GNS.Extensions
{
    public static partial class StringExtentions
    {
        public static bool IsEmail(this string email)
        {
            return email.Contains('@') && email.Contains('.')
                && email.IndexOf('@') < email.IndexOf('.');
        }
        public static bool IsNotAddress(this string address)
        {
            return address.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c) && c != '.');
        }
        public static bool IsNotPassword(this string password)
        {
            return password.Any(c => !char.IsLetterOrDigit(c) && !char.IsPunctuation(c));
        }
        public static bool IsNotName(this string name)
        {
            return name.Any(c => !char.IsLetter(c));
        }
        public static bool IsNotGuid(this string guid)
        {
            return !Guid.TryParse(guid, out Guid result);
        }
    }
}