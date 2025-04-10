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


        // UPDATE view
        [HttpGet]
        public IActionResult UpdateStudent(int Id)
        {
            Student student = new Student();
            HttpResponseMessage httpResponseMsgUpdate = _httpClient.GetAsync($"{_APIBaseUrl}/{Id}").Result;

            if (httpResponseMsgUpdate.IsSuccessStatusCode)
            {
                string result = httpResponseMsgUpdate.Content.ReadAsStringAsync().Result;
                var selectedStudent = JsonConvert.DeserializeObject<Student>(result);

                if (selectedStudent != null)
                {
                    student = selectedStudent;
                }
            }

            //return RedirectToAction("Index");
            return View(student);
        }

        // UPDATE data
        [HttpPost]
        public IActionResult UpdateStudent(Student selectedStudent)
        {
            string data = JsonConvert.SerializeObject(selectedStudent);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage updatedResponse = _httpClient.PutAsync($"{_APIBaseUrl}/{selectedStudent.Id}", content).Result;

            if (updatedResponse.IsSuccessStatusCode)
            {
                TempData["Sucessfully_updated_msg"] = $"The student (ID:{selectedStudent.Id}) has been successfully updated.";
                return RedirectToAction("Index");
            }

            return View();
        }

        // Details view
        [HttpGet]
        public IActionResult Details(int Id)
        {
            Student student = new Student();
            HttpResponseMessage httpResponseMsgUpdate = _httpClient.GetAsync($"{_APIBaseUrl}/{Id}").Result;

            if (httpResponseMsgUpdate.IsSuccessStatusCode)
            {
                string result = httpResponseMsgUpdate.Content.ReadAsStringAsync().Result;
                var selectedStudent = JsonConvert.DeserializeObject<Student>(result);

                if (selectedStudent != null)
                {
                    student = selectedStudent;
                }
            }

            return View(student);
        }

        // Delete view
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            Student student = new Student();
            HttpResponseMessage httpResponse = _httpClient.GetAsync($"{_APIBaseUrl}/{Id}").Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                string result = httpResponse.Content.ReadAsStringAsync().Result;
                var selectedStudent = JsonConvert.DeserializeObject<Student>(result);
                if (selectedStudent != null)
                {
                    student = selectedStudent;
                }
            }

            return View(student);
        }


        // Renamed the second Delete method to DeleteConfirmed to resolve the CS0111 error
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int Id)
        {
            HttpResponseMessage deleteResponse = _httpClient.DeleteAsync(_APIBaseUrl + "/" + Id).Result;
            if (deleteResponse.IsSuccessStatusCode)
            {
                TempData["Delete_success_msg"] = $"The student (ID:{Id}) has been successfully deleted.";
                return RedirectToAction("Index");
            }

            return View();
        }

    }
}
