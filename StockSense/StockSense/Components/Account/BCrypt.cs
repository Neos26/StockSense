using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto.Generators;
using StockSense.Data;
using StockSense.shared; // Updated to point to StockSense's models

namespace StockSense.Utility.Security // Updated namespace to StockSense
{
    // We tell this class to specifically handle passwords for your ApplicationUser model
    public class BCryptPasswordHasher : IPasswordHasher<ApplicationUser>
    {
        // 1. REGISTRATION: Intercepts the plain text password and hashes it before saving to the database
        public string HashPassword(ApplicationUser user, string password)
        {
            // Set the Work Factor (Iteration Count). 
            // 11 or 12 is the current industry standard for web applications.
            // Higher number = exponentially harder to crack, but takes slightly longer to log in.
            int workFactor = 12;

            // BCrypt generates the salt AND applies the work factor automatically
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor);
        }

        // 2. LOGIN: Intercepts the login attempt and compares it to the database hash
        public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
            try
            {
                // BCrypt.Verify automatically extracts the salt from the hashedPassword and checks for a match
                bool isValid = BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);

                if (isValid)
                {
                    return PasswordVerificationResult.Success;
                }

                return PasswordVerificationResult.Failed;
            }
            catch
            {
                // If the database hash is corrupted or isn't a valid BCrypt string, fail safely
                return PasswordVerificationResult.Failed;
            }
        }
    }
}