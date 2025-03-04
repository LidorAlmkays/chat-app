using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Application.Encryption
{
    public interface IPasswordEncryption
    {
        bool CheckPasswordValid(string passwordToCheck, string encryptedPassword, string passwordKey);
        (string encryptedPassword, string encryptionKey) EncryptionPassword(string password);
    }
}