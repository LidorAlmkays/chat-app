using Domain.Exceptions;
using Gateway.Domain.Exceptions.SpecificConstraint;
using Moq;
using Common.DTOs;
using Gateway.Domain.models;

namespace Gateway.Tests.Application.UserManager
{
    public class UserRepositoryManager_UpdateUserByEmailAsync_Tests : UserRepositoryManagerBaseTest
    {
        [Fact]
        public async Task UpdateUserByEmailAsync_ShouldEncryptPassword_AndUpdateUser()
        {
            // Arrange
            var requestDto = new RequestUpdateUserByEmailDTO
            {
                Email = "test@example.com",
                Password = "password123",
                Username = "UpdatedUser"
            };

            var encryptedPassword = "hashedPassword";
            var passwordKey = "passwordKey";

            _mockPasswordEncryption
                .Setup(pe => pe.EncryptionPassword(requestDto.Password))
                .Returns((encryptedPassword, passwordKey));

            _mockUserRepository
                .Setup(repo => repo.UpdateByEmailAsync(requestDto.Email, It.IsAny<UpdateUserModel>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userRepositoryManager.UpdateUserByEmailAsync(requestDto);

            // Assert
            _mockPasswordEncryption.Verify(pe => pe.EncryptionPassword(requestDto.Password), Times.Once);
            _mockUserRepository.Verify(repo => repo.UpdateByEmailAsync(requestDto.Email, It.Is<UpdateUserModel>(u =>
                u.Password == encryptedPassword &&
                u.PasswordKey == passwordKey &&
                u.Username == requestDto.Username
            )), Times.Once);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateUserByEmailAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
#pragma warning disable CS8625
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userRepositoryManager.UpdateUserByEmailAsync(null));
#pragma warning restore CS8625
        }

        [Fact]
        public async Task UpdateUserByEmailAsync_ShouldThrowConnectionException_WhenDatabaseConnectionFails()
        {
            // Arrange
            var requestDto = new RequestUpdateUserByEmailDTO
            {
                Email = "test@example.com",
                Password = "password123",
                Username = "UpdatedUser"
            };

            _mockPasswordEncryption
                .Setup(pe => pe.EncryptionPassword(requestDto.Password))
                .Returns(("hashedPassword", "passwordKey"));

            _mockUserRepository
                .Setup(repo => repo.UpdateByEmailAsync(It.IsAny<string>(), It.IsAny<UpdateUserModel>()))
                .ThrowsAsync(new ConnectionException("Database connection failed"));

            // Act & Assert
            await Assert.ThrowsAsync<ConnectionException>(() => _userRepositoryManager.UpdateUserByEmailAsync(requestDto));
        }

        [Theory]
        [InlineData(ConstraintType.ValidEmail, "Invalid email format. Please provide a valid email address.")]
        [InlineData(ConstraintType.CheckBirthday, "Age must be older.")]
        [InlineData(ConstraintType.ValidRole, "Invalid role. Allowed roles: Guest, User, Admin.")]
        [InlineData(ConstraintType.UsernameNotEmpty, "Username cannot be empty.")]
        [InlineData(ConstraintType.UsernameNotNull, "Username is required and cannot be null.")]
        [InlineData(ConstraintType.CheckPasswordLength, "Password must be at least 8 characters long.")]
        [InlineData(ConstraintType.UniqueEmail, "This email is already in use. Please use a different email.")]
        [InlineData(ConstraintType.PasswordNotNull, "Password is required and cannot be null.")]
        [InlineData(ConstraintType.RoleNotNull, "Role is required and cannot be null.")]
        [InlineData(ConstraintType.PasswordKeyNotNull, "Password key is required and cannot be null.")]
        [InlineData(ConstraintType.EmailNotNull, "Email is required and cannot be null.")]
        public async Task UpdateUserByEmailAsync_ShouldThrowConstraintViolationException_WhenConstraintFails(ConstraintType constraintType, string expectedMessage)
        {
            // Arrange
            var requestDto = new RequestUpdateUserByEmailDTO
            {
                Email = "test@example.com",
                Password = "password123",
                Username = "UpdatedUser"
            };

            _mockPasswordEncryption
                .Setup(pe => pe.EncryptionPassword(requestDto.Password))
                .Returns(("hashedPassword", "passwordKey"));

            _mockUserRepository
                .Setup(repo => repo.UpdateByEmailAsync(It.IsAny<string>(), It.IsAny<UpdateUserModel>()))
                .ThrowsAsync(new ConstraintViolationException(constraintType, expectedMessage));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ConstraintViolationException>(() => _userRepositoryManager.UpdateUserByEmailAsync(requestDto));

            Assert.Equal(constraintType, exception.ConstraintType);
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateUserByEmailAsync_ShouldThrowException_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            var requestDto = new RequestUpdateUserByEmailDTO
            {
                Email = "test@example.com",
                Password = "password123",
                Username = "UpdatedUser"
            };

            _mockPasswordEncryption
                .Setup(pe => pe.EncryptionPassword(requestDto.Password))
                .Returns(("hashedPassword", "passwordKey"));

            _mockUserRepository
                .Setup(repo => repo.UpdateByEmailAsync(It.IsAny<string>(), It.IsAny<UpdateUserModel>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userRepositoryManager.UpdateUserByEmailAsync(requestDto));
        }
    }
}