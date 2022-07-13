using FYP.DatabaseModel;
using FYP.Models.CandidateModel;
using System.Collections.Generic;

namespace FYP.Models.VotingModel
{
    public class VotingViewModel
    {
        public Voting votingModel { get; set; }
        public List<CandidateForVoting> candidateModel {get; set;}
    }
}
