using System.ComponentModel.DataAnnotations;

namespace FYP.Models.VotingModel
{
    public class Main
    {
		public string VotingID { get; set; }
		public string AdminID { get; set; }

		//Voting
		[Display(Name = "Title")]
		public string Title { get; set; }
		[Display(Name = "Description")]
		public string Description { get; set; }
		[Display(Name = "Total Voter")]
		public int TotalVoter { get; set; }
		[Display(Name = "Duration")]
		public int Duration { get; set; }

		//Candidate
		public string CandidateID { get; set; }
		[Display(Name = "Name")]
		public string CandidateName { get; set; }
		[Display(Name = "Vision")]
		public string CandidateVision { get; set; }
		[Display(Name = "Mission")]
		public string CandidateMission { get; set; }
	}
}
