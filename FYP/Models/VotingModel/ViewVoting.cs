using System.ComponentModel.DataAnnotations;

namespace FYP.Models.VotingModel
{
    public class ViewVoting
    {
		public string VotingID { get; set; }
		public string AdminID { get; set; }
		[Display(Name = "Title")]
		public string Title { get; set; }
		[Display(Name = "Description")]
		public string Description { get; set; }
		[Display(Name = "Total Voter")]
		public int TotalVoter { get; set; }
		[Display(Name = "Duration")]
		public int Duration { get; set; }
	}
}
