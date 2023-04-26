using Fit_Center.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using RestSharp;

namespace Fit_Center.Controllers
{
    public class UserController : Controller
    {

        private readonly RestClient Client = new RestClient("https://localhost:7275/api/Users/");
        private readonly RestClient ClassClient = new RestClient("https://localhost:7275/api/Classes/");
        private readonly RestClient AssignmentsClient = new RestClient("https://localhost:7275/api/Assignments/");

        // GET: UserController
        public async Task<IActionResult> AllUsers()
        {
            var UserRequest = new RestRequest("GetList", Method.Get);
            var UserResponse = Client.Execute(UserRequest);

            var userList = JsonConvert.DeserializeObject<List<User>>(UserResponse.Content);

            return View(userList);
        }

        // GET: LogIn
        public ActionResult LogIn()
        {
            return View();
        }

        // GET: LogIn
        [HttpPost]
        public IActionResult LogIn(IFormCollection formCollection)
        {
            var UserRequest = new RestRequest("GetLogin", Method.Get);
            UserRequest.AddParameter("email", formCollection["Email"]);
            UserRequest.AddParameter("password", formCollection["Password"]);

            var UserResponse = Client.Execute(UserRequest);

            if(UserResponse.Content == "")
                return View();
            else
            {
                var user = JsonConvert.DeserializeObject<User>(UserResponse.Content);
                UserSingleton.Instance.User = user;


                var AssignmentsRequest = new RestRequest("GetListByUser", Method.Get);
                AssignmentsRequest.AddParameter("id", UserSingleton.Instance.User.UserId);

                var AssignmentsResponse = AssignmentsClient.Execute(AssignmentsRequest);
                UserSingleton.Instance.User.Assignments = JsonConvert.DeserializeObject<List<Assignment>>(AssignmentsResponse.Content);

                var classes = new List<Class>();
                foreach (var assignment in UserSingleton.Instance.User.Assignments)
                {
                    var ClassRequest = new RestRequest("GetId", Method.Get);
                    ClassRequest.AddParameter("id", assignment.ClassId);
                    var ClassResponse = ClassClient.Execute(ClassRequest);
                    assignment.Class = JsonConvert.DeserializeObject<Class>(ClassResponse.Content);
                    classes.Add(assignment.Class);
                }
                ViewBag.UserClasses = classes;
                return View("IndexUser", UserSingleton.Instance.User);
            }
        }

        // GET: SignIn
        public ActionResult IndexUser()
        {
            if (UserSingleton.Instance.User == null)
                return View("LogIn");
            else
            {
                var AssignmentsRequest = new RestRequest("GetListByUser", Method.Get);
                AssignmentsRequest.AddParameter("id", UserSingleton.Instance.User.UserId);

                var AssignmentsResponse = AssignmentsClient.Execute(AssignmentsRequest);
                UserSingleton.Instance.User.Assignments = JsonConvert.DeserializeObject<List<Assignment>>(AssignmentsResponse.Content);

                var classes = new List<Class>();
                foreach (var assignment in UserSingleton.Instance.User.Assignments)
                {
                    var ClassRequest = new RestRequest("GetId", Method.Get);
                    ClassRequest.AddParameter("id", assignment.ClassId);
                    var ClassResponse = ClassClient.Execute(ClassRequest);
                    assignment.Class = JsonConvert.DeserializeObject<Class>(ClassResponse.Content);
                    classes.Add(assignment.Class);
                }
                ViewBag.UserClasses = classes;
                return View(UserSingleton.Instance.User);
            }
        }

        // GET: SignIn
        public ActionResult LogOut()
        {
            UserSingleton.Instance.User = null;
            return View("LogIn");
        }
        // GET: SignIn
        public ActionResult SignIn()
        {
            return View();
        }


        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClienteController/Create
        [HttpPost]
        public IActionResult Create(IFormCollection formCollection)
        {

            var UserRequest = new RestRequest("Insert", Method.Post);

            var Usuario = new User
            {
                Email = formCollection["Email"],
                Password = formCollection["Password"],
                FirstName = formCollection["FirstName"],
                LastName = formCollection["LastName"],
                BirthDate = DateTime.Parse(formCollection["BirthDate"]),
                Role = formCollection["Role"]
            };

            UserRequest.AddJsonBody(Usuario);
            UserSingleton.Instance.User = Usuario;
            return View("IndexUser", Usuario);
        }


        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, User Usuario)
        {
            var db = new fit_centerContext();

            try
            {

                User UsuarioOriginal = db.Users.Find(db.Users.Where(u => u.UserId == id));
                Usuario.Role = UsuarioOriginal.Role;
                db.Users.Update(Usuario);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
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
