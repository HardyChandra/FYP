using System.ComponentModel.DataAnnotations;

namespace FYP.Models.CandidateModel
{
    public class CandidateForVoting
	{ 
		public string CandidateID { get; set; }
		public string VotingID { get; set; }
		[Display(Name = "Name")]
		public string CandidateName { get; set; }
		[Display(Name = "Vision")]
		public string CandidateVision { get; set; }
		[Display(Name = "Mission")]
		public string CandidateMission { get; set; }

		public string selectedCandidate { get; set; }
	}
}
