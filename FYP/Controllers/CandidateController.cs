using FYP.DatabaseModel;
using FYP.Models.CandidateModel;
using FYP.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace FYP.Controllers
{
    public class CandidateController : Controller
    {

        private readonly ICandidateRepository _candidateRepo;
        private readonly IVotingRepository _voteRepo;

        public CandidateController(ICandidateRepository candidateRepo, IVotingRepository voteRepo)
        {
            _candidateRepo = candidateRepo;
            _voteRepo = voteRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult AddCandidate()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCandidate(InsertCandidate candidate)
        {
            if (ModelState.IsValid)
            {
                string adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (adminID == null)
                {
                    return RedirectToAction("Login", "Admin");
                }
                var vote = _voteRepo.GetVoting(adminID);
                if (vote != null)
                {
                    candidate.VotingID = vote.voting_id;
                    candidate.CandidateID = Guid.NewGuid().ToString();
                    _candidateRepo.AddCandidate(candidate.CandidateID, candidate.VotingID, candidate.CandidateName, candidate.CandidateVision, candidate.CandidateMission);
                    return RedirectToAction("Dashboard", "Voting");
                }
                ViewBag.Message = "There is no any created voting.";
                return RedirectToAction("CreateVoting", "Voting");
            }
            return View();
        }

        [Authorize]
        public IActionResult Edit(string candidateID)
        {
            var candidate = _candidateRepo.GetCandidateID(candidateID);
            if (candidate != null)
            {
                return View(candidate);
            }
            return RedirectToAction("AddCandidate", "Candidate");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditCandidate edit, Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                edit.candidate_id = candidate.candidate_id;
                _candidateRepo.EditCandidate(edit.candidate_id, edit.candidate_name, edit.candidate_vision, edit.candidate_mission);
                return RedirectToAction("Dashboard", "Voting");
            }
            return View();
        }

        [Authorize]
        public IActionResult Delete(string candidateID)
        {
            var candidate = _candidateRepo.GetCandidateID(candidateID);
            if (candidate != null)
            {
                return View(candidate);
            }
            return RedirectToAction("AddCandidate", "Candidate");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                _candidateRepo.RemoveCandidate(candidate.candidate_id);
                return RedirectToAction("Dashboard", "Voting");
            }
            return RedirectToAction("AddCandidate", "Candidate");
        }
    }
}
