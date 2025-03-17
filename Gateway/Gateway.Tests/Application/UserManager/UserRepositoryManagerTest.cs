using Common.DTOs;
using Domain.Exceptions;
using Gateway.Application.Encryption;
using Gateway.Application.UserManager;
using Gateway.Domain.Exceptions;
using Gateway.Domain.Exceptions.SpecificConstraint;
using Gateway.Domain.models;
using Gateway.Infrastructure.UserRepository;
using Moq;

namespace Gateway.Tests.Application.UserManager
{
    public class UserRepositoryManagerTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IPasswordEncryption> _mockPasswordEncryption;
        private readonly UserRepositoryManager _userRepositoryManager;

        public UserRepositoryManagerTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordEncryption = new Mock<IPasswordEncryption>();
            _userRepositoryManager = new UserRepositoryManager(_mockUserRepository.Object, _mockPasswordEncryption.Object);
        }

        [Fact]
        public async Task AddUserAsync_ShouldEncryptPassword_AndInsertUser()
        {
            // Arrange
            var requestDto = new RequestCreateUserDTO
            {
                Birthday = new DateTime(1995, 5, 23),
                Email = "test@example.com",
                Password = "password123",
                Username = "TestUser"
            };

            var encryptedPassword = "hashedPassword";
            var passwordKey = "passwordKey";
            Guid expectedUserId = Guid.NewGuid();

            _mockPasswordEncryption
                .Setup(pe => pe.EncryptionPassword(requestDto.Password))
                .Returns((encryptedPassword, passwordKey));

            _mockUserRepository
                .Setup(repo => repo.InsertUser(It.IsAny<UserModel>()))
                .ReturnsAsync(expectedUserId);

            // Act
            var result = await _userRepositoryManager.AddUserAsync(requestDto);

            // Assert
            _mockPasswordEncryption.Verify(pe => pe.EncryptionPassword(requestDto.Password), Times.Once);
            _mockUserRepository.Verify(repo => repo.InsertUser(It.Is<UserModel>(u =>
                u.Email == requestDto.Email &&
                u.Password == encryptedPassword &&
                u.PasswordKey == passwordKey &&
                u.Birthday == requestDto.Birthday
            )), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(expectedUserId, result.Id);
        }

        [Fact]
        public async Task AddUserAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
#pragma warning disable CS8625
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userRepositoryManager.AddUserAsync(null));
#pragma warning restore CS8625
        }

        [Fact]
        public async Task AddUserAsync_ShouldThrowConnectionException_WhenDatabaseConnectionFails()
        {
            // Arrange
            var requestDto = new RequestCreateUserDTO
            {
                Email = "test@example.com",
                Password = "password123",
                Username = "TestUser",
                Birthday = new DateTime(2000, 1, 1)
            };

            _mockPasswordEncryption
                .Setup(pe => pe.EncryptionPassword(requestDto.Password))
                .Returns(("hashedPassword", "passwordKey"));

            _mockUserRepository
                .Setup(repo => repo.InsertUser(It.IsAny<UserModel>()))
                .ThrowsAsync(new ConnectionException("Database connection failed"));

            // Act & Assert
            await Assert.ThrowsAsync<ConnectionException>(() => _userRepositoryManager.AddUserAsync(requestDto));
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
        public async Task AddUserAsync_ShouldThrowConstraintViolationException_WhenConstraintFails(ConstraintType constraintType, string expectedMessage)
        {
            // Arrange
            var requestDto = new RequestCreateUserDTO
            {
                Email = "test@example.com",
                Password = "password123",
                Username = "TestUser",
                Birthday = new DateTime(1998, 7, 15)
            };

            _mockPasswordEncryption
                .Setup(pe => pe.EncryptionPassword(requestDto.Password))
                .Returns(("hashedPassword", "passwordKey"));

            _mockUserRepository
                .Setup(repo => repo.InsertUser(It.IsAny<UserModel>()))
                .ThrowsAsync(new ConstraintViolationException(constraintType, expectedMessage));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ConstraintViolationException>(() => _userRepositoryManager.AddUserAsync(requestDto));

            Assert.Equal(constraintType, exception.ConstraintType);
            Assert.Equal(expectedMessage, exception.Message);
        }


        [Fact]
        public async Task AddUserAsync_ShouldThrowException_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            var requestDto = new RequestCreateUserDTO
            {
                Email = "test@example.com",
                Password = "password123",
                Username = "TestUser",
                Birthday = new DateTime(1990, 12, 10)
            };

            _mockPasswordEncryption
                .Setup(pe => pe.EncryptionPassword(requestDto.Password))
                .Returns(("hashedPassword", "passwordKey"));

            _mockUserRepository
                .Setup(repo => repo.InsertUser(It.IsAny<UserModel>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userRepositoryManager.AddUserAsync(requestDto));
        }
        [Fact]
        public async Task DeleteUserByEmailAsync_ShouldReturnResponse_WhenDeletionIsSuccessful()
        {
            // Arrange
            var requestDto = new RequestDeleteUserByEmailDTO { Email = "test@example.com" };

            _mockUserRepository
                .Setup(repo => repo.DeleteUserByEmail(requestDto.Email))
                .ReturnsAsync(new UserModel()); // Returns a valid user model instead of bool

            // Act
            var result = await _userRepositoryManager.DeleteUserByEmailAsync(requestDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ResponseDeleteUserByEmailDTO>(result);

            _mockUserRepository.Verify(repo => repo.DeleteUserByEmail(requestDto.Email), Times.Once);
        }

        [Fact]
        public async Task DeleteUserByEmailAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userRepositoryManager.DeleteUserByEmailAsync(null!));
        }

        [Fact]
        public async Task DeleteUserByEmailAsync_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var requestDto = new RequestDeleteUserByEmailDTO { Email = "nonexistent@example.com" };

            _mockUserRepository
                .Setup(repo => repo.DeleteUserByEmail(requestDto.Email))
                .ThrowsAsync(new UserNotFoundException());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UserNotFoundException>(() => _userRepositoryManager.DeleteUserByEmailAsync(requestDto));

            Assert.Equal("User not found inside registry", exception.Message);
        }

        [Fact]
        public async Task DeleteUserByEmailAsync_ShouldThrowConnectionException_WhenDatabaseConnectionFails()
        {
            // Arrange
            var requestDto = new RequestDeleteUserByEmailDTO { Email = "test@example.com" };

            _mockUserRepository
                .Setup(repo => repo.DeleteUserByEmail(requestDto.Email))
                .ThrowsAsync(new ConnectionException());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ConnectionException>(() => _userRepositoryManager.DeleteUserByEmailAsync(requestDto));

            Assert.Equal("Failed to connect to the registry", exception.Message);
        }

        [Fact]
        public async Task DeleteUserByEmailAsync_ShouldThrowException_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            var requestDto = new RequestDeleteUserByEmailDTO { Email = "test@example.com" };

            _mockUserRepository
                .Setup(repo => repo.DeleteUserByEmail(requestDto.Email))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _userRepositoryManager.DeleteUserByEmailAsync(requestDto));

            Assert.Equal("Unexpected error", exception.Message);
        }
    }
}