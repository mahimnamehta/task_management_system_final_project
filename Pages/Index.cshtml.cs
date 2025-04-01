using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using TaskManagerApp.Models;
using static TaskManagerApp.Models.TasksResponse;

namespace TaskManagerApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public bool isShowingCreatedBy = true;
        public string ErrorMessage { get; set; }

        public IndexModel(HttpClient HttpClient)
        {
            _httpClient = HttpClient;
        }

        public async Task<IActionResult> OnGet(string ErrorMessage, bool IsAssignedToTab)
        {
            this.ErrorMessage = ErrorMessage;
            return await (IsAssignedToTab ? OnGetAssignedToMe() : OnGetCreatedByMe());
        }

        public async Task<IActionResult> OnGetCreatedByMe()
        {
            var token = GetToken();
            if (token == null)
            {
                return RedirectToPage("/Login");
            }

            Tasks = await FetchTasksAsync("https://sea-lion-app-772a9.ondigitalocean.app/v1/tasks/createdby/", token);
            isShowingCreatedBy = true;

            return Page();
        }

        public async Task<IActionResult> OnGetAssignedToMe()
        {
            var token = GetToken();
            if (token == null)
            {
                return RedirectToPage("/Login");
            }

            Tasks = await FetchTasksAsync("https://sea-lion-app-772a9.ondigitalocean.app/v1/tasks/assignedto/", token);
            isShowingCreatedBy = false;

            return Page();
        }

        public async Task<List<TaskItem>> FetchTasksAsync(string url, string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-access-token", token);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var tasksReponse = JsonConvert.DeserializeObject<TasksResponse>(responseString);
                    return tasksReponse.AllTasks;
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var tasksReponse = JsonConvert.DeserializeObject<TasksResponse>(responseString);
                    return new List<TaskItem>();
                }
            }
            catch (Exception ex)
            {
                return new List<TaskItem>();
            }
        }

        public async Task<IActionResult> OnPostDeleteTask(string taskUid)
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

                var response = await _httpClient.DeleteAsync($"https://sea-lion-app-772a9.ondigitalocean.app/v1/tasks/{taskUid}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage();
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var taskDeleteResponse = JsonConvert.DeserializeObject<DeleteTaskResponse>(responseString);
                    ErrorMessage = taskDeleteResponse.error;
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = "Error on deleting Task!";
            }

            return RedirectToPage(new { ErrorMessage });
        }

        public async Task<IActionResult> OnPostMarkAsDone(string taskUid)
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

                var requestBody = new UpdateTaskRequest();
                requestBody.Done = true;

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                var response = await _httpClient.PatchAsync($"https://sea-lion-app-772a9.ondigitalocean.app/v1/tasks/{taskUid}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage(new { IsAssignedToTab = true });
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var taskUpdateResponse = JsonConvert.DeserializeObject<UpdateTaskResponse>(responseString);
                    ErrorMessage = taskUpdateResponse.error;
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = "Error on deleting Task!";
            }

            return RedirectToPage(new { ErrorMessage });
        }

        public async Task<IActionResult> OnPostLogout()
        {
            HttpContext.Session.Remove("auth_token");
            return RedirectToPage("/Login");
        }

        private string GetToken()
        {
            return HttpContext.Session.GetString("auth_token");
        }
    }
}
