using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using ProductManagerAPI.Data;
using ProductManagerAPI.Dtos;
using ProductManagerAPI.Helpers;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProductManagerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    // jwt token is required
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;
        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                string sqlCheckUserExits = "SELECT Email FROM ProductsSchema.Users WHERE Email = '" + userForRegistration.Email + "'";

                IEnumerable<string> existingUsers = _dapper.LoadData<string>(sqlCheckUserExits);

                //if (existingUsers.Count() == 0)
                if (!existingUsers.Any())
                {
                    byte[] passwordSalt = new byte[128 / 8];

                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }

                    byte[] passwordHash = _authHelper.GetPasswordHash(userForRegistration.Password, passwordSalt);

                    string sqlAddUser = @"INSERT INTO [ProductsSchema].[Users]
                        (Username, 
                        Email, 
                        PasswordHash, 
                        PasswordSalt) VALUES ('" +
                        userForRegistration.Username + "','" +
                        userForRegistration.Email +
                        "', @PasswordHash, @PasswordSalt)";

                    //List<SqlParameter> sqlParameters = new List<SqlParameter>();
                    List<SqlParameter> sqlParameters = new();

                    //SqlParameter passwordHashParameter = new("@PasswordHash", SqlDbType.VarBinary);
                    //passwordHashParameter.Value = passwordHash;

                    SqlParameter passwordHashParameter = new("@PasswordHash", SqlDbType.VarBinary)
                    {
                        Value = passwordHash
                    };

                    SqlParameter passwordSaltParameter = new("@PasswordSalt", SqlDbType.VarBinary)
                    {
                        Value = passwordSalt
                    };

                    sqlParameters.Add(passwordHashParameter);
                    sqlParameters.Add(passwordSaltParameter);

                    if (_dapper.ExecuteSqlWithParameters(sqlAddUser, sqlParameters))
                    {
                        return Ok();
                    }
                    throw new Exception("Failed to register the new user");
                }
                throw new Exception("User with this email already exists");
            }
            throw new Exception("Mismatched passwords");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            string sqlForHashAndSalt = @"SELECT * FROM ProductsSchema.Users WHERE Email = '" +
                userForLogin.Email + "'";

            UserForLoginConfirmationDto userForLoginConfirmation = _dapper
                .LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt);

            if (userForLoginConfirmation == null)
            {
                return StatusCode(401, "Invalid email");
            }
            byte[] passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForLoginConfirmation.PasswordSalt);

            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != userForLoginConfirmation.PasswordHash[i])
                {
                    return StatusCode(401, "Incorrect password");
                }
            }
            string sqlUserId = @"SELECT UserId FROM ProductsSchema.Users WHERE Email = '" +
                userForLogin.Email + "'";
            int userId = _dapper.LoadDataSingle<int>(sqlUserId);

            return Ok(new Dictionary<string, string>
            {
                {
                    "token", _authHelper.CreateToken(userId)
                }
            });
        }

        [HttpGet("RefreshToken")]
        public string RefreshToken()
        {
            string sqlGetUserId = @"SELECT UserId FROM ProductsSchema.Users WHERE UserId = '" +
                User.FindFirst("userId")?.Value + "'";

            int userId = _dapper.LoadDataSingle<int>(sqlGetUserId);
            return _authHelper.CreateToken(userId);
        }
    }
}
