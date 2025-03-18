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
                return BadRequest(new { message = "No user data was provided." });
            }
            catch (ConstraintViolationException ex)
            {
                _logger.LogWarning("User creation failed due to constraint violation: {ConstraintType}", ex.ConstraintType);
                return UnprocessableEntity(new { message = ex.Message });
            }
            catch (ConnectionException ex)
            {
                _logger.LogError("User creation failed due to database connection error: {Exception}", ex);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = "A database connection error occurred. Please try again later." });
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error occurred while creating user: {Exception}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred while processing your request. Please try again later." });
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ResponseDeleteUserByEmailDTO>> DeleteUserByEmail([FromBody] RequestDeleteUserByEmailDTO userDeleteData)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userDeleteData);
                _logger.LogInformation("Trying to delete user with email: {Email}", userDeleteData.Email);

                ResponseDeleteUserByEmailDTO result = await _userManager.DeleteUserByEmailAsync(userDeleteData).ConfigureAwait(false);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning("Caller tried to delete user by email without providing an email. Exception: {Exception}", ex);
                return BadRequest(new { message = "The email field is required to delete a user. Please provide a valid email and try again." });
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogWarning("User with email {Email} not found when attempting to delete. Exception: {Exception}", userDeleteData?.Email, ex);
                return NotFound(new { message = $"No user was found with the email {userDeleteData?.Email}. Please check the email and try again." });
            }
            catch (ConnectionException ex)
            {
                _logger.LogError("Error when trying to connect to the database: {Exception}", ex);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = "A database connection error occurred while trying to delete the user. Please try again later." });
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error when trying to delete user with email {Email}. Exception: {Exception}", userDeleteData?.Email, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred while trying to delete the user. Please try again later." });
            }
        }

        [HttpGet]
        public async Task<ActionResult<ResponseGetUserByEmailDTO>> GetUserByEmail([FromQuery] RequestGetUserByEmailDTO userGetData)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userGetData);
                _logger.LogInformation("Request to get user with email: {Email}", userGetData.Email);

                ResponseGetUserByEmailDTO result = await _userManager.GetUserByEmailAsync(userGetData).ConfigureAwait(false);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning("Invalid request data: {Message}", ex.Message);
                return BadRequest(new { message = "Request data cannot be null." });
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogWarning("User not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ConnectionException ex)
            {
                _logger.LogError("Database connection error: {Message}", ex.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = "Service unavailable. Please try again later." });
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred. Please try again later." });
            }
        }
    }
}