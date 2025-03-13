using Application.Encryption;
using Application.mapping;
using Application.UserManager;
using DTOs;
using Gateway.Domain.models;
using Gateway.Infrastructure.UserRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

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
            //Arrange
            var requestDto = new RequestCreateUserDTO
            {
                Age = 3,
                Email = "",
                Password = "",
                Username = ""
            };
            var userModel = requestDto.ToUserModelAsUser(); // Convert DTO to Model
            var encryptedPassword = "hashedPassword";
            var passwordKey = "passwordKey";
            var expectedUserId = Guid.NewGuid();

            _mockPasswordEncryption.Setup(pe => pe.EncryptionPassword(requestDto.Password))
                .Returns((encryptedPassword, passwordKey));

            _mockUserRepository.Setup(repo => repo.InsertUser(It.IsAny<UserModel>()))
                .ReturnsAsync(expectedUserId);
            //Act
            var result = await _userRepositoryManager.AddUserAsync(requestDto);
            //Assert
            _mockPasswordEncryption.Verify(pe => pe.EncryptionPassword(requestDto.Password), Times.Once);

            _mockUserRepository.Verify(repo => repo.InsertUser(It.Is<UserModel>(u =>
                u.Password == encryptedPassword &&
                u.PasswordKey == passwordKey
            )), Times.Once);

            Assert.Equal(expectedUserId, result);

        }
    }
}