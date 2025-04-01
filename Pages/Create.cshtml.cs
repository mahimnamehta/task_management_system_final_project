using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json; 
using System;
using System.Collections.Generic;
using System.Linq;  
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Models;

namespace TaskManagerApp.Pages
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public string AssignedToUid { get; set; }

        [BindProperty]
        public string Description { get; set; }

        public string ErrorMessage { get; set; }
        public List<TaskManagerApp.Models.User> Users { get; set; } = new List<TaskManagerApp.Models.User>();  // Corrected type to TaskManagerApp.Models.User

        public async Task<IActionResult> OnGet()
        {
            var token = GetToken();
            if (token == null)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                // Set Authorization Header with the token
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-access-token", token);

                // Call the API to get all users
                var response = await _httpClient.GetAsync("https://sea-lion-app-772a9.ondigitalocean.app/v1/users/all");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var usersResponse = JsonConvert.DeserializeObject<AllUsersResponse>(responseString);

                    // Map the AllUsersResponse.User to TaskManagerApp.Models.User
                    Users = usersResponse.allUsers
                        .Select(user => new User
                        {
                            Id = user.uid,
                            Username = user.name,
                            Email = user.email
                        })
                        .ToList();
                }
                else
                {
                    ErrorMessage = "Unable to fetch users!";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error fetching users: " + ex.Message;
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var token = GetToken();
            if (token == null)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                // Set Authorization Header with the token
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-access-token", token);

                // Prepare the task data to be sent to the API
                var taskData = new CreateTaskRequest
                {
                    Description = Description,
                    AssignedToUid = AssignedToUid
                };

                var json = JsonConvert.SerializeObject(taskData); // Use Newtonsoft.Json.JsonConvert
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // POST the task to the API
                var response = await _httpClient.PostAsync("https://sea-lion-app-772a9.ondigitalocean.app/v1/tasks/", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    ErrorMessage = "Error creating task! Response: " + responseString;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error creating task: " + ex.Message;
            }

            return Page();
        }

        private string GetToken()
        {
            return HttpContext.Session.GetString("auth_token");
        }
    }
}
