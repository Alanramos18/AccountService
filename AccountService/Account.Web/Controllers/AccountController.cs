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
    [Route("[controller]")]
    //[Authorize(Policy = "MustBelongToHR")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAccountVerificationService _accountVerificationService;
        private readonly IAccountValidation _accountValidation;
        private readonly ILogger<AccountController> _logger;

        public Credential Credential { get; set; }
        public string AppSource { get; set; }

        public AccountController(IAccountService accountService, IAccountVerificationService accountVerificationService, IAccountValidation accountValidation, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _accountVerificationService = accountVerificationService;
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
        [Route("/register")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegisterResponsetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = null)]
        public async Task<ActionResult<RegisterResponsetDto>> RegisterAsync(RegisterRequestDto createAccountDto, CancellationToken cancellationToken = default)
        {
            try
            {
                GetAppSource();

                var response = await _accountService.RegisterAsync(createAccountDto, AppSource, cancellationToken);

                if (response == null)
                {
                    return NotFound();
                }

                // Not working for some reason
                //var confirmationLink = Url.Action(nameof(ConfirmEmailAsync), "Account", null);

                //var confirmationLink = Url.RouteUrl(nameof(ConfirmEmailAsync), new { email = response.Email }, Request.Scheme);

                //UrlHelper u = new UrlHelper(ControllerContext.HttpContext.);
                //string confirmationLink = u.Action("About", "Home", null);

                var confirmationLink = $"http://localhost:61768/confirm-email?email={response.Email}";

                await _accountService.SendVerificationEmailAsync(response, confirmationLink, cancellationToken);

                return new OkObjectResult(response);
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
        [Route("/confirm-email")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegisterResponsetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = null)]
        public async Task<ActionResult<RegisterResponsetDto>> ConfirmEmailAsync(string email, string token, CancellationToken cancellationToken = default)
        {
            try
            {
                GetAppSource();

                var result = await _accountService.ConfirmEmailAsync(email, token, AppSource, cancellationToken);

                if (result.Errors.Any())
                {
                    return NotFound();
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
        [Route("/login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = null)]
        public async Task<ActionResult<string>> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                //_accountValidation.ValidateLogin(dto);
                GetAppSource();

                var response = await _accountService.LoginAsync(dto, AppSource, cancellationToken);

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
        [HttpPost]
        [Route("/forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = null)]
        public async Task<IActionResult> ForgotPasswordAsync([FromQuery] string email, CancellationToken cancellationToken = default)
        {
            try
            {
                _accountValidation.ValidateEmail(email);
                GetAppSource();

                await _accountService.ForgotPasswordAsync(email, AppSource, cancellationToken);

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
        /// <param name="code">Reset password code</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>TBD</returns>
        /// <response code="200">Ok.</response>
        /// <response code="400">Bad Request.</response>
        [HttpGet]
        [Route("/verify-reset")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = null)]
        public async Task<ActionResult<string>> VerifyResetCodeAsync([FromQuery] string code, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return new BadRequestObjectResult("The code cannot be empty");
                }
                
                GetAppSource();

                //await _accountService.ForgotPasswordAsync(email, AppSource, cancellationToken);

                return new OkResult();
            }
            catch (AccountException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        #endregion



        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            //await VerifyUserAsync();
            GetAppSource();

            var response = await _accountService.LoginAsync(new LoginDto(), AppSource, new CancellationToken());

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55)
            })
            .ToArray();
        }

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

        private void GetAppSource()
        {
            HttpContext.Items.TryGetValue("Application", out var code);
            Enum.TryParse(code as String, out Business.Utils.Constants.ApplicationCode source);
            AppSource = source.ToString();
        }
    }

    public class Credential
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
