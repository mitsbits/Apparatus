namespace Borg.Infrastructure.Core.Services.Security
{
    public interface ICrypto
    {
        string HashPassword(string password);

        bool VerifyHashedPassword(string hashedPassword, string password);
    }
}