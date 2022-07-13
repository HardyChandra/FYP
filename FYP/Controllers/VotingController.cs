using FYP.DatabaseModel;
using FYP.Models.AdminModel;
using FYP.Models.CandidateModel;
using FYP.Models.VotingModel;
using FYP.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using FYP.BlockchainModel;
using Newtonsoft.Json;

namespace FYP.Controllers
{
    public class VotingController : Controller
    {

        private readonly IVotingRepository _voteRepo;
        private readonly ICandidateRepository _candidateRepo;

        public VotingController(IVotingRepository votingRepo, ICandidateRepository candidateRepo)
        {
            _voteRepo = votingRepo;
            _candidateRepo = candidateRepo;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (adminID == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                var voting = _voteRepo.GetVoting(adminID);

                if (voting != null)
                {
                    if (voting.voting_status == Status.Pending || voting.voting_status == Status.Ongoing)
                    {
                        HomepageViewModel viewModel = new HomepageViewModel();
                        //Voting
                        viewModel.viewVoting = voting;

                        var candidate = _candidateRepo.GetCandidate(voting.voting_id);
                        //Check total candidate; if >= 1, then add to the ViewCandidate List
                        if (candidate.Count >= 1)
                        {
                            List<ViewCandidate> view = new List<ViewCandidate>();
                            foreach (var row in candidate)
                            {
                                view.Add(new ViewCandidate
                                {
                                    CandidateID = row.candidate_id,
                                    VotingID = row.voting_id,
                                    CandidateName = row.candidate_name,
                                    CandidateVision = row.candidate_vision,
                                    CandidateMission = row.candidate_mission,
                                });
                            }
                            viewModel.viewCandidate = view;
                            if (voting.voting_status == Status.Ongoing)
                            {
                                if (voting.end_date < DateTime.Today)
                                {
                                    _voteRepo.EndVote(voting.voting_id, Status.Finish);
                                    return RedirectToAction("Dashboard", "Voting");
                                }
                            }
                            return View(viewModel);
                        }
                        else
                        {
                            return RedirectToAction("AddCandidate", "Candidate");
                        }

                    }

                    ViewBag.Message = "Currently, there is no ongoing voting";
                    return View();
                }

                ViewBag.Message = "Currently, there is no ongoing voting";
                return View();
            }
        }

        [Authorize]
        [HttpPost("Dashboard")]
        [ValidateAntiForgeryToken]
        //Export the Credential from DB to CSV File
        public IActionResult GetVoterCredential()
        {
            var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var voting = _voteRepo.GetVoting(adminID);
            if (voting != null)
            {
                var getCredential = _voteRepo.GetCredential(voting.voting_id);
                if (getCredential.Count >= 1)
                {
                    List<object> credentials = new List<object>();
                    foreach (var row in getCredential)
                    {
                        credentials.Add(new[]
                        {
                            row.voter_credential_id,
                        });
                    }

                    credentials.Insert(0, new string[1] { "Voter Credential" });
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in credentials)
                    {

                        string[] arrCredential = (string[])item;
                        foreach (var data in arrCredential)
                        {
                            //Append data with comma(,) separator.
                            sb.Append(data + ',');
                        }
                        //Append new line character.
                        sb.Append("\r\n");
                    }
                    return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Credential.csv");
                }
                return RedirectToAction("Dashboard", "Voting");
            }
            return RedirectToAction("Dashboard", "Voting");
        }

        [Authorize]
        public IActionResult StartVoting()
        {
            var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (adminID == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                var voting = _voteRepo.GetVoting(adminID);
                if (voting != null && voting.voting_status == Status.Pending || voting.voting_status == Status.Ongoing)
                {
                    HomepageViewModel viewModel = new HomepageViewModel();
                    //Voting
                    viewModel.viewVoting = voting;

                    var candidate = _candidateRepo.GetCandidate(voting.voting_id);
                    //Check total candidate; if >= 1, then add to the ViewCandidate List
                    if (candidate.Count >= 1)
                    {
                        List<ViewCandidate> view = new List<ViewCandidate>();
                        foreach (var row in candidate)
                        {
                            view.Add(new ViewCandidate
                            {
                                CandidateID = row.candidate_id,
                                VotingID = row.voting_id,
                                CandidateName = row.candidate_name,
                                CandidateVision = row.candidate_vision,
                                CandidateMission = row.candidate_mission,
                            });
                        }
                        viewModel.viewCandidate = view;
                        return View(viewModel);
                    }
                }
            }
            return View();
        }

        [Authorize]
        [HttpPost("StartVoting")]
        [ValidateAntiForgeryToken]
        public IActionResult Start()
        {
            if (ModelState.IsValid)
            {
                var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var voting = _voteRepo.GetVoting(adminID);
                if (voting != null)
                {
                    var startDate = DateTime.Now;
                    var endDate = startDate.AddDays(voting.voting_duration);
                    _voteRepo.StartVote(voting.voting_id, startDate, endDate, Status.Ongoing);
                    return RedirectToAction("Dashboard", "Voting");
                }
                return View();
            }
            return View();
        }

        [Authorize]
        public IActionResult CreateVoting()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateVoting(Create voting)
        {
            if (ModelState.IsValid)
            {
                var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                voting.VotingID = Guid.NewGuid().ToString();
                voting.AdminID = adminID;
                _voteRepo.CreateVoting(voting.VotingID, voting.AdminID, voting.Title, voting.Description, voting.TotalVoter, voting.Duration, DatabaseModel.Status.Pending);

                //add voter credential
                for (int i = 0; i <= voting.TotalVoter; i++)
                {
                    VoterCredential voter = new VoterCredential();
                    voter.voter_credential_id = Guid.NewGuid().ToString();
                    voter.voting_id = voting.VotingID;
                    _voteRepo.CreateVoterCredential(voter.voter_credential_id, voter.voting_id);
                }
                return RedirectToAction("Dashboard", "Voting");
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult VotingPage(string VoteID)
        {
            if (VoteID != null)
            {
                var voting = _voteRepo.GetVotingByID(VoteID);
                if (voting != null && voting.voting_status == Status.Pending || voting.voting_status == Status.Ongoing)
                {
                    VotingViewModel viewModel = new VotingViewModel();
                    //Voting
                    viewModel.votingModel = voting;

                    //Candidate
                    var candidate = _candidateRepo.GetCandidateForVoting(VoteID);
                    //Check total candidate; if >= 1, then add to the ViewCandidate List
                    if (candidate.Count >= 1)
                    {
                        List<CandidateForVoting> candidateList = new List<CandidateForVoting>();
                        foreach (var row in candidate)
                        {
                            candidateList.Add(new CandidateForVoting
                            {
                                CandidateID = row.candidate_id,
                                VotingID = row.voting_id,
                                CandidateName = row.candidate_name,
                                CandidateVision = row.candidate_vision,
                                CandidateMission = row.candidate_mission,
                            });
                        }
                        viewModel.candidateModel = candidateList;
                        return View(viewModel);
                    }
                    ViewBag.Message = "Invalid Voting's ID";
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("VotingPage")]
        [ValidateAntiForgeryToken]
        public IActionResult Vote(CandidateForVoting vote, string VoteID)
        {
            if (ModelState.IsValid)
            {
                var voting = _voteRepo.GetVotingByID(VoteID);
                if (voting != null && voting.voting_status == Status.Ongoing && voting.end_date >= DateTime.Today)
                {
                    //Voting Logic

                    //Blockchain blockchain = new Blockchain();

                    //blockchain.AddBlock(new Block(DateTime.Now, null, $"votingID: {voting.voting_id}, ballot: {vote.selectedCandidate}, date: {DateTime.Now}"));
                    
                    //System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(blockchain, Formatting.Indented));

             
                    return RedirectToAction("VotingPage", "Voting");
                }
            }
            return RedirectToAction("VotingPage", "Voting");
        }

        [Authorize]
        public IActionResult Standings(string VoteID)
        {
            if (VoteID != null)
            {
                var voting = _voteRepo.GetVotingByID(VoteID);
                if (voting != null && voting.voting_status == Status.Pending || voting.voting_status == Status.Ongoing)
                {
                    VotingViewModel viewModel = new VotingViewModel();
                    //Voting
                    viewModel.votingModel = voting;

                    //Candidate
                    var candidate = _candidateRepo.GetCandidateForVoting(VoteID);
                    //Check total candidate; if >= 1, then add to the ViewCandidate List
                    if (candidate.Count >= 1)
                    {
                        List<CandidateForVoting> candidateList = new List<CandidateForVoting>();
                        foreach (var row in candidate)
                        {
                            candidateList.Add(new CandidateForVoting
                            {
                                CandidateID = row.candidate_id,
                                VotingID = row.voting_id,
                                CandidateName = row.candidate_name,
                                CandidateVision = row.candidate_vision,
                                CandidateMission = row.candidate_mission,
                            });
                        }
                        viewModel.candidateModel = candidateList;
                        return View(viewModel);
                    }
                    ViewBag.Message = "Invalid Voting's ID";
                    return View();
                }
            }
            return RedirectToAction("Dashboard", "Voting");
        }
    }
}
