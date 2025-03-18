using Gateway.Application.Encryption;
using Gateway.Application.UserManager;
using Gateway.Infrastructure.UserRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Tests.Application.UserManager
{
    public abstract class UserRepositoryManagerBaseTest
    {
#pragma warning disable CA1051 // Do not declare visible instance fields
        protected readonly Mock<IUserRepository> _mockUserRepository;
        protected readonly Mock<IPasswordEncryption> _mockPasswordEncryption;
        protected readonly UserRepositoryManager _userRepositoryManager;
#pragma warning restore CA1051 // Do not declare visible instance fields

        protected UserRepositoryManagerBaseTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordEncryption = new Mock<IPasswordEncryption>();
            _userRepositoryManager = new UserRepositoryManager(_mockUserRepository.Object, _mockPasswordEncryption.Object);
        }
    }
}