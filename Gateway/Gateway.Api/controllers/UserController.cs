using Application.UserManager;
using Domain.Exceptions;
using DTOs;
using Gateway.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Api.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(ILogger<UserController> logger, IUserManager userManager) : ControllerBase
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly IUserManager _userManager = userManager;

        [HttpPost]
        public async Task<ActionResult<ResponseCreateUserDTO>> CreateUser([FromBody] RequestCreateUserDTO userCreationData)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userCreationData);
                _logger.Log(LogLevel.Information, "Adding user: " + userCreationData.ToString());
                int userId = await _userManager.AddUserAsync(userCreationData).ConfigureAwait(false);
                return Ok(new ResponseCreateUserDTO { Id = userId });
            }
            catch (ArgumentNullException)
            {
                _logger.LogWarning("Caller tried to create user without any user data");
                return Problem(
                    type: "Bad Request",
                    title: "Failed to create user, no user data was givin",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
            catch (ConnectionException ex)
            {
                _logger.LogError("User tried to register, and failed because of connection, Error: {Exception}", ex);

                return Problem(
                    type: "Internal Server Error",
                    title: "Failed to create user",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
            catch (CreateUserEmailTakenException)
            {
                _logger.LogWarning("User tried to register with a used email");
                return Problem(
                    type: "Bad Request",
                    title: "Failed Email is taken",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteUserByEmail([FromBody] DeleteUserByEmailDTO userDeleteData)
        {

            try
            {
                ArgumentNullException.ThrowIfNull(userDeleteData);
                _logger.Log(LogLevel.Information, "Trying to deleting user with email:" + userDeleteData.Email);
                bool result = await _userManager.DeleteUserByEmailAsync(userDeleteData.Email).ConfigureAwait(false);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    _logger.LogWarning("Failed to delete user with email: " + userDeleteData.Email);
                    return Problem(
                        type: "Bad Request",
                        title: "User not found",
                        statusCode: StatusCodes.Status400BadRequest
                    );
                }

            }
            catch (ArgumentNullException)
            {
                _logger.LogWarning("Caller tried to delete user by email without giving the email");
                return Problem(
                    type: "Bad Request",
                    title: "Failed to delete user, no email givin",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
            catch (ConnectionException ex)
            {
                _logger.LogError("Failed to delete user, because of connection, Error: {Exception}", ex);

                return Problem(
                    type: "Internal Server Error",
                    title: "Failed to delete user by email because connection",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
            catch (DeleteUserByEmailException ex)
            {
                _logger.LogError("Failed to delete user, because of error with delete request, Error: {Exception}", ex);

                return Problem(
                    type: "Internal Server Error",
                    title: "Failed to delete user by email, because delete logic",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}