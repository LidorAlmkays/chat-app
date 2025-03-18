using Common.DTOs;
using Domain.Exceptions;
using Gateway.Domain.Exceptions;
using Gateway.Domain.models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Gateway.Tests.Application.UserManager
{
    public class UserRepositoryManager_GetUserByEmailAsync_Tests : UserRepositoryManagerBaseTest
    {
        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var requestDto = new RequestGetUserByEmailDTO { Email = "test@example.com" };
            var userModel = new UserModel
            {
                Id = Guid.NewGuid(),
                Email = requestDto.Email,
                Username = "TestUser",
                Birthday = new DateTime(1995, 5, 23)
            };

            _mockUserRepository
                .Setup(repo => repo.GetUserByEmail(requestDto.Email))
                .ReturnsAsync(userModel);

            // Act
            var result = await _userRepositoryManager.GetUserByEmailAsync(requestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userModel.Email, result.Email);
            Assert.Equal(userModel.Username, result.Username);
            Assert.Equal(userModel.Birthday, result.Birthday);
            _mockUserRepository.Verify(repo => repo.GetUserByEmail(requestDto.Email), Times.Once);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userRepositoryManager.GetUserByEmailAsync(null!));
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var requestDto = new RequestGetUserByEmailDTO { Email = "nonexistent@example.com" };

            _mockUserRepository
                .Setup(repo => repo.GetUserByEmail(requestDto.Email))
                .ThrowsAsync(new UserNotFoundException());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UserNotFoundException>(() => _userRepositoryManager.GetUserByEmailAsync(requestDto));
            Assert.Equal("User not found inside registry", exception.Message);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldThrowConnectionException_WhenDatabaseConnectionFails()
        {
            // Arrange
            var requestDto = new RequestGetUserByEmailDTO { Email = "test@example.com" };

            _mockUserRepository
                .Setup(repo => repo.GetUserByEmail(requestDto.Email))
                .ThrowsAsync(new ConnectionException());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ConnectionException>(() => _userRepositoryManager.GetUserByEmailAsync(requestDto));
            Assert.Equal("Failed to connect to the registry", exception.Message);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldThrowException_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            var requestDto = new RequestGetUserByEmailDTO { Email = "test@example.com" };

            _mockUserRepository
                .Setup(repo => repo.GetUserByEmail(requestDto.Email))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _userRepositoryManager.GetUserByEmailAsync(requestDto));
            Assert.Equal("Unexpected error", exception.Message);
        }
    }
}