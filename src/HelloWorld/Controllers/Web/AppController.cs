using HelloWorld.Models;
using HelloWorld.Services;
using HelloWorld.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using static System.String;

namespace HelloWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _servies;
        private IWorldRepository _repository;

        public AppController(IMailService service, IWorldRepository repository)
        {
            _servies = service;
            _repository = repository;
        }

		public IActionResult Index() => View();

		[Authorize]
		public IActionResult Trips() => View();
        

        public IActionResult About() => View();

        public IActionResult Contact() => View();

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (IsNullOrWhiteSpace(model.Email)) ModelState.AddModelError("", "Can not send Email ,config Problem");

            if (ModelState.IsValid)
            {
                var siteEmailAddress = Startup.configuration["AppSettings:SiteEmailAddress"];
                if (_servies.SendEmail(siteEmailAddress, siteEmailAddress, $"email to{ model.Email},Message:{model.Message}", ""))
                {
                    //Clear the model as well;
                    ModelState.Clear();
                    ViewBag.Message = "Email Sent ,Thanks";
                }

            }
            return View();
        }



    }
}

