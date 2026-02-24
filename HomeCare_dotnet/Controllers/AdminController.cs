using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using HomeCare_dotnet.Data;
using HomeCare_dotnet.Data.Repository;
using HomeCare_dotnet.DTOs;
using HomeCare_dotnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeCare_dotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ICommonRepository<Admin> _adminRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _config;

        public AdminController(IMapper mapper, ICommonRepository<Admin> adminrepository, IPasswordService passwordService, IConfiguration configuration)
        {
            _adminRepository = adminrepository;
            _mapper = mapper;
            _passwordService = passwordService;
            _config = configuration;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> LoginAdminAsync(AdminLoginDTO dto)
        {
            if(!ModelState.IsValid) return BadRequest("Please provide Email and Password");

            var existingAdmin = await _adminRepository.GetAsync(x => x.Email.ToLower().Equals(dto.Email.ToLower()));
            if(existingAdmin == null)   return Unauthorized();

            var salt = Convert.FromBase64String(existingAdmin.PasswordSalt);
            var hashedPassword = _passwordService.HashPassword(dto.Password, salt);

            if(hashedPassword != existingAdmin.Password)    return Unauthorized();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, existingAdmin.Email),
                new Claim("id", existingAdmin.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: creds);

            var response = _mapper.Map<AdminDTO>(existingAdmin);
            return Ok(new { 
                response, 
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task<ActionResult> CreateAdminAsync([FromBody] AdminCreateDTO dto)
        {
            if(dto == null) return BadRequest("Please enter details");

            var salt = _passwordService.GenerateSalt();
            var hashedPassword = _passwordService.HashPassword(dto.Password, salt);

            Admin newAdmin = _mapper.Map<Admin>(dto);
            newAdmin.Password = hashedPassword;
            newAdmin.PasswordSalt = Convert.ToBase64String(salt);
            newAdmin.IsActive = true;
            newAdmin.IsDeleted = false;
            newAdmin.CreatedDate = DateTime.UtcNow;
            newAdmin.ModifiedDate = DateTime.UtcNow;

            var adminAtCreation = await _adminRepository.CreateAsync(newAdmin);

            return Ok(adminAtCreation);
        }

        [HttpGet]
        [Route("All")]
        [Authorize]
        public async Task<ActionResult<List<AdminDTO>>> GetAllAdminAsync()
        {
            var admins = await _adminRepository.GetAllAsync();
            var result = _mapper.Map<List<AdminDTO>>(admins);
            return Ok(result);
        }

        // [HttpPost]
        // [Route("ForgotPassword")]
        // [AllowAnonymous]
        // public ActionResult ResetPassword([FromBody] )
    }
}
