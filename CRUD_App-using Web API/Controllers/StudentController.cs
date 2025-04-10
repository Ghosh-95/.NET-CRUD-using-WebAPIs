using CRUD_App_using_Web_API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace CRUD_App_using_Web_API.Controllers
{
    public class StudentController : Controller
    {
        private string _APIBaseUrl = "https://localhost:7030/api/StudentAPI";
        private HttpClient _httpClient = new HttpClient();

        [HttpGet]
        public IActionResult Index()
        {
            List<Student> students = new List<Student>();
            HttpResponseMessage httpResponseMessage = _httpClient.GetAsync($"{_APIBaseUrl}/students").Result;
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Student>>(result);

                if (data != null)
                {
                    students = data;
                }
            }

            return View(students);
        }

        //CREATE view
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // CREATE data
        [HttpPost]
        public IActionResult CreateNewStudent(Student newStudent)
        {
            string data = JsonConvert.SerializeObject(newStudent);

            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMsgForCreation = _httpClient.PostAsync(_APIBaseUrl, content).Result;

            if (httpResponseMsgForCreation.IsSuccessStatusCode)
            {
                TempData["creation_success_msg"] = $"A new student {newStudent.FullName} has been successfully added.";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
