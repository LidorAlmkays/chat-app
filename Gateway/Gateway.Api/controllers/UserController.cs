using Application.UserManager;
using Domain.Exceptions;
using DTOs;
using Gateway.Domain.Exceptions;
using Gateway.Domain.Exceptions.database;
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
                _logger.LogInformation("Adding user with the following data: {@UserData}", userCreationData);

                Guid userId = await _userManager.AddUserAsync(userCreationData).ConfigureAwait(false);
                return Ok(new ResponseCreateUserDTO { Id = userId });
            }
            catch (ArgumentNullException)
            {
                _logger.LogWarning("Caller tried to create a user without providing any user data.");
                return Problem(
                    type: "Bad Request",
                    title: "Failed to create user",
                    detail: "No user data was provided.",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
            catch (ConstraintViolationException ex)
            {
                _logger.LogWarning("User creation failed due to constraint violation: {ConstraintType}", ex.ConstraintType);

                return Problem(
                    type: "Unprocessable Entity",
                    title: "Failed to create user due to business rule violation",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status422UnprocessableEntity
                );
            }
            catch (ConnectionException ex)
            {
                _logger.LogError("User creation failed due to database connection error: {Exception}", ex);

                return Problem(
                    type: "Internal Server Error",
                    title: "Failed to create user",
                    detail: "A database connection error occurred. Please try again later.",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error occurred while creating user: {Exception}", ex);

                return Problem(
                    type: "Internal Server Error",
                    title: "Unexpected error occurred",
                    detail: "An unexpected error occurred while processing your request. Please try again later.",
                    statusCode: StatusCodes.Status500InternalServerError
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