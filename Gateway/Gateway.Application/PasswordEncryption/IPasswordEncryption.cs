using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Application.Encryption
{
    public interface IPasswordEncryption
    {
        bool CheckPasswordValid(string? password, string? encryptedPassword, string? encryptionKey);
        (string encryptedPassword, string encryptionKey) EncryptionPassword(string password);
    }
}