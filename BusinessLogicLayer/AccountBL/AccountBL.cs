using CommonDataLayer.Entities;
using CommonDataLayer.Untilities;
using DataAccessLayer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLogicLayer
{
    public class AccountBL : BaseBL<Account>, IAccountBL
    {
        private readonly IAccountDL _accountDL;
        private readonly IConfiguration _configuration;

        public AccountBL(IAccountDL accountDL, IConfiguration configuration) : base(accountDL)
        {
            _accountDL = accountDL;
            _configuration = configuration;
        }

        public Account Authenticate(Login login)
        {
            Account account = _accountDL.Authenticate(login.Username);

            if (!EntityUntilities.VerifyPassword(login.Password, account.Password))
            {
                throw new Exception("Sai tài khoản hoặc mật khẩu");
            }

            account.Token = CreateJWT(account);
            _accountDL.SaveToken(account.AccountId, account.Token);
            return account;
        }

        public Guid Register(Account account)
        {
            string passwordHash = EntityUntilities.HashPassword(account.Password);
            account.Password = passwordHash;
            return _accountDL.Add(account);
        }

        private static string CreateJWT(Account account)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            //var secretKey = _configuration["JwtSettings:SecretKey"];
            var key = Encoding.ASCII.GetBytes("DayLaChuoiBiMatSieuCapVipProTren32KyTu123456!");
            var identity = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Role, account.Role.ToString()),
                new Claim(ClaimTypes.Name, account.FullName),
                new Claim(ClaimTypes.Email, account.Email),
            });

            var credential = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDesciptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credential
            };

            var token = jwtTokenHandler.CreateToken(tokenDesciptor);

            return jwtTokenHandler.WriteToken(token);
        }
    }
}
