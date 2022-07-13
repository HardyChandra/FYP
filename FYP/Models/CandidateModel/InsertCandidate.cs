using System.ComponentModel.DataAnnotations;

namespace FYP.Models.CandidateModel
{
    public class InsertCandidate
    {
		public string CandidateID { get; set; }
		public string VotingID { get; set; }
		[Required(ErrorMessage = "Please enter the candidate's name")]
		[Display(Name = "Name")]
		public string CandidateName { get; set; }
		[Required(ErrorMessage = "Please enter the candidate's vision")]
		[Display(Name = "Vision")]
		public string CandidateVision { get; set; }
		[Required(ErrorMessage = "Please enter the candidate's mission")]
		[Display(Name = "Mission")]
		public string CandidateMission { get; set; }
	}
}
