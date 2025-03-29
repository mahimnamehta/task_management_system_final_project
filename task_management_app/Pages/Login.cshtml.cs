using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson.IO;
using task_management_app.Models;

namespace task_management_app.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public LoginModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var requestBody = new LoginRequest(Email, Password);
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://sea-lion-app-772a9.ondigitalocean.app/v1/users/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var loginReponse = JsonConvert.DeserializeObject<LoginResponse>(responseString);
                    HttpContext.Session.SetString("auth_token", loginReponse.token);
                    return RedirectToPage("/Index");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var loginReponse = JsonConvert.DeserializeObject<LoginResponse>(errorContent);
                    ErrorMessage = $"Error on Login: {loginReponse?.error}";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error on call the API: {ex.Message}";
                return Page();
            }
        }
    }
}
}
