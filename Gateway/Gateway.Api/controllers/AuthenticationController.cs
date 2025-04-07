using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Common.DTOs;
using Common.Enums;
using Gateway.Application.AuthenticationManager;
using Gateway.Application.TokenManager;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gateway.Api.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationManager authenticationManager) : Controller
    {
        private readonly ILogger<AuthenticationController> _logger = logger;
        private readonly IAuthenticationManager _authenticationManager = authenticationManager;

        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<ResponseLoginDTO>> Login([FromBody] RequestLoginDTO loginData)
        {
            _logger.LogInformation("User logging in, data: " + loginData);
            var response = await _authenticationManager.Login(loginData).ConfigureAwait(false);
            return Ok(response);
        }

        [Route("user-role")]
        [HttpGet]
        public async Task<ActionResult<ResponseUserRoleFromTokenDTO>> UserRole()
        {
            var token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
                return Unauthorized("No token found");
            return Ok(await _authenticationManager.GetUserRoleFromToken(token).ConfigureAwait(false));
        }


        [Route("google-login")]
        [HttpGet]
        public async Task GoogleLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
             new AuthenticationProperties
             {
                 RedirectUri = Url.Action("GoogleResponse")
             }).ConfigureAwait(false);
        }

        [Route("GoogleResponse")]
        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
            var claims = result.Principal?.Identities.FirstOrDefault()?.Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });
            return Json(claims);
        }
    }
}