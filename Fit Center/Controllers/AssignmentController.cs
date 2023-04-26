using Fit_Center.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using RestSharp;

namespace Fit_Center.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly RestClient classClient = new RestClient("https://localhost:7275/api/Classes/");
        private readonly RestClient assignmentClient = new RestClient("https://localhost:7275/api/Assignments/");

        // GET: AssignmentController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AssignmentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AssignmentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AssignmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id)
        {
            var ClassRequest = new RestRequest("GetId", Method.Get);
            ClassRequest.AddParameter("id", id);

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

        // GET: AssignmentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AssignmentController/Edit/5
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

        // GET: AssignmentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AssignmentController/Delete/5
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
