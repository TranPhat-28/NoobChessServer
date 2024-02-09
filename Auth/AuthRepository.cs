using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NoobChessServer.DTOs.LoginDtos;
using System.IdentityModel.Tokens.Jwt;
using NoobChessServer.DTOs.UserDtos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace NoobChessServer.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public AuthRepository(IConfiguration configuration, DataContext dataContext, IMapper mapper)
        {
            _configuration = configuration;
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetUserDto>> LoginWithFacebook(FacebookLoginDto facebookLoginDto)
        {
            var response = new ServiceResponse<GetUserDto>();

            try
            {
                // Call Facebook API to verify the token
                using (var client = new HttpClient())
                {
                    // Make a request to the Facebook Graph API for information
                    var uri = new Uri($"https://graph.facebook.com/me?access_token={facebookLoginDto.FacebookAccessToken}&&fields=id,name,email,picture.width(640)");

                    // Read the response from Facebook
                    var result = await client.GetAsync(uri);
                    var jsonMessage = await result.Content.ReadAsStringAsync();

                    // Now parse it to get the information
                    var facebookUserInfo = JsonConvert.DeserializeObject<FacebookUser>(jsonMessage);

                    // Check in the DB for email
                    bool userExists = await UserExists(facebookUserInfo!.Email);

                    if (!userExists)
                    // Not exists, write to DB first then load
                    {
                        // Facebook valid credential
                        if (!string.IsNullOrEmpty(facebookUserInfo!.Id) && !string.IsNullOrEmpty(facebookUserInfo!.Email))
                        {
                            User newUser = new User()
                            {
                                Name = facebookUserInfo.Name,
                                Picture = facebookUserInfo.Picture.Data.Url,
                                Email = facebookUserInfo.Email
                            };

                            _dataContext.Users.Add(newUser);
                            await _dataContext.SaveChangesAsync();
                        }
                        // If login not success
                        else
                        {
                            response.Message = "Facebook login failed";
                            response.IsSuccess = false;
                        }
                    }

                    // Now load the users from DB and return
                    var user = await _dataContext.Users.FirstAsync(u => u.Email == facebookUserInfo.Email);

                    // The response
                    response.Message = "Facebook login success";
                    response.Data = _mapper.Map<GetUserDto>(user);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.Message = "Server error";
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<ServiceResponse<GetUserDto>> LoginWithGoogle(GoogleLoginDto googleLoginDto)
        {
            var response = new ServiceResponse<GetUserDto>();

            try
            {
                // Call Google API to verify the token
                using (var client = new HttpClient())
                {
                    var uri = new Uri("https://www.googleapis.com/oauth2/v3/userinfo");

                    // Add the Google token to the request headers
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", googleLoginDto.GoogleAccessToken);

                    // Read the response from Google
                    var result = await client.GetAsync(uri);
                    var jsonMessage = await result.Content.ReadAsStringAsync();

                    // Now parse it to get the information
                    var googleUserInfo = JsonConvert.DeserializeObject<GoogleUser>(jsonMessage);

                    // Check in the DB for email
                    bool userExists = await UserExists(googleUserInfo!.Email);

                    if (!userExists)
                    // Not exists, write to DB first then load
                    {
                        // Google valid credential
                        if (!string.IsNullOrEmpty(googleUserInfo!.Sub) && !string.IsNullOrEmpty(googleUserInfo!.Email))
                        {
                            var newUser = _mapper.Map<User>(googleUserInfo);
                            // Get high quality profile picture
                            newUser.Picture = newUser.Picture.Replace("s96-c", "s192-c");

                            _dataContext.Users.Add(newUser);
                            await _dataContext.SaveChangesAsync();
                        }
                        // If login not success
                        else
                        {
                            response.Message = "Google login failed";
                            response.IsSuccess = false;
                        }
                    }

                    // Now load the users from DB and return
                    var user = await _dataContext.Users.FirstAsync(u => u.Email == googleUserInfo.Email);

                    // The response
                    response.Message = "Google login success";
                    response.Data = _mapper.Map<GetUserDto>(user);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.Message = "Server error";
                response.IsSuccess = false;
            }

            return response;
        }

        private string CreateJWTToken(string userId, string userEmail)
        {
            // The list of Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userEmail)
            };

            // Getting the app secret from appsetting.json
            var appSettingToken = _configuration.GetSection("AppSettings:Token").Value;

            // If AppSetting token is null
            if (appSettingToken is null)
            {
                throw new Exception("Appsettings token not found");
            }

            // Symmetric key for the token with secret is the AppSettings token
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingToken));

            // Signing credential
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Storing some information such as Claims and Expiring day for the final token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signingCredentials
            };

            // JWT handler
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            // Use the handler to create the token with the tokenDescriptor
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            // Write the token
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _dataContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}