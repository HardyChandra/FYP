using FYP.Models;
using FYP.Models.VotingModel;
using FYP.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVotingRepository _voteRepo;

        public HomeController(ILogger<HomeController> logger, IVotingRepository voteRepo)
        {
            _logger = logger;
            _voteRepo = voteRepo;
        }

        public IActionResult Index(string message)
        {
            if (message != null)
			{
                ViewBag.Message = message;
			}
            return View();
        }

        [HttpPost("Index")]
        [ValidateAntiForgeryToken]
        public IActionResult Search(VotingSearch search)
		{
			if (ModelState.IsValid)
			{
                var check = _voteRepo.GetVotingByID(search.VotingID);
                if(check != null)
				{
                    return RedirectToAction("VotingPage", "Voting", new {VoteID = search.VotingID});
				}
                ViewBag.Message = "Vote's ID is Invalid. Enter the correct ID.";
                return RedirectToAction("Index", "Home" , new {message = ViewBag.Message});
			}
            return View();
		}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
