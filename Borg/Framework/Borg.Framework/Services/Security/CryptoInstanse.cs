using Borg.Infrastructure.Core.DI;
using Borg.Infrastructure.Core.Services.Security;

namespace Borg.Framework.Services.Security
{
    [PlugableService(ImplementationOf = typeof(ICrypto), Lifetime = Lifetime.Singleton, OneOfMany = true, Order = 1)]
    public class CryptoInstanse : ICrypto
    {
        public string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            return Crypto.VerifyHashedPassword(hashedPassword, password);
        }
    }
}