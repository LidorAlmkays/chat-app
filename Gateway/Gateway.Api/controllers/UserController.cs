using Application.UserManager;
using Domain.Exceptions;
using DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Api.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(ILogger<UserController> logger, IUserManager userManager) : ControllerBase
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly IUserManager _userManager = userManager;

        // [HttpGet()]
        // public ActionResult<ResponseGetUserByEmailDTO> GetByEmail([FromQuery] RequestGetUserByEmailDTO dto)
        // {
        //     return Ok(dto);
        // }

        [HttpPost]
        public async Task<ActionResult<ResponseCreateUserDTO>> CreateUser([FromBody] RequestCreateUserDTO user)
        {
            _logger.Log(LogLevel.Information, "Adding user");
            try
            {
                int userId = await _userManager.AddUserAsync(user).ConfigureAwait(false);
                return Ok(new ResponseCreateUserDTO { Id = userId });
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
            catch (UsernameTakenException)
            {
                _logger.LogWarning("User tried to register with a used username");
                return Problem(
                    type: "Bad Request",
                    title: "Failed to create user, because username is taken.",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
        }

    }
}




//     try
//     {
//         ArgumentNullException.ThrowIfNull(user);
//         Enum.Parse<Role>(user.Role, ignoreCase: true);
//         ArgumentNullException.ThrowIfNullOrWhiteSpace(user.Email);
//         ArgumentNullException.ThrowIfNullOrWhiteSpace(user.Password);
//         ArgumentNullException.ThrowIfNullOrWhiteSpace(user.UserName);
//     }
//     catch (Exception e) when (e is ArgumentNullException || e is ArgumentException || e is InvalidOperationException)
//     {
//         return Problem(
//             type: "Bad Request",
//             title: "Invalid user input",
//             statusCode: StatusCodes.Status400BadRequest
//             );
//     }