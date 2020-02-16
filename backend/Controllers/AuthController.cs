using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Data;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Dtos;
using backend.Model;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthRepository _repo;
		private readonly IConfiguration _configure;
		public AuthController(IAuthRepository repo, IConfiguration configure)
		{
		  _configure = configure;
			_repo = repo;
		}


		[HttpPost("register")]
		public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
		{
			userRegisterDto.username = userRegisterDto.username.ToLower();

			if (await _repo.CheckUserExist(userRegisterDto.username)) BadRequest("Username already exists");

			var createUser = await _repo.Register(new User { Username = userRegisterDto.username }, userRegisterDto.password);

			return StatusCode(201);
		}
		[HttpPost("login")]
		public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
		{
			var userFromRepo = await _repo.Login(userForLoginDto.username.ToLower(), userForLoginDto.password);

			if (userFromRepo == null) return Unauthorized();

			var claims = new[]
			{
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.userId.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
			};
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configure.GetSection("AppSettings:Token").Value));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = creds
			};

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return Ok(new {
				token = tokenHandler.WriteToken(token)
			});
		}
	}
}