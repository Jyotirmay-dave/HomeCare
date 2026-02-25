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
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeCare_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ICommonRepository<User> _userRepository;
        private readonly ICommonRepository<OTP> _otpRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserController(IEmailService emailService, IMapper mapper, ICommonRepository<User> userRepository, ICommonRepository<OTP> otpRepository, IConfiguration configuration)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _otpRepository = otpRepository;
            _mapper = mapper;
            _config = configuration;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public  async Task<ActionResult> LoginUserAsync([FromBody] UserLoginDTO dto)
        {
            if(!ModelState.IsValid) return BadRequest("Please provide Username and Email.");

            // var existingUser = await _userRepository.GetAsync(x => x.Email.ToLower().Equals(dto.Email.ToLower()));
            // if(existingUser == null)
            // {
            //     User newUser = _mapper.Map<User>(dto);
            //     await _userRepository.CreateAsync(newUser);
            // }

            var otp = new Random().Next(1000, 9999).ToString();
            OTP newOtp = new OTP
            {
                Email = dto.Email,
                OtpCode = otp,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                CreatedDate = DateTime.UtcNow
            };

            await _emailService.SendOtpAsync(dto.Email, otp);
            // await _otpRepository.CreateAsync(newOtp);
            return Ok(new { message = "OTP sent to your Email." });
        }

        [HttpPost]
        [Route("VerifyOtp")]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyOtpAsync([FromBody] OtpVerifyDTO dto)
        {
            if(!ModelState.IsValid) return BadRequest("Please enter details.");

            var record = await _otpRepository.GetAsync(x => x.Email.ToLower().Equals(dto.Email.ToLower()));
            if(record == null)  return BadRequest("OTP not found or expired.");

            if(record.ExpiresAt < DateTime.UtcNow){
                await _otpRepository.DeleteAsync(record);
                return BadRequest("OTP has expired."); 
            }  

            if(record.OtpCode != dto.OtpCode){
                await _otpRepository.DeleteAsync(record);
                return BadRequest("Invalid OTP.");
            }

            var existingUser = await _userRepository.GetAsync(x => x.Email.ToLower().Equals(record.Email.ToLower()));
            await _otpRepository.DeleteAsync(record);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, existingUser.Email),
                new Claim("id", existingUser.Id.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: creds
            );

            var response = _mapper.Map<UserDTO>(existingUser);
            return Ok(new {
                response,
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        // [HttpPost]
        // [Route("Create")]
        // [AllowAnonymous]
        // public  async Task<ActionResult> CreateUserAsync([FromBody] UserLoginDTO dto)
        // {
        //     if(!ModelState.IsValid) return BadRequest("Please enter details");
            
        //     User newUser = _mapper.Map<User>(dto);
        //     await _userRepository.CreateAsync(newUser);

        //     return Ok(new { message = "OTP sent to your Email" });
        // }
    }
}
