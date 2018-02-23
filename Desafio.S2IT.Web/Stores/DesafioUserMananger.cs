using Desafio.S2IT.Data.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace Desafio.S2IT.Web.Stores
{
    public class DesafioUserMananger : UserManager<Login>
    {
        public DesafioUserMananger(IUserStore<Login> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<Login> passwordHasher, IEnumerable<IUserValidator<Login>> userValidators, IEnumerable<IPasswordValidator<Login>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<Login>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            this.PasswordHasher = new DesafioPasswordHasher();
        }
    }

    public class DesafioPasswordHasher : PasswordHasher<Login>
    {
        private byte[] key = Convert.FromBase64String("299E651B7E443B6B682633CD098AD26A");
        private byte[] IV = new byte[] { 0x50, 0x08, 0xF1, 0xDD, 0xDE, 0x3C, 0xF2, 0x18, 0x44, 0x74, 0x19, 0x2C, 0x53, 0x49, 0xAB, 0xBC };

        public override string HashPassword(Login usuario, string password)
        {
            //string hashProvidedPassword = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(password));

            //var key = Convert.FromBase64String("Q3JpcHRvZ3JhZmlhcyBjb20gUmluamRhZWwgLyBBRVM=");
            //var IV = new byte[] { 0x50, 0x08, 0xF1, 0xDD, 0xDE, 0x3C, 0xF2, 0x18, 0x44, 0x74, 0x19, 0x2C, 0x53, 0x49, 0xAB, 0xBC };

            string hashProvidedPassword = Convert.ToBase64String(EncryptStringToBytes(password, key, IV));

            return hashProvidedPassword;
        }

        public override PasswordVerificationResult VerifyHashedPassword(Login usuario, string hashedPassword, string providedPassword)
        {
            string hashProvidedPassword = Convert.ToBase64String(EncryptStringToBytes(providedPassword, key, IV));

            if (hashProvidedPassword == hashedPassword)
            {
                return PasswordVerificationResult.SuccessRehashNeeded;
            }
            else
            {
                return PasswordVerificationResult.Failed;
            }
        }

        static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an Rijndael object
            // with the specified key and IV.
            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
    }
}
