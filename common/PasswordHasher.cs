using Microsoft.AspNetCore.Identity;

namespace authmodule.Common
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly int iterationCount;
        public PasswordHasher(int iterationCount = 10000) 
        {
            if(iterationCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(iterationCount), "Password has iteration count cannot be less than 1");
            }

            this.iterationCount = iterationCount;
        }

        public string HashPassword(string password)
        {
            return Crypto.HashPassword(password, this.iterationCount);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if(Crypto.VerifyHashedPassword(hashedPassword, providedPassword, iterationCount)) 
            { 
                return PasswordVerificationResult.Success;
            }
            return PasswordVerificationResult.Failed;
        }
    }

    public interface IPasswordHasher
    {
        string HashPassword(string password);
        PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword);
    }
}