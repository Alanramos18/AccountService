using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Account.Business.Exceptions;
using Account.Business.Services.Interfaces;
using Account.Dto.WebDtos;
using Account.Web.Validations.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;

namespace Account.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAccountValidation _accountValidation;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, IAccountValidation accountValidation, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _accountValidation = accountValidation;
            _logger = logger;
            
            Credential = new Credential
            {
                UserName = "alanramos",
                Password = "1234"
            };
        }

        #region Register
        
        /// <summary>
        ///     Create new accounts.
        /// </summary>
        /// <param name="createAccountDto">Create account dto</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Created account dto</returns>
        /// <response code="200">Ok.</response>
        /// <response code="400">Bad Request.</response>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegisterResponsetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegisterResponsetDto>> RegisterAsync(RegisterRequestDto createAccountDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var confirmationLink = Url.Action("ConfirmEmail", "Account", null, Request.Scheme);

                var response = await _accountService.RegisterAsync(createAccountDto, confirmationLink, GetAppSource(), cancellationToken);

                if (response == null)
                {
                    return new BadRequestObjectResult("That email is already registered.");
                }

                return new CreatedAtRouteResult("RegisterAsync", response);
            }
            catch (AccountException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        ///     Confirm email validation.
        /// </summary>
        /// <param name="email">User Email</param>
        /// <param name="token">Token</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Created account dto</returns>
        /// <response code="200">Ok.</response>
        /// <response code="400">Bad Request.</response>
        [HttpGet]
        [Route("confirm-email")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegisterResponsetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegisterResponsetDto>> ConfirmEmailAsync(string email, string token, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _accountService.ConfirmEmailAsync(email, token, GetAppSource(), cancellationToken);

                if (result == null || result.Errors.Any())
                {
                    // We should not return nothing else than ok due to security.
                    // We should log something tho.
                    _logger.LogInformation($"The email: {email} try confirm itself.");
                }

                // May be we should return a simple html at least.
                return new OkObjectResult("Confirmed email");
            }
            catch (AccountException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        #endregion

        #region Login

        /// <summary>
        ///     Login users.
        /// </summary>
        /// <param name="dto">Login dto</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>JWT Token</returns>
        /// <response code="200">Ok.</response>
        /// <response code="400">Bad Request.</response>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> LoginAsync([FromBody]LoginDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _accountService.LoginAsync(dto, GetAppSource(), cancellationToken);

                return new OkObjectResult(response);
            }
            catch (AccountException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        #endregion

        #region Forgot Password

        /// <summary>
        ///     Send recovery email code to reset password.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        /// <response code="200">Ok.</response>
        /// <response code="400">Bad Request.</response>
        [HttpGet]
        [Route("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPasswordAsync([FromQuery] string email, CancellationToken cancellationToken = default)
        {
            try
            {
                _accountValidation.ValidateEmail(email);

                await _accountService.ForgotPasswordAsync(email, GetAppSource(), cancellationToken);

                return new OkResult();
            }
            catch (AccountException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        ///     Verify the reset code.
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="code">Reset password code</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>TBD</returns>
        /// <response code="200">Ok.</response>
        /// <response code="400">Bad Request.</response>
        [HttpPost]
        [Route("verify-reset")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> VerifyResetCodeAsync([FromBody] VerifyResetRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Code))
                {
                    return new BadRequestObjectResult("The code cannot be empty");
                }

                var result = await _accountService.VerifyResetCodeAsync(request.Email, GetAppSource(), request.Code, cancellationToken);

                if (string.IsNullOrEmpty(result))
                {
                    return new BadRequestResult();
                }

                return new OkObjectResult(result);
            }
            catch (AccountException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        ///     Changes account password.
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="newPassword">New password to change</param>
        /// <param name="token">Reset password token</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>TBD</returns>
        /// <response code="200">Ok.</response>
        /// <response code="400">Bad Request.</response>
        [HttpPost]
        [Route("change-password")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> ChangePasswordAsync([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Token))
                {
                    return new BadRequestObjectResult("The code cannot be empty");
                }

                await _accountService.ChangePasswordAsync(request.Email, GetAppSource(), request.NewPassword, request.Token, cancellationToken);

                return new OkResult();
            }
            catch (AccountException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        #endregion

        private async Task VerifyUserAsync()
        {
            if (Credential.UserName == "alanramos" && Credential.Password == "1234")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "alan"),
                    new Claim(ClaimTypes.Email, "alanramos@hotmail.com"),
                    new Claim("Department", "HR")
                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);
            }
        }

        private string GetAppSource()
        {
            HttpContext.Items.TryGetValue("Application", out var code);
            Enum.TryParse(code as String, out Business.Utils.Constants.ApplicationCode source);

            return source.ToString();
        }
    }
}
