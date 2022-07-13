using FYP.DatabaseModel;
using FYP.Models.CandidateModel;
using FYP.Models.VotingModel;
using System.Collections.Generic;

namespace FYP.Models.AdminModel
{
    public class HomepageViewModel
    {
        public Voting viewVoting { get; set; }
        public List<ViewCandidate> viewCandidate { get; set; }
    }
}
