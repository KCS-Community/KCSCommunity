using KCSCommunity.Domain.Entities;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace KCSCommunity.Infrastructure.Security.Hashing;

public class Argon2PasswordHasher : IPasswordHasher<ApplicationUser>
{
    private const int SaltSize = 16; //128bit
    private const int HashSize = 32; //256bit
    private const int DegreeOfParallelism = 8;
    private const int Iterations = 4;
    private const int MemorySize = 1024 * 128; //128MB

    public string HashPassword(ApplicationUser user, string password)
    {
        //生成一个盐
        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        //得到Argon2id哈希
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = DegreeOfParallelism,
            Iterations = Iterations,
            MemorySize = MemorySize
        };

        var hash = argon2.GetBytes(HashSize);

        //合并盐和哈希
        //格式[Salt (16 bytes)][Hash (32 bytes)]
        var combinedBytes = new byte[SaltSize + HashSize];
        Buffer.BlockCopy(salt, 0, combinedBytes, 0, SaltSize);
        Buffer.BlockCopy(hash, 0, combinedBytes, SaltSize, HashSize);

        return Convert.ToBase64String(combinedBytes);
    }

    public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword) || string.IsNullOrWhiteSpace(providedPassword))
        {
            return PasswordVerificationResult.Failed;
        }

        try
        {
            var combinedBytes = Convert.FromBase64String(hashedPassword);

            //判断长度
            if (combinedBytes.Length != SaltSize + HashSize)
            {
                return PasswordVerificationResult.Failed;
            }

            //取盐
            var salt = new byte[SaltSize];
            Buffer.BlockCopy(combinedBytes, 0, salt, 0, SaltSize);

            //重哈希
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(providedPassword))
            {
                Salt = salt,
                DegreeOfParallelism = DegreeOfParallelism,
                Iterations = Iterations,
                MemorySize = MemorySize
            };

            var newHash = argon2.GetBytes(HashSize);

            //取原哈希
            var originalHash = new byte[HashSize];
            Buffer.BlockCopy(combinedBytes, SaltSize, originalHash, 0, HashSize);
            
            if (CryptographicOperations.FixedTimeEquals(newHash, originalHash))
            {
                return PasswordVerificationResult.Success;
            }

            return PasswordVerificationResult.Failed;
        }
        catch (FormatException)
        {
            //Base64错误
            return PasswordVerificationResult.Failed;
        }
        catch (Exception)
        {
            //最终Fallback error
            return PasswordVerificationResult.Failed;
        }
    }
}