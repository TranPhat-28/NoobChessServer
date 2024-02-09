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

namespace NoobChessServer.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;

        public AuthRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<ServiceResponse<GetUserDto>> LoginWithFacebook(FacebookLoginDto facebookLoginDto)
        {
            var response = new ServiceResponse<GetUserDto>();

            // Call Google API to verify the token
            using (var client = new HttpClient())
            {
                // Make a request to the Facebook Graph API for information
                var uri = new Uri($"https://graph.facebook.com/me?access_token={facebookLoginDto.FacebookAccessToken}&&fields=id,name,email,picture.width(640)");

                // Read the response from Facebook
                var result = await client.GetAsync(uri);
                var jsonMessage = await result.Content.ReadAsStringAsync();

                // Now try parsing it to get the information
                try
                {
                    var userInfo = JsonConvert.DeserializeObject<FacebookUser>(jsonMessage);

                    if (!string.IsNullOrEmpty(userInfo!.Id) && !string.IsNullOrEmpty(userInfo!.Email))
                    {
                        response.Data = new GetUserDto()
                        {
                            Id = "1",
                            Name = userInfo.Name,
                            Picture = userInfo.Picture.Data.Url,
                            Email = userInfo.Email,
                            DateJoined = DateTime.Today,
                            JWTToken = CreateJWTToken(userInfo.Id, userInfo.Email)
                        };
                        response.Message = "Facebook login successfully";
                    }
                    // If login not success
                    else
                    {
                        response.Message = "Facebook login failed";
                        response.IsSuccess = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    response.Message = "Server error";
                    response.IsSuccess = false;
                }
            }

            return response;
        }

        public async Task<ServiceResponse<GetUserDto>> LoginWithGoogle(GoogleLoginDto googleLoginDto)
        {
            var response = new ServiceResponse<GetUserDto>();

            // Call Google API to verify the token
            using (var client = new HttpClient())
            {
                var uri = new Uri("https://www.googleapis.com/oauth2/v3/userinfo");

                // Add the Google token to the request headers
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", googleLoginDto.GoogleAccessToken);

                // Read the response from Google
                var result = await client.GetAsync(uri);
                var jsonMessage = await result.Content.ReadAsStringAsync();

                // Now try parsing it to get the information
                try
                {
                    var userInfo = JsonConvert.DeserializeObject<GoogleUser>(jsonMessage);

                    if (!string.IsNullOrEmpty(userInfo!.Sub) && !string.IsNullOrEmpty(userInfo!.Email))
                    {
                        response.Data = new GetUserDto()
                        {
                            Id = "1",
                            Name = userInfo.Name,
                            Picture = userInfo.Picture.Replace("s96-c", "s192-c"),
                            Email = userInfo.Email,
                            DateJoined = DateTime.Today,
                            JWTToken = CreateJWTToken(userInfo.Sub, userInfo.Email)
                        };
                        response.Message = "Google login successfully";
                    }
                    // If login not success
                    else
                    {
                        response.Message = "Google login failed";
                        response.IsSuccess = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    response.Message = "Server error";
                    response.IsSuccess = false;
                }
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
    }
}