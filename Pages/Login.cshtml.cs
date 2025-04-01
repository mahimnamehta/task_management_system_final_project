using System.Text;
using NewtonsoftJson = Newtonsoft.Json.JsonConvert;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using TaskManagerApp.Models;

namespace TaskManagerApp.Pages
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
            // OnGet logic (if necessary)
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                // Construct the login request body
                var requestBody = new LoginRequest(Email, Password);
                var json = NewtonsoftJson.SerializeObject(requestBody); // Using alias for clarity
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Call the login API
                var response = await _httpClient.PostAsync("https://sea-lion-app-772a9.ondigitalocean.app/v1/users/login", content);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response string
                    var responseString = await response.Content.ReadAsStringAsync();
                    var loginResponse = NewtonsoftJson.DeserializeObject<LoginResponse>(responseString);

                    // Store the token in the session
                    HttpContext.Session.SetString("auth_token", loginResponse.token);

                    // Redirect to the home page or another page after successful login
                    return RedirectToPage("/Index");
                }
                else
                {
                    // Handle failed login
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var loginResponse = NewtonsoftJson.DeserializeObject<LoginResponse>(errorContent);

                    // Show the error message
                    ErrorMessage = $"Error on Login: {loginResponse?.error}";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                // Catch any other exceptions
                ErrorMessage = $"Error calling the API: {ex.Message}";
                return Page();
            }
        }
    }
}
