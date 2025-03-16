using Gateway.Application.Encryption;
using Gateway.Application.mapping;
using Gateway.Application.UserManager;
using Common.DTOs;
using Gateway.Domain.models;
using Gateway.Infrastructure.UserRepository;
using Moq;


namespace Gateway.Tests.Application.UserManager
{
    public class UserRepositoryManagerTest
    {
        Mock<IUserRepository> _mockUserRepository;
        Mock<IPasswordEncryption> _mockPasswordEncryption;
        UserRepositoryManager _userRepositoryManager;
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
                Birthday = 3,
                Email = "",
                Password = "",
                Username = ""
            };

            var userModel = requestDto.ToUserModelAsUser(); // Convert DTO to Model
            var encryptedPassword = "hashedPassword";
            var passwordKey = "passwordKey";
            Guid expectedUserId = Guid.NewGuid();

            // Set up mock for password encryption
            _mockPasswordEncryption.Setup(pe => pe.EncryptionPassword(requestDto.Password))
                .Returns((encryptedPassword, passwordKey));

            // Set up mock for repository insert method
            _mockUserRepository.Setup(repo => repo.InsertUser(It.IsAny<UserModel>()))
                .ReturnsAsync(expectedUserId);

            // Act
            var result = await _userRepositoryManager.AddUserAsync(requestDto);

            // Assert
            _mockPasswordEncryption.Verify(pe => pe.EncryptionPassword(requestDto.Password), Times.Once);

            _mockUserRepository.Verify(repo => repo.InsertUser(It.Is<UserModel>(u =>
                u.Password == encryptedPassword &&
                u.PasswordKey == passwordKey
            )), Times.Once);

            // Assert the returned object is of type ResponseCreateUserDTO and check its Id
            Assert.IsType<ResponseCreateUserDTO>(result);

            Assert.Equal(expectedUserId, result.Id);

        }
    }
}