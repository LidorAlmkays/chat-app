using Xunit;
using Application.Encryption;
using System.Text;

namespace Gateway.Tests.Application.PasswordEncryptionTest
{
    public class SaltAndPepperPasswordEncryptionTest
    {
        [Fact]
        public void TestCheckPasswordValid()
        {
            // Arrange
            string pepperLetters = "safcxzv";
            int pepperWordLength = 2;
            var encryption = new SaltAndPepperEncryption(pepperLetters, pepperWordLength);
            string userPasswordToCheck = "testPassword";
            // Act
            var (encryptedPassword, encryptionKey) = encryption.EncryptionPassword(userPasswordToCheck);
            // Assert
            // Check if the password is valid
            Assert.True(encryption.CheckPasswordValid(userPasswordToCheck, encryptedPassword, encryptionKey), "Password should be valid.");

            // Check if the password with extra space is invalid
            Assert.False(encryption.CheckPasswordValid(userPasswordToCheck + " ", encryptedPassword, encryptionKey), "Password with space at the end should be invalid.");

            // Check if the password with an empty substring is invalid
            Assert.False(encryption.CheckPasswordValid(userPasswordToCheck[..0], encryptedPassword, encryptionKey), "Empty password should be invalid.");

        }
    }
}