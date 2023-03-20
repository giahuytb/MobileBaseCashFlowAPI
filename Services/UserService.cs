using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Collections;

using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.DTO;


namespace MobileBasedCashFlowAPI.Services
{

    public class UserService : IUserService
    {
        public const string SUCCESS = "success";
        public const string FAILED = "failed";
        public const string NOTFOUND = "not found";

        private readonly IConfiguration _configuration;
        private readonly ISendMailService _sendMailService;
        private readonly ILoginHistoryService _loginHistoryService;
        private readonly MobileBasedCashFlowGameContext _context;

        public UserService(MobileBasedCashFlowGameContext context,
                           IConfiguration configuration,
                           ISendMailService sendMailService,
                           ILoginHistoryService loginHistoryService)
        {
            _configuration = configuration;
            _sendMailService = sendMailService;
            _context = context;
            _loginHistoryService = loginHistoryService;
        }

        public async Task<object> Authenticate(LoginRequest request)
        {
            var user = await _context.UserAccounts.SingleOrDefaultAsync(x => x.UserName == request.UserName);
            if (user == null)
            {
                return "User not found";
            }
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isValidPassword)
            {
                return "Wrong password";
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
            var claims = new[]
            {
                new Claim(ClaimTypes.Email , user.Email),
                new Claim(ClaimTypes.Role , role.roleName),
                new Claim("NickName" , user.NickName),
                new Claim("UserName" , user.UserName),
                new Claim("Email", user.Email),
                new Claim("AvatarImageUrl", user.AvatarImageUrl),
                new Claim("Gender", user.Gender),
                new Claim("Id" , user.UserId.ToString()),
                new Claim("Status" , user.Status.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);

            // write login date time when user Login
            var loginId = await _loginHistoryService.WriteLog(user.UserId);

            return new
            {
                user = new
                {
                    user.NickName,
                    user.Email,
                    user.Coin,
                    user.AvatarImageUrl,
                    user.Phone,
                    user.Gender,
                    role.roleName,
                    loginId,
                },
                token = stringToken,
            };
        }

        public async Task<string> Register(RegisterRequest request)
        {
            var user = new UserAccount()
            {
                UserId = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                NickName = request.NickName,
                Gender = request.Gender,
                Email = request.Email,
                Phone = request.Phone,
                Coin = 0,
                AvatarImageUrl = request.ImageUrl,
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
            else if (!ValidateInput.isEmail(request.Email) || request.Email.Equals(""))
            {
                return "You need to fill your email";
            }
            else if (request.UserName.Equals(""))
            {
                return "You need to fill your username";
            }
            else if (request.Password.Equals("") || request.Password.Length < 8)
            {
                return "Your password must be at least 8 character";
            }
            else if (request.NickName.Equals(""))
            {
                return "You need to fill your nickname";
            }
            else if (!ValidateInput.isPhone(request.Phone))
            {
                return "Your phone number is not correct";
            }
            else if (request.ConfirmPassword.Equals(""))
            {
                return "Please confirm your password";
            }
            else if (!request.Password.Equals(request.ConfirmPassword))
            {
                return "Your confirm password must be the same with password";
            }
            else
            {
                // Find role "Player" in database
                var roleId = await (from role in _context.UserRoles
                                    where role.RoleName == "Player"
                                    select new { roleId = role.RoleId }).FirstOrDefaultAsync();
                // Find gameId in database that match version == "Ver_1"
                var gameId = await (from game in _context.Games
                                    where game.GameVersion == "Ver_1"
                                    select new { gameId = game.GameId }).FirstOrDefaultAsync();

                if (roleId != null && gameId != null)
                {
                    try
                    {
                        // Add role to user
                        user.GameId = gameId.gameId;
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

                    return SUCCESS;
                }
            }
            return FAILED;
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

            return SUCCESS;
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
            try
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
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> EditProfile(string userId, EditProfileRequest request)
        {
            var oldProfile = await _context.UserAccounts.FirstOrDefaultAsync(i => i.UserId == userId);
            if (oldProfile != null)
            {
                try
                {
                    if (request.NickName.Equals(""))
                    {
                        return "You need to fill your nickname";
                    }
                    else if (!ValidateInput.isPhone(request.Phone))
                    {
                        return "You need to enter the the correct phone with 10 number";
                    }

                    oldProfile.NickName = request.NickName;
                    oldProfile.Gender = request.Gender;
                    oldProfile.Phone = request.Phone;
                    oldProfile.Email = request.Email;
                    oldProfile.AvatarImageUrl = request.ImageUrl;
                    oldProfile.UpdateAt = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    if (!UserExists(userId))
                    {
                        return NOTFOUND;
                    }
                    return ex.ToString();
                }
            }
            return FAILED;
        }

        public async Task<object?> ViewProfile(string userId)
        {
            var user = await (from u in _context.UserAccounts
                              where u.UserId == userId
                              select new
                              {
                                  u.NickName,
                                  u.Gender,
                                  u.Phone,
                                  u.Email,
                                  u.AvatarImageUrl,
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                return user;
            }
            return null;
        }

        private bool UserExists(string id)
        {
            return _context.UserAccounts.Any(e => e.UserId == id);
        }


    }
}
