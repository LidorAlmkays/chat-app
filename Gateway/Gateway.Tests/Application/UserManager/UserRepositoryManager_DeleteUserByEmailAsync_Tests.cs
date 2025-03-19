using Common.DTOs;
using Moq;
using Gateway.Domain.models;
using Gateway.Domain.Exceptions;
using Domain.Exceptions;

namespace Gateway.Tests.Application.UserManager
{
    public class UserRepositoryManager_DeleteUserByEmailAsync_Tests : UserRepositoryManagerBaseTest
    {
        [Fact]
        public async Task DeleteUserByEmailAsync_ShouldReturnResponse_WhenDeletionIsSuccessful()
        {
            // Arrange
            var requestDto = new RequestDeleteUserByEmailDTO { Email = "test@example.com" };

            _mockUserRepository
                .Setup(repo => repo.DeleteUserByEmailAsync(requestDto.Email))
                .ReturnsAsync(new UserModel()); // Returns a valid user model instead of bool

            // Act
            var result = await _userRepositoryManager.DeleteUserByEmailAsync(requestDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ResponseDeleteUserByEmailDTO>(result);

            _mockUserRepository.Verify(repo => repo.DeleteUserByEmailAsync(requestDto.Email), Times.Once);
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
                .Setup(repo => repo.DeleteUserByEmailAsync(requestDto.Email))
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
                .Setup(repo => repo.DeleteUserByEmailAsync(requestDto.Email))
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
                .Setup(repo => repo.DeleteUserByEmailAsync(requestDto.Email))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _userRepositoryManager.DeleteUserByEmailAsync(requestDto));

            Assert.Equal("Unexpected error", exception.Message);
        }
    }
}