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
using Microsoft.Extensions.Logging;

namespace Account.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize(Policy = "MustBelongToHR")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAccountValidation _accountValidation;
        private readonly ILogger<AccountController> _logger;

        public Credential Credential { get; set; }
        public string AppSource { get; set; }

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponsetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = null)]
        public async Task<ActionResult<RegisterResponsetDto>> RegisterAsync(RegisterRequestDto createAccountDto, CancellationToken cancellationToken = default)
        {
            try
            {
                _accountValidation.Validate(createAccountDto);

                GetAppSource();

                var response = await _accountService.RegisterAsync(createAccountDto, AppSource, cancellationToken);

                return new OkObjectResult(response);
            }
            catch (AccountException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

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

        /// <summary>
        ///     Send recovery email code to reset password.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>TBD</returns>
        /// <response code="200">Ok.</response>
        /// <response code="400">Bad Request.</response>
        [HttpGet]
        [Route("/forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = null)]
        public async Task<ActionResult<string>> ForgotPasswordAsync([FromQuery]string email, CancellationToken cancellationToken = default)
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
