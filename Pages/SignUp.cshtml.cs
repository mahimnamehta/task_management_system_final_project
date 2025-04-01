using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagerApp.Models;

namespace TaskManagerApp.Pages
{
    public class SignUpModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public SignUpModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public string Name { get; set; }

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
                var requestBody = new SignUpRequest(Name, Email, Password);
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://sea-lion-app-772a9.ondigitalocean.app/v1/users/signup", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var signUpReponse = JsonConvert.DeserializeObject<SignUpResponse>(responseString);
                    return RedirectToPage("/Login");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var signUpReponse = JsonConvert.DeserializeObject<SignUpResponse>(errorContent);
                    ErrorMessage = $"Error on SignUp: {signUpReponse?.error}";
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

