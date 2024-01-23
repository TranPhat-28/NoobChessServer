using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using NoobChessServer.DTOs.LoginDtos;

namespace NoobChessServer.Auth
{
    public class AuthRepository : IAuthRepository
    {
        public async Task<ServiceResponse<string>> LoginWithFacebook(FacebookLoginDto facebookLoginDto)
        {
            var response = new ServiceResponse<string>
            {
                Message = "Login with Facebook"
            };

            return response;
        }

        public async Task<ServiceResponse<string>> LoginWithGoogle(GoogleLoginDto googleLoginDto)
        {
            var response = new ServiceResponse<string>();

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

                    // If login not success
                    if (!string.IsNullOrEmpty(userInfo!.Sub) && !string.IsNullOrEmpty(userInfo!.Email))
                    {
                        response.Message = userInfo!.Email;
                    }
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
    }
}