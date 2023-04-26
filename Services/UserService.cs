using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Collections;

using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.DTO;


namespace MobileBasedCashFlowAPI.Services
{

    public class UserService : UserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SendMailRepository _sendMailService;
        private readonly MobileBasedCashFlowGameContext _context;

        public UserService(MobileBasedCashFlowGameContext context,
                           IConfiguration configuration,
                           SendMailRepository sendMailService)
        {
            _configuration = configuration;
            _sendMailService = sendMailService;
            _context = context;
        }

        public async Task<object> Authenticate(LoginRequest request)
        {
            var user = await _context.UserAccounts.SingleOrDefaultAsync(x => x.UserName == request.UserName);
            if (user == null)
            {
                return Constant.NotFound;
            }
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isValidPassword)
            {
                return Constant.WrongPassword;
            }
            // find role name by id
            var role = await _context.UserRoles
                                .Where(r => r.RoleId == user.RoleId)
                                .Select(r => new { roleName = r.RoleName }).FirstOrDefaultAsync();
            if (role == null)
            {
                return "Can not found this role";
            }
            if (user.Coin == null)
            {
                user.Coin = 0;
            }
            if (user.Address == null)
            {
                user.Address = "";
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Role , role.roleName),
                new Claim("Id" , user.UserId.ToString()),
                new Claim("Status" , user.Status.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);

            return new
            {
                user = new
                {
                    user.UserId,
                    user.NickName,
                    user.Email,
                    user.Address,
                    user.Coin,
                    user.ImageUrl,
                    user.Phone,
                    user.Gender,
                    role.roleName,
                },
                token = stringToken,
            };
        }

        public async Task<string> Register(RegisterRequest request)
        {
            var user = new UserAccount()
            {
                UserName = request.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                NickName = "",
                Gender = "Male",
                Email = request.Email,
                Phone = "",
                ImageUrl = "",
                Coin = 0,
                CreateAt = DateTime.UtcNow,
                EmailConfirmToken = GenerateEmailConfirmationToken(),
                RoleId = null,
                Status = true,

            };

            var checkUser = await _context.UserAccounts.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            var checkMail = await _context.UserAccounts.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (checkUser != null)
            {
                return "This username existed";
            }
            else if (checkMail != null)
            {
                return "This email has used already";
            }
            else
            {
                // Find role "Player" in database
                var roleId = await (from role in _context.UserRoles
                                    where role.RoleName == "Player"
                                    select new { roleId = role.RoleId }).AsNoTracking().FirstOrDefaultAsync();
                // Find gameId in database that match version == "ver_1"
                var gameServerId = await (from game in _context.GameServers
                                          where game.GameVersion == "Ver_1"
                                          select new { gameId = game.GameServerId }).AsNoTracking().FirstOrDefaultAsync();

                if (roleId != null && gameServerId != null)
                {
                    try
                    {
                        // Add role to user
                        user.GameServerId = gameServerId.gameId;
                        user.RoleId = roleId.roleId;
                        await _context.UserAccounts.AddAsync(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return ex.ToString();
                    }
                    // Create a url path for user to cofirm account in mail

                    //var uriBuilder = new UriBuilder(_configuration["ReturnPaths:VerifyEmail"]);
                    //var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                    //query["token"] = user.EmailConfirmToken;
                    //query["userid"] = user.UserId.ToString();
                    //uriBuilder.Query = query.ToString();
                    //var urlString = uriBuilder.ToString();

                    //var htmlBody = string.Format(@"<div style='text-align:center;'>
                    //                            <h1>Welcome to our Web Site</h1>
                    //                            <h3>Click below button for verify your Email Id</h3>
                    //                            <a href ='" + urlString + "' type = 'submit' " +
                    //                                "style='display: block;" +
                    //                                "text-align: center;" +
                    //                                "font-weight: bold;" +
                    //                                "background-color: #008CBA;" +
                    //                                "font-size: 16px;border-radius: 10px;" +
                    //                                "color:#ffffff;" +
                    //                                "cursor:pointer;" +
                    //                                "width:100%;" +
                    //                                "padding:10px;'>" +
                    //                                "Confirm Mail" +
                    //                                "</a>" +
                    //                                "</div>)");

                    //MailContent mailContent = new MailContent
                    //{
                    //    To = user.Email,
                    //    Subject = "Confirm Email",
                    //    Body = htmlBody,
                    //};

                    //// call method to send mail
                    //var mailMessage = await _sendMailService.SendMail(mailContent);

                    return Constant.Success;
                }
                return Constant.Failed;
            }
        }

        public async Task<string> VerifyEmail(string token)
        {
            var user = _context.UserAccounts.FirstOrDefault(u => u.EmailConfirmToken == token);

            if (user == null)
            {
                return "Invalid Token";
            }
            if (!user.VerifyAt.HasValue)
            {
                return "Email has verify already";
            }
            user.VerifyAt = DateTime.Now;
            user.UpdateAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Constant.Success;
        }

        public async Task<bool> ForgotPassword(string email)
        {
            var user = _context.UserAccounts.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.PasswordResetToken = GenerateEmailConfirmationToken();
                user.ResetTokenExpire = DateTime.Now.AddDays(1);
                user.UpdateAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ResetPassword(ResetPasswordRequest request)
        {
            var user = _context.UserAccounts.FirstOrDefault(u => u.PasswordResetToken == request.Token);
            // 02/03/2023 > 02/02/2023
            if (user != null && user.ResetTokenExpire >= DateTime.Now)
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                user.PasswordResetToken = null;
                user.ResetTokenExpire = null;
                user.UpdateAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public string GenerateEmailConfirmationToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<IEnumerable> GetAsync()
        {

            var users = await (from user in _context.UserAccounts
                               join role in _context.UserRoles on user.RoleId equals role.RoleId
                               select new
                               {
                                   userId = user.UserId,
                                   userName = user.UserName,
                                   userRole = role.RoleName,
                               }).ToListAsync();
            return users;
        }

        public async Task<string> EditProfile(int userId, EditProfileRequest request)
        {
            var oldProfile = await _context.UserAccounts.FirstOrDefaultAsync(i => i.UserId == userId);
            if (oldProfile != null)
            {
                oldProfile.NickName = request.NickName;
                oldProfile.Gender = request.Gender;
                oldProfile.Phone = request.Phone;
                oldProfile.Email = request.Email;
                oldProfile.ImageUrl = request.ImageUrl;
                oldProfile.UpdateAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return Constant.Success;

            }
            return Constant.Failed;
        }

        public async Task<object?> ViewProfile(int userId)
        {
            var user = await (from u in _context.UserAccounts
                              where u.UserId == userId
                              select new
                              {
                                  u.NickName,
                                  u.Gender,
                                  u.Phone,
                                  u.Email,
                                  u.ImageUrl,
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<string> UpdateCoin(int userId, int coin)
        {
            var oldProfile = await _context.UserAccounts.FirstOrDefaultAsync(i => i.UserId == userId);
            if (oldProfile != null)
            {
                if (!ValidateInput.isNumber(coin.ToString()))
                {
                    return "Coin must be number";
                }
                //else if
                oldProfile.Coin += coin;
                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return Constant.Failed;
        }

        public async Task<object?> FindUserById(int userId)
        {
            var user = await (from u in _context.UserAccounts
                              where u.UserId == userId
                              select new
                              {
                                  u.NickName,
                                  u.Gender,
                                  u.ImageUrl,
                              }).FirstOrDefaultAsync();
            return user;
        }
    }
}
