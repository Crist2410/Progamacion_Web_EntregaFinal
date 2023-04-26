using Fit_Center.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using RestSharp;
using System.Globalization;

namespace Fit_Center.Controllers
{
    public class ClassController : Controller
    {

        private readonly RestClient classClient = new RestClient("https://localhost:7275/api/Classes/");
        private readonly RestClient assignmentClient = new RestClient("https://localhost:7275/api/Assignments/");

        // GET: ClassController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ShowClases
        public async Task<IActionResult> ListClass()
        {
            var ClassRequest = new RestRequest("GetList", Method.Get);
            var ClassResponse = classClient.Execute(ClassRequest);
            var response = await classClient.GetAsync(ClassRequest);

            var classList = JsonConvert.DeserializeObject<List<Class>>(response.Content);

            var AssignmentsRequest = new RestRequest("GetListByUser", Method.Get);
            AssignmentsRequest.AddParameter("id", UserSingleton.Instance.User.UserId);

            var AssignmentsResponse = assignmentClient.Execute(AssignmentsRequest);
            UserSingleton.Instance.User.Assignments = JsonConvert.DeserializeObject<List<Assignment>>(AssignmentsResponse.Content);

            var upcomingClasses = classList.Where(c => c.StartDate > DateTime.Now 
                                                    && !UserSingleton.Instance.User.Assignments.Any(a => a.ClassId == c.ClassId)).ToList();

            return View(upcomingClasses);
        }




        // GET: ClassController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClassController/Create
        public ActionResult Create()
        {
            return View("CreateClass");
        }

        // POST: ClassController/Create
        [HttpPost]
        public IActionResult Create(IFormCollection formCollection)
        {
            var ClassRequest = new RestRequest("Insert", Method.Post);
            var startTime = TimeSpan.Parse(formCollection["StartTime"]);
            var endTime = TimeSpan.Parse(formCollection["EndTime"]);

            var clase = new Class
            {
                ClassName = formCollection["ClassName"],
                ClassDescription = formCollection["ClassDescription"],
                StartDate = DateTime.Parse(formCollection["StartDate"]),
                EndDate = DateTime.Parse(formCollection["EndDate"]),
                StartTime = new TimeSpan(startTime.Hours, startTime.Minutes, 0),
                EndTime = new TimeSpan(endTime.Hours, endTime.Minutes, 0),
                Location = formCollection["Location"]
            };

            ClassRequest.AddJsonBody(clase);
            var ClassResponse = classClient.Execute(ClassRequest);
            var Class = JsonConvert.DeserializeObject<Class>(ClassResponse.Content);

            var asignacion = new Assignment
            {
                ClassId = Class.ClassId,
                UserId = UserSingleton.Instance.User.UserId,
                AssignmentDate = DateTime.Now.Date,
                Status = "CONFIRMED"
            };
            var AssignmentRequest = new RestRequest("Insert", Method.Post);
            AssignmentRequest.AddJsonBody(asignacion);
            var AssignmentResponse = assignmentClient.Execute(AssignmentRequest);

            return RedirectToAction("IndexUser", "User");
        }



        

        // GET: ClassController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClassController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClassController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClassController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
