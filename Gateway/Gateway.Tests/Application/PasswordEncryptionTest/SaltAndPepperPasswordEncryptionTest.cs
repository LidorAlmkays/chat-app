using Gateway.Application.Encryption;

namespace Gateway.Tests.Application.PasswordEncryptionTest
{
    public class SaltAndPepperPasswordEncryptionTest
    {
        private readonly SaltAndPepperEncryption _encryption;
        private const string PepperLetters = "safcxzv";
        private const int PepperWordLength = 2;

        public SaltAndPepperPasswordEncryptionTest()
        {
            _encryption = new SaltAndPepperEncryption(PepperLetters, PepperWordLength);
        }

        [Fact]
        public void CheckPasswordValid_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            string userPassword = "testPassword";
            var (encryptedPassword, encryptionKey) = _encryption.EncryptionPassword(userPassword);

            // Act
            bool isValid = _encryption.CheckPasswordValid(userPassword, encryptedPassword, encryptionKey);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void CheckPasswordValid_IncorrectPassword_ShouldReturnFalse()
        {
            // Arrange
            string userPassword = "testPassword";
            var (encryptedPassword, encryptionKey) = _encryption.EncryptionPassword(userPassword);
            string incorrectPassword = userPassword + " ";

            // Act
            bool isValid = _encryption.CheckPasswordValid(incorrectPassword, encryptedPassword, encryptionKey);

            // Assert
            Assert.False(isValid, "Modified password should be invalid.");
        }

        [Fact]
        public void EmptyPassword_ShouldBeInvalid()
        {
            // Arrange
            string userPasswordToCheck = "";
            var (encryptedPassword, encryptionKey) = _encryption.EncryptionPassword("testPassword");

            // Act
            bool isValid = _encryption.CheckPasswordValid(userPasswordToCheck, encryptedPassword, encryptionKey);

            // Assert
            Assert.False(isValid, "Empty password should be invalid.");
        }

        [Fact]
        public void CheckPasswordValid_EmptyStoredPassword_ShouldReturnFalse()
        {
            // Arrange
            string userPassword = "testPassword";
            var (_, encryptionKey) = _encryption.EncryptionPassword(userPassword);

            // Act
            bool isValid = _encryption.CheckPasswordValid(userPassword, "", encryptionKey);

            // Assert
            Assert.False(isValid, "Empty stored password should be invalid.");
        }

        [Fact]
        public void CheckPasswordValid_EmptyEncryptionKey_ShouldReturnFalse()
        {
            // Arrange
            string userPassword = "testPassword";
            var (encryptedPassword, _) = _encryption.EncryptionPassword(userPassword);

            // Act
            bool isValid = _encryption.CheckPasswordValid(userPassword, encryptedPassword, "");

            // Assert
            Assert.False(isValid, "Empty encryption key should be invalid.");
        }

        [Fact]
        public void CheckPasswordValid_NullInputs_ShouldReturnFalse()
        {
            // Arrange
            var (encryptedPassword, encryptionKey) = _encryption.EncryptionPassword("testPassword");

            // Act & Assert
            Assert.False(_encryption.CheckPasswordValid(null, encryptedPassword, encryptionKey), "Null password should be invalid.");
            Assert.False(_encryption.CheckPasswordValid("testPassword", null, encryptionKey), "Null encrypted password should be invalid.");
            Assert.False(_encryption.CheckPasswordValid("testPassword", encryptedPassword, null), "Null encryption key should be invalid.");
        }

        [Fact]
        public void CheckPasswordValid_EmptyEncryptionKeyAndPassword_ShouldReturnFalse()
        {
            // Act & Assert
            Assert.False(_encryption.CheckPasswordValid("", "", ""), "Empty user password, stored password, and encryption key should be invalid.");
        }

        [Fact]
        public void CheckPasswordValid_ShouldFailForModifiedEncryptionKey()
        {
            // Arrange
            string userPassword = "securePassword";
            var (encryptedPassword, encryptionKey) = _encryption.EncryptionPassword(userPassword);
            string modifiedKey = encryptionKey + "X"; // Modify the encryption key

            // Act
            bool isValid = _encryption.CheckPasswordValid(userPassword, encryptedPassword, modifiedKey);

            // Assert
            Assert.False(isValid, "Modified encryption key should invalidate the password.");
        }
    }
}