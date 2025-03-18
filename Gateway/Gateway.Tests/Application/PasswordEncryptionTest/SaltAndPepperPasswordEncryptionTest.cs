using Gateway.Application.Encryption;

namespace Gateway.Tests.Application.PasswordEncryptionTest
{
    public abstract class BaseSaltAndPepperPasswordEncryptionTest
    {
#pragma warning disable CA1051 // Do not declare visible instance fields
        protected readonly SaltAndPepperEncryption _encryption;
        protected const string PepperLetters = "safcxzv";
        protected const int PepperWordLength = 2;
#pragma warning restore CA1051 // Do not declare visible instance fields

        protected BaseSaltAndPepperPasswordEncryptionTest()
        {
            _encryption = new SaltAndPepperEncryption(PepperLetters, PepperWordLength);
        }


    }
}