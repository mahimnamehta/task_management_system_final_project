using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Text;
using task_management_app.Models;
using static task_management_app.Models.AllUsersResponse;
using static task_management_app.Models.TasksResponse;

namespace task_management_app.Pages
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(HttpClient HttpClient)
        {
            _httpClient = HttpClient;
        }
        [BindProperty]
        public string AssignedToUid { get; set; }

        [BindProperty]
        public string Description { get; set; }
        public string ErrorMessage { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public async Task<IActionResult> OnGet()
        {
            var token = GetToken();
            if (token == null)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-access-token", token);

                var response = await _httpClient.GetAsync("https://sea-lion-app-772a9.ondigitalocean.app/v1/users/all");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var usersReponse = JsonConvert.DeserializeObject<AllUsersResponse>(responseString);
                    Users = usersReponse.allUsers;
                }
            }
            catch (Exception ex)
            {

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
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-access-token", token);

                var taskData = new CreateTaskRequest();
                taskData.description = Description;
                taskData.assignedToUid = AssignedToUid;

                var json = JsonConvert.SerializeObject(taskData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://sea-lion-app-772a9.ondigitalocean.app/v1/tasks/", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    ErrorMessage = "Error on creating Task!";
                }
            } catch (Exception ex)
            {
                ErrorMessage = "Error on creating Task!";
            }
            return Page();
        }

        private string GetToken()
        {
            return HttpContext.Session.GetString("auth_token");
        }
    }
}
