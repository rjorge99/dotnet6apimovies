using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MoviesApi.Context;
using MoviesApi.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : CustomBaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public AccountsController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration, DataContext context, IMapper mapper) : base(context, mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _signInManager = signInManager;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserToken>> Register(UserInfo userInfo)
        {
            var user = new IdentityUser() { UserName = userInfo.Email, Email = userInfo.Email };
            var result = await _userManager.CreateAsync(user, userInfo.Password);

            if (result.Succeeded)
            {
                var response = await CreateToken(userInfo);
                return response;
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> Login(UserInfo userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var response = await CreateToken(userInfo);
                return response;
            }

            return BadRequest("Login error");
        }

        [HttpPost("Renew")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserToken>> Renew()
        {
            var emailClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "email");
            var email = emailClaim?.Value;


            var userCredentials = new UserInfo() { Email = email };
            var response = await CreateToken(userCredentials);
            return response;
        }

        [HttpPost("AssignRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserToken>> AssignRole(EditUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null) return NotFound();

            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, dto.Rol));
            return NoContent();
        }

        [HttpPost("RemoveRol")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserToken>> RemoveRol(EditUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null) return NotFound();

            await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, dto.UserId));
            return NoContent();
        }

        private async Task<UserToken> CreateToken(UserInfo userInfo)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim(ClaimTypes.Email, userInfo.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddYears(1);

            var user = await _userManager.FindByEmailAsync(userInfo.Email);
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            var claimsDb = await _userManager.GetClaimsAsync(user);
            claims.AddRange(claimsDb);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration,
                signingCredentials: credentials);

            return new UserToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiration
            };

        }

        [HttpGet("users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<List<UserDto>>> Get([FromQuery] PaginationDto pagination)
        {
            var queryable = _context.Users.AsQueryable();
            queryable = queryable.OrderBy(u => u.Email);
            return await Get<IdentityUser, UserDto>(pagination);

        }

    }
}
