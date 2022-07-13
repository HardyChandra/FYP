using System.ComponentModel.DataAnnotations;

namespace FYP.Models.VotingModel
{
	public class Create
	{
		public string VotingID { get; set; }
		public string AdminID { get; set; }

		//Voting
		[Required(ErrorMessage = "Please enter the voting's title")]
		[Display(Name = "Title")]
		public string Title { get; set; }
		[Required(ErrorMessage = "Please enter the voting's description")]
		[Display(Name = "Description")]
		public string Description { get; set; }
		[Required(ErrorMessage = "Please enter the voting's total voter")]
		[Display(Name = "Total Voter")]
		public int TotalVoter { get; set; }
		[Required(ErrorMessage = "Please enter the voting's duration")]
		[Display(Name = "Duration")]
		public int Duration { get; set; }
	}
}
