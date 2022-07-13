using System.ComponentModel.DataAnnotations;

namespace FYP.Models.VotingModel
{
	public class VotingSearch
	{
		[Required]
		[Display(Name ="Voting's ID")]
		public string VotingID { get; set; }
	}
}
