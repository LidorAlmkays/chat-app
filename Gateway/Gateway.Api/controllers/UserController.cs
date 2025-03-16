using Gateway.Application.UserManager;
using Domain.Exceptions;
using Common.DTOs;
using Gateway.Domain.Exceptions;
using Gateway.Domain.Exceptions.SpecificConstraint;
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

                ResponseCreateUserDTO result = await _userManager.AddUserAsync(userCreationData).ConfigureAwait(false);
                return Ok(result);
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
        public async Task<ActionResult<ResponseDeleteUserByEmailDTO>> DeleteUserByEmail([FromBody] RequestDeleteUserByEmailDTO userDeleteData)
        {

            try
            {
                ArgumentNullException.ThrowIfNull(userDeleteData);
                _logger.Log(LogLevel.Information, "Trying to deleting user with email:" + userDeleteData.Email);
                ResponseDeleteUserByEmailDTO result = await _userManager.DeleteUserByEmailAsync(userDeleteData).ConfigureAwait(false);

                return Ok(result);

            }
            catch (ArgumentNullException ex) // If email is not provided
            {
                _logger.LogWarning("Caller tried to delete user by email without providing an email. Exception: {Exception}", ex);

                return Problem(
                    type: "Bad Request",
                    title: "Failed to delete user, no email provided",
                    detail: "The email field is required to delete a user. Please provide a valid email and try again.",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
            catch (UserNotFoundException ex) // If the user was not found
            {
                _logger.LogWarning("User with email {Email} not found when attempting to delete. Exception: {Exception}", userDeleteData?.Email, ex);

                return Problem(
                    type: "Not Found",
                    title: "User not found",
                    detail: $"No user was found with the email {userDeleteData?.Email}. Please check the email and try again.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }
            catch (ConnectionException ex) // If there is a connection issue
            {
                _logger.LogError("Error when trying to connect to the database: {Exception}", ex);

                return Problem(
                    type: "Internal Server Error",
                    title: "Database connection error",
                    detail: "A database connection error occurred while trying to delete the user. Please try again later.",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
            catch (Exception ex) // General exception if something unexpected occurs
            {
                _logger.LogError("Unexpected error when trying to delete user with email {Email}. Exception: {Exception}", userDeleteData?.Email, ex);

                return Problem(
                    type: "Internal Server Error",
                    title: "Unexpected error",
                    detail: "An unexpected error occurred while trying to delete the user. Please try again later.",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}